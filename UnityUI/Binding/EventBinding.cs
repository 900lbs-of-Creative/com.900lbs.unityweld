﻿using System;
using System.Linq;
using UnityEngine;
using UnityUI.Binding;

namespace UnityUI.Binding
{
    /// <summary>
    /// Class for binding Unity UI events to methods in a view model.
    /// </summary>
    public class EventBinding : AbstractMemberBinding
    {
        /// <summary>
        /// Name of the method in the view model to bind to.
        /// </summary>
        public string viewModelMethodName;

        /// <summary>
        /// Type of the component we're binding to.
        /// Must be a string so because Types can't be serialised in the scene.
        /// </summary>
        public string boundComponentType;

        /// <summary>
        /// Name of the event to bind to.
        /// </summary>
        public string uiEventName;

        private EventBinder eventBinder;

        public override void Connect()
        {
            // Add self to event listener
            eventBinder = new EventBinder(this.gameObject, 
                viewModelMethodName, 
                uiEventName, 
                boundComponentType, 
                GetViewModelBinding());
        }

        public override void Disconnect()
        {
            if (eventBinder != null)
            {
                eventBinder.Dispose();
                eventBinder = null;
            }
        }

        void OnDestroy()
        {
            Disconnect();
        }
    }
}
