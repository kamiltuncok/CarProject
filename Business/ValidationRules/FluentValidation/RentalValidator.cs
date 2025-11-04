using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class RentalValidator : AbstractValidator<Rental>
    {
        public RentalValidator()
        {
            RuleFor(r => r.ReturnDate).NotEmpty();

            RuleFor(r => r)
                .Must(HaveValidUserOrCustomer)
                .WithMessage("UserId veya CustomerId gereklidir");

            RuleFor(r => r)
                .Must(NotHaveBothUserAndCustomer)
                .WithMessage("UserId ve CustomerId aynı anda kullanılamaz");

            RuleFor(r => r.RentDate)
                .NotEmpty().WithMessage("Kiralama tarihi gereklidir")
                .GreaterThan(DateTime.Now.AddHours(-1)).WithMessage("Kiralama tarihi geçmiş olamaz");

            RuleFor(r => r.CarId)
                .GreaterThan(0).WithMessage("Geçerli bir araç seçilmelidir");

            RuleFor(r => r.StartLocation)
                .NotEmpty().WithMessage("Başlangıç lokasyonu gereklidir")
                .MinimumLength(3).WithMessage("Başlangıç lokasyonu en az 3 karakter olmalıdır");

            RuleFor(r => r.EndLocation)
                .NotEmpty().WithMessage("Bitiş lokasyonu gereklidir")
                .MinimumLength(3).WithMessage("Bitiş lokasyonu en az 3 karakter olmalıdır");
        }

        private bool HaveValidUserOrCustomer(Rental rental)
        {
            return rental.UserId > 0 || rental.CustomerId > 0;
        }

        private bool NotHaveBothUserAndCustomer(Rental rental)
        {
            return !(rental.UserId > 0 && rental.CustomerId > 0);
        }
    }
}