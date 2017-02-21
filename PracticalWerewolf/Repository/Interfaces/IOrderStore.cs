using PracticalWerewolf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWerewolf.Repository.Interfaces
{
    public interface IOrderStore
    {
        //Get all Order
        IEnumerable<IOrder> Get();

        //Get Order by guid
        IOrder Get(Guid guid);

        //Batch Get Orders by guid
        IEnumerable<IOrder> Get(IEnumerable<Guid> guids);

        //Get Order by truck
        IEnumerable<IOrder> GetByTruck(ITruck truck);

        //Add Order
        void Add(IOrder order);

        //Batch Add Order
        void Add(IEnumerable<IOrder> orderList);

        //Update Order
        void Update(IOrder order);

        //Batch Update Order
        void Update(IEnumerable<IOrder> orderList);

        //Delete Order
        void Delete(IOrder order);

        //Batch DeleteContactorInfo
        void Delete(IEnumerable<IOrder> orderList);
    }
}
