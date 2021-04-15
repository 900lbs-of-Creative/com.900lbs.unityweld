using UnityEngine;

using UnityWeld.Binding;

namespace UnityWeld.Examples.EventBinding
{
    [Binding]
    public class ExampleViewModel : MonoBehaviour
    {
        private float cubeRotation = 0f;

        [Binding]
        public void RotateCube()
        {
            cubeRotation = cubeRotation + 10f;
        }

        void Update()
        {
            var cube = GameObject.Find("Cube");
            cube.transform.localEulerAngles = new Vector3(0f, cubeRotation, 0f);
        }
    }
}
