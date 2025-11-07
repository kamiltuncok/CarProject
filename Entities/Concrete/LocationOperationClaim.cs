using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class LocationOperationClaim:IEntity
    {
        public int LocationOperationClaimId { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
    }
}
