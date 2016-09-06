﻿using System;
using System.Linq;
using UnityEngine;

namespace UnityUI.Binding
{
    /// <summary>
    /// Base class for binders to Unity MonoBehaviours.
    /// </summary>
    public abstract class AbstractMemberBinding : MonoBehaviour, IMemberBinding
    {
        /// <summary>
        /// Name of the view model to bind.
        /// </summary>
        public string viewModelName;

        /// <summary>
        /// Initialise this binding. Used when we first start the scene.
        /// Detaches any attached view models, finds available view models afresh and then connects the binding.
        /// </summary>
        public virtual void Init()
        {
            Disconnect();

            Connect();
        }

        /// <summary>
        /// Helper method to get the voiw model object from the connected ViewModelBinding.
        /// </summary>
        protected object GetViewModel()
        {
            return GetViewModelBinding().BoundViewModel;
        }

        /// <summary>
        /// Scan up the hierarchy to get the view model corrosponding to the name set in viewModelName.
        /// </summary>
        protected IViewModelBinding GetViewModelBinding()
        {
            var trans = transform;
            while (trans != null)
            {
                var components = trans.GetComponents<MonoBehaviour>();
                var boundMonoBehaviour = components.Where(component => component.GetType().Name == viewModelName)
                    .FirstOrDefault();
                if (boundMonoBehaviour != null)
                {
                    return new MonoBehaviourBinding(boundMonoBehaviour);
                }

                var newViewModelBinding = components                    
                    .Select(component => component as IViewModelBinding)
                    .Where(component => component != null)
                    .Where(viewModelBinding => viewModelBinding.ViewModelTypeName == viewModelName && (object)viewModelBinding != this)
                    .FirstOrDefault();
                if (newViewModelBinding != null)
                {
                    return newViewModelBinding;
                }


                // Stop if we found what we're looking for or have reached the top level.
                if (trans.GetComponent<BindingRoot>() != null)
                {
                    break;
                }

                trans = trans.parent;
            }

            throw new ApplicationException(string.Format("Tried to get view {0} but it could not be found on "
                + "object {1}. Check that a ViewModelBinding for that view exists further up in "
                + "the scene hierarchy. ", viewModelName, gameObject.name)
            );
        }

        /// <summary>
        /// Connect to all the attached view models
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// Disconnect from all attached view models.
        /// </summary>
        public abstract void Disconnect();
    }
}
