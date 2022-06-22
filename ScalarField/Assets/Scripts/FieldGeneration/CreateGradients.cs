using System;
using log4net;
using Model;
using Model.InitFile;
using UnityEngine;
using Utility;

namespace FieldGeneration
{
    public class CreateGradients : MonoBehaviour
    {
        public GameObject BoundingBox;
        public GameObject ArrowPrefab;

        public bool ShowGradients;
        public int stepsBetweenArrows;
        
        public void ToggleGradients()
        {
            ShowGradients = !ShowGradients;
            
            SetGradientsActive(ShowGradients);
        }

        private void SetGradientsActive(bool isActive)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isActive);
            }
        }
        
        
        private void Start()
        {
            var startIndex = 0;
            var lastIndexBefore = GlobalDataModel.CurrentField.Gradients.Count; 
            for(var i = startIndex; i < lastIndexBefore; i++)
            {
                if (i % stepsBetweenArrows != 0) continue;
                
                var gradient = GlobalDataModel.CurrentField.Gradients[i];
                gradient = new Vector3(gradient.x, gradient.z, gradient.y);
                var start = GlobalDataModel.CurrentField.MeshPoints[i];
                var end = start + gradient;

                // Debug.Log(
                //     "index: " + i + "\n" +
                //     "gradient: " + gradient + "\n" +
                //     "start: " + start + "\n" +
                //     "end: " + end + "\n" +
                //     "target: " + end
                // );

                DrawingUtility.DrawArrow(start, end, transform, ArrowPrefab, BoundingBox.transform.localScale);
            }
         
            SetGradientsActive(ShowGradients);
        }
    }
}