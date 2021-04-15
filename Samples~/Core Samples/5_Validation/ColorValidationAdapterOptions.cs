using UnityEngine;
using UnityEngine.UI;

namespace UnityWeld.Examples.Validation
{
    [CreateAssetMenu(menuName = "Unity Weld/Examples/Color Validation Adapter Options")]
    public class ColorValidationAdapterOptions : ScriptableObject
    {
        [Header("Normal Color")]
        public ColorBlock NormalColorBlock;

        [Header("Invalid Color")]
        public ColorBlock InvalidColorBlock;
    }
}
