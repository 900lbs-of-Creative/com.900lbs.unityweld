﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityUI.Binding;
using UnityUI.Internal;

namespace UnityUI_Editor
{
    /// <summary>
    /// A base editor for Unity-Weld bindings.
    /// </summary>
    public class BaseBindingEditor : Editor
    {
        /// <summary>
        /// Sets the specified value and sets dirty to true if it doesn't match the old value.
        /// </summary>
        protected void UpdateProperty<TValue>(Action<TValue> setter, TValue oldValue, TValue newValue)
            where TValue : class
        {
            if (newValue != oldValue)
            {
                setter(newValue);

                InspectorUtils.MarkSceneDirty(((Component)target).gameObject);
            }
        }

        /// <summary>
        /// Display the adapters popup menu.
        /// </summary>
        protected static void ShowAdapterMenu(
            GUIContent label,
            string[] adapterTypeNames,
            string curValue,
            Action<string> valueUpdated
        )
        {
            var adapterMenu = new string[] { "None" }
                .Concat(adapterTypeNames)
                .Select(typeName => new GUIContent(typeName))
                .ToArray();

            var curSelectionIndex = Array.IndexOf(adapterTypeNames, curValue) + 1; // +1 to account for 'None'.
            var newSelectionIndex = EditorGUILayout.Popup(
                    label,
                    curSelectionIndex,
                    adapterMenu
                );

            if (newSelectionIndex != curSelectionIndex)
            {
                if (newSelectionIndex == 0)
                {
                    valueUpdated(null); // No adapter selected.
                }
                else
                {
                    valueUpdated(adapterTypeNames[newSelectionIndex - 1]); // -1 to account for 'None'.
                }
            }
        }

        /// <summary>
        /// Display a popup menu for selecting a property from a view-model.
        /// </summary>
        protected void ShowViewModelPropertyMenu(
            GUIContent label,
            AbstractMemberBinding target,
            PropertyInfo[] bindableProperties,
            Action<string> propertyValueSetter,
            string curPropertyValue,
            Func<PropertyInfo, bool> menuEnabled
        )
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);

            var dropdownPosition = GUILayoutUtility.GetLastRect();
            dropdownPosition.x += dropdownPosition.width;

            if (GUILayout.Button(new GUIContent(curPropertyValue, label.tooltip), EditorStyles.popup))
            {
                InspectorUtils.ShowMenu<PropertyInfo>(
                    property => property.ReflectedType + "/" + property.Name + " : " + property.PropertyType.Name,
                    menuEnabled,
                    property => property.ReflectedType.Name + "." + property.Name == curPropertyValue,
                    property => UpdateProperty(
                        propertyValueSetter,
                        curPropertyValue,
                        property.ReflectedType.Name + "." + property.Name
                    ),
                    bindableProperties
                        .OrderBy(property => property.ReflectedType.Name)
                        .ThenBy(property => property.Name)
                        .ToArray(),
                    dropdownPosition
                );
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Shows a dropdown for selecting a property in the UI to bind to.
        /// </summary>
        public void ShowViewPropertyMenu(
            GUIContent label, 
            AbstractMemberBinding targetScript, 
            BindablePropertyInfo[] properties, 
            Action<string> propertyValueSetter,
            string curPropertyValue,
            out Type selectedPropertyType
        )
        {
            var propertyNames = properties
                .Select(prop => prop.PropertyInfo.ReflectedType.Name + "." + prop.PropertyInfo.Name)
                .ToArray();
            var selectedIndex = Array.IndexOf(propertyNames, curPropertyValue);
            var content = properties.Select(prop => new GUIContent(
                    prop.PropertyInfo.ReflectedType.Name + "/" +
                    prop.PropertyInfo.Name + " : " +
                    prop.PropertyInfo.PropertyType.Name
                ))
                .ToArray();

            var newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, content);
            if (newSelectedIndex != selectedIndex)
            {
                var newSelectedProperty = properties[newSelectedIndex];

                UpdateProperty(
                    propertyValueSetter,
                    curPropertyValue,
                    newSelectedProperty.PropertyInfo.ReflectedType.Name + "." + newSelectedProperty.PropertyInfo.Name
                );

                selectedPropertyType = newSelectedProperty.PropertyInfo.PropertyType;
            }
            else
            {
                if (selectedIndex < 0)
                {
                    selectedPropertyType = null;
                    return;
                }

                selectedPropertyType = properties[selectedIndex].PropertyInfo.PropertyType;
            }
        }

        /// <summary>
        /// Show dropdown for selecting a UnityEvent to bind to.
        /// </summary>
        protected void ShowEventMenu(
            AbstractMemberBinding targetScript, 
            BindableEvent[] events,
            Action<string> propertyValueSetter,
            string curPropertyValue
        )
        {
            var eventNames = events
                .Select(evt => evt.ComponentType.Name + "." + evt.Name)
                .ToArray();
            var selectedIndex = Array.IndexOf(eventNames, curPropertyValue);
            var content = events
                .Select(evt => new GUIContent(evt.ComponentType.Name + "." + evt.Name))
                .ToArray();

            var newSelectedIndex = EditorGUILayout.Popup(
                new GUIContent("View event", "Event on the view to bind to."),
                selectedIndex,
                content
            );

            if (newSelectedIndex != selectedIndex)
            {
                var selectedEvent = events[newSelectedIndex];
                UpdateProperty(
                    propertyValueSetter,
                    curPropertyValue,
                    selectedEvent.ComponentType.Name + "." + selectedEvent.Name
                );
            }
        }

        /// <summary>
        /// Find the adapter attribute for a named adapter type.
        /// </summary>
        protected AdapterAttribute FindAdapterAttribute(string adapterName)
        {
            if (!string.IsNullOrEmpty(adapterName))
            {
                var adapterType = TypeResolver.FindAdapterType(adapterName);
                if (adapterType != null)
                {
                    return TypeResolver.FindAdapterAttribute(adapterType);
                }
            }

            return null;
        }

        /// <summary>
        /// Pass a type through an adapter and get the result.
        /// </summary>
        protected Type AdaptTypeBackward(Type inputType, string adapterName)
        {
            var adapterAttribute = FindAdapterAttribute(adapterName);
            if (adapterAttribute != null)
            {
                return adapterAttribute.InputType;
            }

            return inputType;
        }

        /// <summary>
        /// Pass a type through an adapter and get the result.
        /// </summary>
        protected Type AdaptTypeForward(Type inputType, string adapterName)
        {
            var adapterAttribute = FindAdapterAttribute(adapterName);
            if (adapterAttribute != null)
            {
                return adapterAttribute.OutputType;
            }

            return inputType;
        }
    }
}
