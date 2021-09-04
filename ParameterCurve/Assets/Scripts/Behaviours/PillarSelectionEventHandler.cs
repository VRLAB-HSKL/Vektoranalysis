using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviours
{
    /// <summary>
    /// Class based on VIU developer pdf
    /// </summary>
    public class PillarSelectionEventHandler : AbstractVisualChangeSelectionEventHandler
    {
        public ThreeSelectionView threeSelView;
        //public GameObject boundariesParent;

        // public Material defaultMat;
        // public Material hoverMat;
        // public Material selectionMat;

        //private readonly HashSet<PointerEventData> _hovers = new HashSet<PointerEventData>();

        protected override void HandlePointerClick(PointerEventData eventData)
        {
            // if (eventData is VivePointerEventData viveEventData)
            // {
            //     if (viveEventData.viveButton != ControllerButton.Trigger) return;
            //
            //     var meshRenderers = boundariesParent.GetComponentsInChildren<MeshRenderer>();
            //     foreach (var m in meshRenderers)
            //     {
            //         m.material = selectionMat;
            //     }
            // }
            // else if (eventData != null)
            // {
            //     if (eventData.button != PointerEventData.InputButton.Left) return;
            //
            //     // Standalone button triggered!
            //     var meshRenderers = boundariesParent.GetComponentsInChildren<MeshRenderer>();
            //     foreach (var m in meshRenderers)
            //     {
            //         m.material = selectionMat;
            //     }
            // }
        }

        protected override void HandlePointerEnter(PointerEventData eventData)
        {
            // if (!_hovers.Add(eventData) || _hovers.Count != 1) return;
            //
            // var meshRenderers = boundariesParent.GetComponentsInChildren<MeshRenderer>();
            // foreach (var m in meshRenderers)
            // {
            //     m.material = hoverMat;
            // }
        }

        protected override void HandlePointerExit(PointerEventData eventData)
        {
            // if (!_hovers.Remove(eventData) || _hovers.Count != 0) return;
            //
            // var meshRenderers = boundariesParent.GetComponentsInChildren<MeshRenderer>();
            // foreach (var m in meshRenderers)
            // {                
            //     m.material = defaultMat;                
            // }
        }
    }
}
