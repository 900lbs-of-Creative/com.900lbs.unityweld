using System.Collections.ObjectModel;
using UnityEngine;

using UnityWeld.Binding;

namespace UnityWeld.Examples.Collections
{
    [Binding]
    public class ExampleViewModel : MonoBehaviour
    {
        private ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>()
        {
            new ItemViewModel("1"),
            new ItemViewModel("2"),
            new ItemViewModel("3")
        };

        [Binding]
        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return items;
            }
        }
    }
}
