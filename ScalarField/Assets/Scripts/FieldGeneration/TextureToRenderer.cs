using Calculation;
using Model;
using UnityEngine;

/// <summary>
/// Sets the texture on the MeshRenderer of the game object this script is attached to
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class TextureToRenderer : MonoBehaviour
{
    public ScalarFieldManager ScalarFieldManager;
    
    public Texture2D texture;
    
    private void Start()
    {
        var cmId = ScalarFieldManager.CurrentField.ColorMapId;
        var cmDataClassesCount = ScalarFieldManager.CurrentField.ColorMapDataClassesCount;
        texture = TextureUtility.FetchColorMapTexture(cmId, cmDataClassesCount);    
        // Debug.Log("cmId: " + cmId + ", cmDcCount: " + cmDataClassesCount + ", textureIsNull: " + (texture is null));
        //
        // // If not explicitly set, try to get texture based on parsed init file values
        // if (texture is null)
        // {
        //     
        // }
        // else
        // {
        //     Debug.Log("textureIsNull: " + (texture is null));
        // }

        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    
}
