using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class LocationValidator:AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(l=>l.LocationName).NotEmpty().WithMessage("Konum İsmi Boş Olamaz");
            RuleFor(l => l.LocationName).MinimumLength(3).WithMessage("Konum İsmi Minimum 3 Karakter Olmalıdır");
        }
    }
}
