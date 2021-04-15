using System;
using UnityWeld.Binding.Adapters;

namespace UnityWeld.Examples.Validation
{
    public class ExceptionValidationAdapter : IAdapterInfo
    {
        public string Id => "ExceptionValidation";

        public Type InType => typeof(Exception);

        public Type OutType => typeof(bool);

        public Type OptionsType => null;

        public object Convert(object valueIn, object options)
        {
            return (Exception)valueIn == null;
        }
    }
}
