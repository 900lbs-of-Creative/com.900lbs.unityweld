using System;
using UnityEngine.UI;
using UnityWeld.Binding.Adapters;

namespace UnityWeld.Examples.Validation
{
    public class ColorValidationAdapter : IAdapterInfo
    {
        public string Id => "ColorValidation";

        public Type InType => typeof(bool);

        public Type OutType => typeof(ColorBlock);

        public Type OptionsType => typeof(ColorValidationAdapterOptions);

        public object Convert(object valueIn, object options)
        {
            var isValid = (bool)valueIn;
            var colorValidationAdapterOptions = (ColorValidationAdapterOptions)options;

            return isValid ? colorValidationAdapterOptions.NormalColorBlock : colorValidationAdapterOptions.InvalidColorBlock;
        }
    }
}
