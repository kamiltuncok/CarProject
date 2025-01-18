using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IIndividualCustomerService : IGenericService<IndividualCustomer>
    {
        IDataResult<IndividualCustomer> AddCustomer(IndividualCustomer entity);
    }
}
