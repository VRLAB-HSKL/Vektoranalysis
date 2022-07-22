using UnityEngine;

namespace Model.ScriptableObjects
{
    /// <summary>
    /// Data container for file paths used in the application
    /// </summary>
    [CreateAssetMenu(fileName = "Path", menuName = "ScriptableObjects/PathManager", order = 1)]
    public class PathManager : ScriptableObject
    {
        /// <summary>
        /// Path to the init file resource
        /// </summary>
        public const string InitFileResourcePath = "json/init/sf_initFile";

        public const string FormulaImageResourcePath = "img/latex/";    
    }
}