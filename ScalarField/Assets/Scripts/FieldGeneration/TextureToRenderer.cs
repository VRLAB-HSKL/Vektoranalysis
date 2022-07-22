using Calculation;
using Model.ScriptableObjects;
using UnityEngine;

namespace FieldGeneration
{
    /// <summary>
    /// Sets the texture on the MeshRenderer of the game object this script is attached to
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class TextureToRenderer : MonoBehaviour
    {
        [Header("Data")]
        public ScalarFieldManager scalarFieldManager;
    
        private void Start()
        {
            var cmId = scalarFieldManager.CurrentField.ColorMapId;
            var cmDataClassesCount = scalarFieldManager.CurrentField.ColorMapDataClassesCount;
            var texture = TextureUtility.FetchColorMapTexture(cmId, cmDataClassesCount);    
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }
    }
}
