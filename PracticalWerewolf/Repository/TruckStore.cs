using PracticalWerewolf.Models;
using PracticalWerewolf.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Repository
{
    public class TruckStore : ITruckStore
    {
        private ApplicationDbContext Db;

        public TruckStore(ApplicationDbContext dbContext)
        {
            this.Db = dbContext;
        }

        public void Add(IEnumerable<ITruck> truckList)
        {
            throw new NotImplementedException();
        }

        public void Add(ITruck truck)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<ITruck> truckList)
        {
            throw new NotImplementedException();
        }

        public void Delete(ITruck Truck)
        {
            throw new NotImplementedException();
        }

        public List<ITruck> Get()
        {
            throw new NotImplementedException();
        }

        public ITruck Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<ITruck> truckList)
        {
            throw new NotImplementedException();
        }

        public void Update(ITruck truck)
        {
            throw new NotImplementedException();
        }
    }
}