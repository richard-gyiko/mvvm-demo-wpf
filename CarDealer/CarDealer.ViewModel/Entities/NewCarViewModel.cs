using CarDealer.ViewModel.Abstractions;
using FluentValidation;
using System;

namespace CarDealer.ViewModel.Entities
{
    public class NewCarViewModel : ValidatableObservableObject
    {
        private readonly IValidator<NewCarViewModel> _validator;

        private string _vin;

        public string Vin
        {
            get => _vin;
            set => SetProperty(ref _vin, value);
        }


        private string _model;

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }


        private string _manufacturer;

        public string Manufacturer
        {
            get => _manufacturer;
            set => SetProperty(ref _manufacturer, value);
        }


        public bool IsEmpty => Manufacturer == null && Model == null && Vin == null;

        public NewCarViewModel(IValidator<NewCarViewModel> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public Car ToCar() => new Car { Manufacturer = _manufacturer, Model = _model, Vin = _vin };

        public void Reset()
        {
            Manufacturer = null;
            Model = null;
            Vin = null;

            Clear();
        }

        protected override FluentValidation.Results.ValidationResult ValidateCore() => _validator.Validate(this);
    }
}
