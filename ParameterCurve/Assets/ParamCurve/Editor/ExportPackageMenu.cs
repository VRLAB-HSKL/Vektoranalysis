using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ExportPackageMenu : MonoBehaviour
    {
        [MenuItem("Custom Export/Export ParamCurve package")]
        public static void ExportParamCurvePackage()
        {
            var exportList = new List<string>();
        
            exportList.Add("Assets/ParamCurve/Scripts/Views/TubeMesh.cs");

            //const string targetFolder = "Assets/Export";
            
            // foreach (var path in exportList)
            // {
            //     var targetPath = targetFolder + "/" + path.Split('/').Last();
            //     if (!AssetDatabase.CopyAsset(path, targetPath))
            //     {
            //         Debug.LogError("Failed to copy " + path + " to " + targetPath);
            //     }
            // }
            
            AssetDatabase.ExportPackage(exportList.ToArray(), "ParamCurveAssets.unitypackage",
                 ExportPackageOptions.Interactive | ExportPackageOptions.IncludeDependencies);

            // foreach (var asset in AssetDatabase.FindAssets("", new [] {targetFolder} ))
            // {
            //     var path = AssetDatabase.GUIDToAssetPath(asset);
            //     Debug.Log("Path: " + path);
            //     AssetDatabase.DeleteAsset(path);
            // }
        }
    }
}
