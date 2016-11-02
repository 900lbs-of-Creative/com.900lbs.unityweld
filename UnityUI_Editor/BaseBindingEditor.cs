﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityUI.Binding;

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
            if (!newValue.Equals(oldValue))
            {
                setter(newValue);

                InspectorUtils.MarkSceneDirty(((Component)target).gameObject);
            }
        }

        /// <summary>
        /// Display the adapters popup menu.
        /// </summary>
        protected static void ShowAdapterMenu(
            string label,
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
                    new GUIContent(label),
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
            string label,
            AbstractMemberBinding target,
            PropertyInfo[] bindableProperties,
            Action<string> propertyNameSetter,
            string propertyName,
            Func<PropertyInfo, bool> menuEnabled
        )
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);

            var dropdownPosition = GUILayoutUtility.GetLastRect();
            dropdownPosition.x += dropdownPosition.width;

            if (GUILayout.Button(new GUIContent(propertyName), EditorStyles.popup))
            {
                InspectorUtils.ShowMenu<PropertyInfo>(
                    property => property.ReflectedType + "/" + property.Name + " : " + property.PropertyType.Name,
                    menuEnabled,
                    property => property.ReflectedType.Name + "." + property.Name == propertyName,
                    property => UpdateProperty(
                        propertyNameSetter,
                        propertyName,
                        property.ReflectedType.Name + "." + property.Name
                    ),
                    bindableProperties,
                    dropdownPosition
                );
            }

            EditorGUILayout.EndHorizontal();
        }

    }
}
