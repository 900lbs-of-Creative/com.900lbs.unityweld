using UnityWeld.Binding;

namespace UnityWeld.Examples.Collections
{
    [Binding]
    public class ItemViewModel
    {
        [Binding]
        public string DisplayText
        {
            get;
            private set;
        }

        public ItemViewModel(string displayText)
        {
            DisplayText = displayText;
        }
    }
}
