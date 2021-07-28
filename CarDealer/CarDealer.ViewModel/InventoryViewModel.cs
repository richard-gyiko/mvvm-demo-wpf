using CarDealer.ViewModel.Entities;
using FluentValidation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

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


        private RelayCommand _addNewCarCommand;

        public RelayCommand AddNewCarCommand
        {
            get
            {
                return _addNewCarCommand ??= new RelayCommand(
                    execute: () =>
                    {
                        _cars.Insert(0, NewCar.ToCar());
                        NewCar.Reset();
                    },
                    canExecute: () => !NewCar.HasErrors && !NewCar.IsEmpty);
            }
        }

        private void NewCar_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            AddNewCarCommand.RaiseCanExecuteChanged();
        }
    }
}
