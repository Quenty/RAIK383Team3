﻿using PracticalWerewolf.Repository.Interfaces;
using PracticalWerewolf.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Services
{
    public class TruckManager : ITruckManager
    {
        private ITruckStore TruckStore;

        public TruckManager(ITruckStore store)
        {
            TruckStore = store;
        }
    }
}