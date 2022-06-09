using UnityEngine;

namespace Calculation
{
    public static class TextureUtility
    {
        public static Texture2D FetchColorMapTexture(string colorMapKey, string colorMapDataClassesCount)
        {
            return Resources.Load("texture_maps/" +
                                         colorMapKey + "/" +
                                         colorMapDataClassesCount + "/" +
                                         colorMapKey + "_" + colorMapDataClassesCount + "_texture") as Texture2D;
        }
    }
}