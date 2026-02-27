using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class RentalValidator : AbstractValidator<Rental>
    {
        public RentalValidator()
        {
            RuleFor(r => r.CarId)
                .GreaterThan(0).WithMessage("Geçerli bir araç seçilmelidir");

            RuleFor(r => r.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId gereklidir");

            RuleFor(r => r.StartLocationId)
                .GreaterThan(0).WithMessage("Başlangıç lokasyonu gereklidir");

            RuleFor(r => r.EndLocationId)
                .GreaterThan(0).WithMessage("Bitiş lokasyonu gereklidir");

            RuleFor(r => r.StartDate)
                .NotEmpty().WithMessage("Başlangıç tarihi gereklidir");

            RuleFor(r => r.EndDate)
                .NotEmpty().WithMessage("Bitiş tarihi gereklidir")
                .GreaterThan(r => r.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır");

            RuleFor(r => r.RentedDailyPrice)
                .GreaterThan(0).WithMessage("Günlük fiyat 0'dan büyük olmalıdır");

            RuleFor(r => r.TotalPrice)
                .GreaterThan(0).WithMessage("Toplam fiyat 0'dan büyük olmalıdır");

            RuleFor(r => r.DepositAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Depozito miktarı negatif olamaz");
        }
    }
}