using UnityWeld.Binding.Internal;
using UnityWeld.Examples.Validation;

namespace UnityWeld.Examples
{
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    #endif
    public static class UnityWeldIoC
    {
    #if UNITY_EDITOR
        static UnityWeldIoC()
        {
            Register();
        }
    #endif

        private static void Register()
        {
            TypeResolver.RegisterAdapter(new ColorValidationAdapter());
            TypeResolver.RegisterAdapter(new ExceptionValidationAdapter());
        }

    #if !UNITY_EDITOR
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            static void Initialize()
            {
                Register();
            }
    #endif
    }
}