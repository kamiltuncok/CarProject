using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using Entities.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfLocationUserRoleDal : EfEntityRepositoryBase<LocationUserRole, RentACarContext>, ILocationUserRoleDal
    {
    }
}
