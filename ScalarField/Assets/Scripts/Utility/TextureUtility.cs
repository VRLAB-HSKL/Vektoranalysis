using UnityEngine;

namespace Calculation
{
    public static class TextureUtility
    {
        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,
            Transparent
        }
        

        /// <summary>
        /// Source: https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="blendMode"></param>
        /// <returns></returns>
        public static Material ChangeMaterialRenderMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                     material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                     material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                     material.SetInt("_ZWrite", 1);
                     material.DisableKeyword("_ALPHATEST_ON");
                     material.DisableKeyword("_ALPHABLEND_ON");
                     material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                     material.renderQueue = -1;
                     break;
                 case BlendMode.Cutout:
                     material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                     material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                     material.SetInt("_ZWrite", 1);
                     material.EnableKeyword("_ALPHATEST_ON");
                     material.DisableKeyword("_ALPHABLEND_ON");
                     material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                     material.renderQueue = 2449;//2450;
                     break;
                 case BlendMode.Fade:
                     material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                     material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                     material.SetInt("_ZWrite", 0);
                     material.DisableKeyword("_ALPHATEST_ON");
                     material.EnableKeyword("_ALPHABLEND_ON");
                     material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                     material.renderQueue = 2449; // 3000;
                     break;
                 case BlendMode.Transparent:
                     material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                     material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                     material.SetInt("_ZWrite", 0);
                     material.DisableKeyword("_ALPHATEST_ON");
                     material.DisableKeyword("_ALPHABLEND_ON");
                     material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                     material.renderQueue = 2449; //3000;
                     break;
             }

            return material;
        }
        
        public static Texture2D FetchColorMapTexture(string colorMapKey, string colorMapDataClassesCount)
        {
            return Resources.Load("texture_maps/" +
                                         colorMapKey + "/" +
                                         colorMapDataClassesCount + "/" +
                                         colorMapKey + "_" + colorMapDataClassesCount + "_texture") as Texture2D;
        }
    }
}