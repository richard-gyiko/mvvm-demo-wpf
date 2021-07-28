using Bogus;
using System.Collections.Generic;

namespace CarDealer.ViewModel.Entities
{
    public class Car
    {
        public string Vin { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public static IList<Car> Generate(int count) => _faker.Generate(count);

        private static readonly Faker<Car> _faker = new Faker<Car>()
            .RuleFor(c => c.Vin, f => f.Vehicle.Vin())
            .RuleFor(c => c.Manufacturer, f => f.Vehicle.Manufacturer())
            .RuleFor(c => c.Model, f => f.Vehicle.Model());
    }
}
