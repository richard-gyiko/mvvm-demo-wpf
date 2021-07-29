using FluentValidation.Results;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CarDealer.ViewModel.Abstractions
{
    public abstract class ValidatableObservableObject : ObservableObject, INotifyDataErrorInfo
    {
        #region events
        /// <summary>
        /// See <see cref="INotifyDataErrorInfo.ErrorsChanged"/>.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion


        #region members
        private Dictionary<string, string> _errors = new Dictionary<string, string>();
        #endregion


        #region properties
        internal IReadOnlyDictionary<string, string> Errors => _errors;

        /// <summary>
        /// Gets whether validation executes <see cref="INotifyPropertyChanged.PropertyChanged"/> or manually invoked.
        /// </summary>
        public bool ValidateOnPropertyChanged { get; set; } = true;

        /// <summary>
        /// See <see cref="INotifyDataErrorInfo.HasErrors"/>
        /// </summary>
        public bool HasErrors => _errors.Count > 0;
        #endregion


        /// <summary>
        /// See <see cref="INotifyDataErrorInfo.GetErrors(string)"/>.
        /// </summary>
        /// <param name="propertyName">See <see cref="INotifyDataErrorInfo.GetErrors(string)"/>.</param>
        /// <returns>See <see cref="INotifyDataErrorInfo.GetErrors(string)"/>.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return new[] { _errors[propertyName] };
        }


        #region overrides
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (ValidateOnPropertyChanged)
            {
                Validate();
            }

            base.OnPropertyChanged(e);
        }
        #endregion


        public ValidationResult Validate()
        {
            var validationResult = ValidateCore();

            UpdateValidationState(validationResult);

            return validationResult;
        }


        protected void Clear()
        {
            UpdateValidationState(new ValidationResult());
        }


        #region abstracts
        protected abstract ValidationResult ValidateCore();
        #endregion


        #region helpers
        private void UpdateValidationState(ValidationResult result)
        {
            var errorsChangedKeys = new HashSet<string>(_errors.Keys);

            if (result == null || result.IsValid)
            {
                _errors = new Dictionary<string, string>();
            }
            else
            {
                _errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        keySelector: group => group.Key,
                        elementSelector: group => group.Aggregate(new StringBuilder(), (prev, next) => prev.AppendLine(next.ErrorMessage)).ToString().TrimEnd(Environment.NewLine.ToCharArray()));

                foreach (var error in _errors.Keys)
                {
                    if (!errorsChangedKeys.Contains(error))
                    {
                        errorsChangedKeys.Add(error);
                    }
                }
            }

            RaiseErrosChanged(errorsChangedKeys);
        }


        private void RaiseErrosChanged(ICollection<string> properties)
        {
            foreach (var property in properties)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
            }
        }
        #endregion
    }
}
