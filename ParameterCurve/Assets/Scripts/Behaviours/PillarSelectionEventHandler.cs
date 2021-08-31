using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class based on VIU developer pdf
/// ToDo: Implement abstract base class and copy into VRKL
/// </summary>
public class PillarSelectionEventHandler : AbstractCanvasRaycastEventHandler
{
    public ThreeSelectionView threeSelView;
    public GameObject BoundariesParent;

    public Material DefaultMat;
    public Material HoverMat;
    public Material SelectionMat;

    private HashSet<PointerEventData> Hovers = new HashSet<PointerEventData>();

    protected override void HandlePointerClick(PointerEventData eventData)
    {
        if (eventData is VivePointerEventData viveEventData)
        {
            if (viveEventData.viveButton == ControllerButton.Trigger)
            {
                MeshRenderer[] meshRenderers = BoundariesParent.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer m in meshRenderers)
                {
                    m.material = SelectionMat;
                }
            }
        }
        else if (eventData != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Standalone button triggered!
                MeshRenderer[] meshRenderers = BoundariesParent.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer m in meshRenderers)
                {
                    m.material = SelectionMat;
                }
            }
        }
    }

    protected override void HandlePointerEnter(PointerEventData eventData)
    {
        if (Hovers.Add(eventData) && Hovers.Count == 1)
        {
            MeshRenderer[] meshRenderers = BoundariesParent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in meshRenderers)
            {
                m.material = HoverMat;
            }
        }
    }

    protected override void HandlePointerExit(PointerEventData eventData)
    {
        if (Hovers.Remove(eventData) && Hovers.Count == 0)
        {
            MeshRenderer[] meshRenderers = BoundariesParent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in meshRenderers)
            {                
                m.material = DefaultMat;                
            }
        }
    }
}
