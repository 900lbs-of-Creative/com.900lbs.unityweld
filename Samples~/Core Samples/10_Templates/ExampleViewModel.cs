using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace UnityWeld.Examples.Templates
{
    [Binding]
    public class ExampleViewModel : MonoBehaviour, INotifyPropertyChanged 
    {
        [Binding]
        public NameTab NameTab { get; } = new NameTab();

        [Binding]
        public AddressTab AddressTab { get; } = new AddressTab();

        [Binding]
        public Tab ActiveTab
        {
            get
            {
                return activeTab;
            }
            set 
            {
                if (activeTab == value)
                {
                    return;
                }

                activeTab = value;

                OnPropertyChanged("ActiveTab");
            }
        }
        private Tab activeTab = null;

        [Binding]
        public void ActivateNameTab()
        {
            ActiveTab = NameTab;
        }

        [Binding]
        public void ActivateAddressTab()
        {
            ActiveTab = AddressTab;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
