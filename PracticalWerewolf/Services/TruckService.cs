﻿using PracticalWerewolf.Stores.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class TruckService : ITruckManager
    {
        private ITruckStore TruckStore;

        public TruckService(ITruckStore store)
        {
            TruckStore = store;
        }
    }
}