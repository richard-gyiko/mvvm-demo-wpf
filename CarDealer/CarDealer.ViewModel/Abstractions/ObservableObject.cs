using System.Runtime.CompilerServices;

namespace System.ComponentModel
{
    /// <summary>
    /// Represents a bindable class.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property is changed on a component.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event while sets the given property with the given property value.
        /// </summary>
        /// <typeparam name="T">The type of property.</typeparam>
        /// <param name="storage">The property backing store.</param>
        /// <param name="value">The property value.</param>
        /// <param name="needsEqualityCheck">Determines whether equality check is required before assign the value on the backing store.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, bool needsEqualityCheck = true, [CallerMemberName] string propertyName = null)
        {
            if (needsEqualityCheck && Equals(storage, value))
            {
                return false;
            }

            storage = value;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Invoked when a property changed.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }
    }
}
