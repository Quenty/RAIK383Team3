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

    public class ContractorApprovalModel
    {
        ContractorInfo ApprovedContractor { get; set; }
        Boolean IsApproved { get; set; }
    }

    public class PendingContractorsModel
    {
        public IEnumerable<ContractorInfo> Pending { get; set; }
    }

    public class ContractorIndexModel
    {
        public ContractorInfo ContractorInfo { get; set; }
    }
}