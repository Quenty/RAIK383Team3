using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.ViewModels.Contractor
{
    public class ContractorRegisterModel
    {
        
    }

    public class ContractorIndexModel
    {
        public bool HasContractorInfo { get; set;}
        public ContractorInfo ContractorInfo { get; set; }
    }
}