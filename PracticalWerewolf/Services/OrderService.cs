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


namespace PracticalWerewolf.Services
{
    public class OrderService : IOrderService
    {
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

        public async void SetOrderAsInProgress(Guid orderId)
        {
            var order = OrderStore.Find(orderId);
            if (order != null)
            {
                order.TrackInfo.OrderStatus = OrderStatus.InProgress;
                OrderStore.Update(order);

                var customerId = order.RequestInfo.Requester.CustomerInfoGuid;
                var customer = UserManager.Users.Single(x => x.CustomerInfo.CustomerInfoGuid == customerId);

                await EmailService.SendOrderShippedEmail(order.RequestInfo, customer);
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

            await EmailService.SendOrderDeliveredEmail(order.RequestInfo, customer);
        }

        public IEnumerable<Order> GetOrderHistory(Guid customerInfoGuid)
        {
            return OrderStore
                .Find(x => x.RequestInfo.Requester.CustomerInfoGuid == customerInfoGuid)
                .OrderByDescending(x => x.RequestInfo.RequestDate);
        }
    }
}