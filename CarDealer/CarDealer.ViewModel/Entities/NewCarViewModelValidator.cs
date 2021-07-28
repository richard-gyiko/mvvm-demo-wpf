using FluentValidation;

namespace CarDealer.ViewModel.Entities
{
    public class NewCarViewModelValidator : AbstractValidator<NewCarViewModel>
    {
        public NewCarViewModelValidator()
        {
            RuleFor(c => c.Vin).Length(min: 3, max: 10);
            RuleFor(c => c.Manufacturer).NotEmpty();
            RuleFor(c => c.Model).NotEmpty();
        }
    }
}
