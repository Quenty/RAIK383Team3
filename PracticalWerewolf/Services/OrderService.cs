using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Stores.Interfaces;
using System.Text;
using log4net;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Models.UserInfos;
using System.Linq.Expressions;

namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
        private static readonly decimal COST_PER_POUND_MILE = 0.05m;
        private static readonly decimal METERS_PER_MILE = 1609.344m;

        private static ILog logger = LogManager.GetLogger(typeof(OrderService));
        private readonly IOrderStore OrderStore;
        private readonly IContractorStore ContractorStore;
        private readonly IOrderTrackInfoStore OrderTrackInfoStore;
        private readonly ApplicationUserManager UserManager;
        private readonly EmailService EmailService;

        public OrderService (IOrderStore orderStore, IContractorStore contractorStore, IOrderTrackInfoStore orderTrackInfoStore, ApplicationUserManager userManager, EmailService emailService)
        {
            this.OrderStore = orderStore;
            this.ContractorStore = contractorStore;
            this.OrderTrackInfoStore = orderTrackInfoStore;
            this.UserManager = userManager;
            this.EmailService = emailService;
        }

        public int QueryCount(Expression<Func<Order, bool>> where)
        {
            return this.OrderStore.AsQueryable().Where(where).Count();
        }

        public IEnumerable<Order> GetDeliveredOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).ToList();
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Complete).ToList();
        }

        public IEnumerable<Order> GetInprogressOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).ToList();
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.InProgress).ToList();
        }

        public IEnumerable<Order> GetInprogressOrdersNoTruck(ContractorInfo contractorinfo)
        {
            var assignee = ContractorStore.Single(o => o.ContractorInfoGuid == contractorinfo.ContractorInfoGuid);
            return OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractorinfo.ContractorInfoGuid).ToList()
            .Where(o => o.TrackInfo.OrderStatus == OrderStatus.InProgress)
            .Where(o => o.TrackInfo.CurrentTruck != assignee.Truck);
        }

        public IEnumerable<Order> GetInprogressOrdersNoTruck(Guid guid)
        {
            var assignee = ContractorStore.Single(o => o.ContractorInfoGuid == guid);
            return GetInprogressOrdersNoTruck(assignee);
        }

        public IEnumerable<Order> GetInprogressOrdersInTruck(ContractorInfo contractor)
        {
            return OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).ToList()
                .Where(o => o.TrackInfo.OrderStatus == OrderStatus.InProgress)
                .Where(o => o.TrackInfo.CurrentTruck != null)
                .Where(o => o.TrackInfo.CurrentTruck.TruckGuid == contractor.Truck.TruckGuid);
        }
        public Order GetOrder(Guid orderGuid)
        {
            Order order = OrderStore.Single(o => o.OrderGuid == orderGuid);
            if (order == null)
            {
                throw new ArgumentException("Order does not exist in database");
            }
            return order;
        }

        public void CancelOrder(Guid orderGuid)
        {
            Order order = GetOrder(orderGuid);

            OrderTrackInfo orderTrackInfo = order.TrackInfo ?? new OrderTrackInfo();
            orderTrackInfo.OrderStatus = OrderStatus.Cancelled;
            OrderStore.Update(order);
        }

        public void AssignOrder(Guid orderGuid, ContractorInfo contractor)
        {
            Order order = GetOrder(orderGuid);
            
            if(order.TrackInfo == null)
            {
                order.TrackInfo = new OrderTrackInfo{ OrderTrackInfoGuid = Guid.NewGuid() };
                OrderTrackInfoStore.Insert(order.TrackInfo);
            } else
            {
                OrderTrackInfoStore.Update(order.TrackInfo);
            }

            OrderTrackInfo orderTrackInfo = order.TrackInfo;
            orderTrackInfo.OrderStatus = OrderStatus.InProgress;
            orderTrackInfo.Assignee = contractor;
        }

        public void UnassignOrder(Order order)
        {
            order.TrackInfo.Assignee = null;
            order.TrackInfo.OrderStatus = OrderStatus.Queued;
            OrderTrackInfoStore.Update(order.TrackInfo);
        }

        public void AssignOrders()
        {
            var Orders = GetUnassignedOrders().ToList();
            
            // Initial slow, trivial algorithm
            foreach (Order order in Orders)
            {
                double volume = order.RequestInfo.Size.Volume;
                IQueryable<ContractorInfo> ContractorQuery = ContractorStore.getAvailableContractorsQuery()
                    .Where(x => x.Truck.MaxCapacity.Volume >= x.Truck.UsedCapacity.Volume + volume);

                ContractorInfo contractorInfo = ContractorQuery.FirstOrDefault();
                if (contractorInfo != null)
                {
                    AssignOrder(order.OrderGuid, contractorInfo);
                }
                else
                {
                    logger.Warn("Failed to assign order, no available contractors");
                }
            }
        }

        public IEnumerable<Order> GetUnassignedOrders()
        {
            return OrderStore.Find(o => o.TrackInfo.OrderStatus == OrderStatus.Queued, 
                o => o.TrackInfo, 
                o => o.TrackInfo.Assignee,
                o => o.RequestInfo,
                o => o.RequestInfo.Size)
                .Where(o => o.TrackInfo.Assignee == null);
        }

        public IEnumerable<Order> GetQueuedOrders(ContractorInfo contractor)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractor.ContractorInfoGuid).ToList();
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Queued).ToList();
        }

        public IEnumerable<Order> GetQueuedOrders(Guid contractorInfoGuid)
        {
            var allOrders = OrderStore.Find(o => o.TrackInfo.Assignee.ContractorInfoGuid == contractorInfoGuid).ToList();
            return allOrders.Where(o => o.TrackInfo.OrderStatus == OrderStatus.Queued).ToList();
        }

        public async void SetOrderAsInProgress(Guid orderId)
        {
            var order = OrderStore.Find(orderId);
            if (order != null)
            {
                order.TrackInfo.OrderStatus = OrderStatus.InProgress;
                OrderStore.Update(order);

                var customerId = order.RequestInfo.Requester.CustomerInfoGuid;
                var customer = UserManager.Users.Single(x => x.CustomerInfo.CustomerInfoGuid == customerId);

                var cost = CalculateOrderCost(order);
                await EmailService.SendOrderShippedEmail(order, customer, cost);
            }
            else
            {
                logger.Error($"SetOrderAsInProgress() - No order with id {orderId.ToString()}");
            }
        }

        public object GetOrders()
        {
            return OrderStore.GetAll().ToList();
        }

        public object GetOrders(CustomerInfo customerInfo)
        {
            return OrderStore.Find(o => o.RequestInfo.Requester.CustomerInfoGuid == customerInfo.CustomerInfoGuid);
        }

        public async void SetOrderAsComplete(Guid guid)
        {
            Order order = GetOrder(guid);
            OrderTrackInfo orderTrackInfo = order.TrackInfo;
            orderTrackInfo.OrderStatus = OrderStatus.Complete;
            OrderStore.Update(order);

            var customerId = order.RequestInfo.Requester.CustomerInfoGuid;
            var customer = UserManager.Users.Single(x => x.CustomerInfo.CustomerInfoGuid == customerId);

            var cost = CalculateOrderCost(order);
            await EmailService.SendOrderDeliveredEmail(order, customer, cost);
        }

        public IEnumerable<Order> GetOrderHistory(Guid customerInfoGuid)
        {
            return OrderStore
                .Find(x => x.RequestInfo.Requester.CustomerInfoGuid == customerInfoGuid)
                .OrderByDescending(x => x.RequestInfo.RequestDate);
        }

        public void CreateOrder(Order order)
        {
            OrderStore.Insert(order);
        }

        public decimal CalculateOrderCost(Guid orderGuid)
        {
            Order order = GetOrder(orderGuid);

            return CalculateOrderCost(order);
        }

        public decimal CalculateOrderCost(Order order)
        {
            var directions = LocationHelper.GetRouteBetweenLocations(order.RequestInfo.PickUpAddress, order.RequestInfo.DropOffAddress);

            var miles = directions.Routes.First().Legs.First().Distance.Value / METERS_PER_MILE;

            return (decimal)order.RequestInfo.Size.Mass * miles * COST_PER_POUND_MILE;
        }
    }
}
