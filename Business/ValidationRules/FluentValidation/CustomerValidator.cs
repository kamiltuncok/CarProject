using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
   public class CustomerValidator:AbstractValidator<IndividualCustomer>
    {
        public CustomerValidator()
        {
            RuleFor(c=>c.FirstName).NotEmpty().WithMessage("Ad Boş Olamaz");
            RuleFor(c => c.LastName).NotEmpty().WithMessage("Soyad Boş Olamaz");
            RuleFor(c => c.IdentityNumber).NotEmpty().WithMessage("Tc Kimlik Numarası Boş Olamaz");
            RuleFor(c => c.PhoneNumber).NotEmpty().WithMessage("Telefon Numarası Boş Olamaz");
            RuleFor(c => c.Email).NotEmpty().WithMessage("Email Boş Olamaz");

            RuleFor(l => l.IdentityNumber).MinimumLength(11).WithMessage("Tc Kimlik Numarası 11 Karakter Olmalıdır");
        }
    }
}
