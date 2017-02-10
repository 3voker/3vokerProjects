using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace UnitySampleAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Grayscale")]
    public class GrayscaleEffect : ImageEffectBase
    {
        public Texture textureRamp;
        public float rampOffset;

        // Called by camera to apply image effect
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.SetTexture("_RampTex", textureRamp);
            material.SetFloat("_RampOffset", rampOffset);
            Graphics.Blit(source, destination, material);
        }
    }
}