using System.ComponentModel;
using UnityEngine;

using UnityWeld.Binding;

namespace UnityWeld.Examples.Dropdowns
{
    [Binding]
    public class ExampleViewModel : MonoBehaviour, INotifyPropertyChanged
    {
        private string[] options = new string[]
        {
        "Option-A",
        "Option-B",
        "Option-C"
        };

        private string selectedItem = "Option-B";

        [Binding]
        public string SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem == value)
                {
                    return; // No change.
                }

                selectedItem = value;

                OnPropertyChanged("SelectedItem");
            }
        }

        public string[] Options
        {
            get
            {
                return options;
            }
        }

        /// <summary>
        /// Event to raise when a property's value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
