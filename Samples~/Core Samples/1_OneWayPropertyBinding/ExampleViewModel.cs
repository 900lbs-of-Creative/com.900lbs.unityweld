using System.ComponentModel;
using UnityEngine;

using UnityWeld.Binding;

namespace UnityWeld.Examples.OneWayPropertyBinding
{
    [Binding]
    public class ExampleViewModel : MonoBehaviour, INotifyPropertyChanged
    {
        private float timer = 0;

        private string text = "<Type some text>";

        [Binding]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text == value)
                {
                    return; // No change.
                }

                text = value;

                OnPropertyChanged("Text");
            }
        }

        /// <summary>
        /// Event to raise when a property's value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        void Start()
        {
            SetRandomText();
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                SetRandomText();
                timer = 0f;
            }        
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetRandomText()
        {
            Text = Random.Range(0f, 10000f).ToString();
        }
    }
}
