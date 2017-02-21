﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UnityWeld.Binding
{
    /// <summary>
    /// Watches an object for property changes and invokes an action when the property has changed.
    /// </summary>
    public class PropertyWatcher : IDisposable
    {
        /// <summary>
        /// The object that owns the property that is being watched.
        /// </summary>
        private object propertyOwner;

        /// <summary>
        /// The name of the property that is being watched.
        /// </summary>
        private string propertyName;

        /// <summary>
        /// The action to invoke when the property has changed.
        /// </summary>
        private Action action;

        public PropertyWatcher(object propertyOwner, string propertyName, Action action)
        {
            this.propertyOwner = propertyOwner;
            this.propertyName = propertyName;
            this.action = action;

            var notifyPropertyChanged = propertyOwner as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                notifyPropertyChanged.PropertyChanged += propertyOwner_PropertyChanged;
            }
        }

        public void Dispose()
        {
            if (propertyOwner != null)
            {
                var notifyPropertyChanged = propertyOwner as INotifyPropertyChanged;
                if (notifyPropertyChanged != null)
                {
                    notifyPropertyChanged.PropertyChanged -= propertyOwner_PropertyChanged;
                }

                propertyOwner = null;
            }
        }

        private void propertyOwner_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
            {
                action();
            }
        }
    }
}
