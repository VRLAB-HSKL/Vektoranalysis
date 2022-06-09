using Calculation;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TextureToRenderer : MonoBehaviour
{
    public Texture2D texture;
    
    private void Start()
    {
        // If not explicitly set, try to get texture based on parsed init file values
        if (texture is null)
        {
            var cmId = GlobalDataModel.InitFile.color_map_id;
            var cmDataClassesCount = GlobalDataModel.InitFile.color_map_data_classes_count;
            texture = TextureUtility.FetchColorMapTexture(cmId, cmDataClassesCount);    
        }

        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    
}
