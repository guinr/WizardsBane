using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Objects
{
    public class Torch : MonoBehaviour
    {
        public Light2D lightSource;

        private void Start()
        {
            InvokeRepeating(nameof(ChangeIntensity), 0.2f, 0.3f);
        
        }

        private void ChangeIntensity()
        {
            lightSource.pointLightOuterRadius = lightSource.pointLightOuterRadius > 2f ? 2f : 2.2f;
        }
    }
}
