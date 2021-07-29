using CarDealer.ViewModel.Entities;
using FluentValidation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CarDealer.ViewModel
{
    public class InventoryViewModel : ObservableObject
    {
        private readonly ObservableCollection<Car> _cars;

        public ReadOnlyObservableCollection<Car> Cars { get; }

        public NewCarViewModel NewCar { get; }

        public InventoryViewModel(IValidator<NewCarViewModel> carValidator)
        {
            Cars = new ReadOnlyObservableCollection<Car>(_cars ??= new ObservableCollection<Car>());

            _cars.AddRange(Car.Generate(15));

            NewCar = new NewCarViewModel(carValidator);
            NewCar.ErrorsChanged += NewCar_ErrorsChanged;
        }


        private IAsyncRelayCommand _addNewCarCommand;

        public IAsyncRelayCommand AddNewCarCommand => _addNewCarCommand ??= new AsyncRelayCommand(
            execute: AddNewCarAsync,
            canExecute: () => !NewCar.HasErrors && !NewCar.IsEmpty);

        private async Task AddNewCarAsync()
        {
            var car = NewCar.ToCar();

            // Emulate API call
            await Task.Delay(750);

            _cars.Insert(0, car);

            NewCar.Reset();
        }


        private void NewCar_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            AddNewCarCommand.NotifyCanExecuteChanged();
        }
    }
}
