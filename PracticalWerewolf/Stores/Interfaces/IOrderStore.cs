using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Trucks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface IOrderStore
    {
        //Get all Order
        IEnumerable<Order> Get();

        //Get Order by guid
        Order Get(Guid guid);

        //Batch Get Orders by guid
        IEnumerable<Order> Get(IEnumerable<Guid> guids);

        //Batch Get Orders by guid
        IEnumerable<Order> GetByUserGuids(IEnumerable<Guid> userGuids);

        //Batch Get Orders by guid
        IEnumerable<Order> Get(OrderStatus orderStatus);

        //Get Order by truck
        IEnumerable<Order> GetByTruck(Truck truck);

        //Add Order
        void Add(Order order);

        //Batch Add Order
        void Add(IEnumerable<Order> orderList);

        //Update Order
        void Update(Order order);

        //Batch Update Order
        void Update(IEnumerable<Order> orderList);

        IEnumerable<Order> GetOrdersByContractorInfoGuid(Guid contractorInfoGuid);
        IEnumerable<Order> GetOrdersByCustomerInfoGuid(Guid customerInfoGuid);

        //Delete Order
        void Delete(Order order);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<Order> orderList);
    }
}
