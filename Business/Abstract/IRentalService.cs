using Core.Utilities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
   public interface IRentalService
    {
        IDataResult<List<Rental>> GetAll();
        IDataResult<List<Rental>> GetById(int rentalid);
        IResult Add(Rental rental);
        IResult Delete(Rental rental);
        IResult Update(Rental rental);
        IDataResult<List<Rental>> GetRentalsByCarId(int carId);
        IDataResult<List<RentalDetailDto>> GetRentalDetailsByUserId(int userId, CustomerType customerType);
        IDataResult<List<RentalDetailDto>> GetRentalDetailsByLocationName(string locationName);
        IResult AddBulk(List<Rental> rentals);
        IDataResult<List<Rental>> GetRentalsByStartDate(DateTime startDate);
        IDataResult<List<RentalDetailDto>> GetRentalsByEmail(string email);
        IDataResult<List<RentalDetailDto>> GetRentalsByName(string name);
        IDataResult<List<RentalDetailDto>> GetRentalsByDateRange(DateTime startDate, DateTime endDate);
        //IDataResult<List<RentalDetailDto>> GetRentalDetails();
    }
}
