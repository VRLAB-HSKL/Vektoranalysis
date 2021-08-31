using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class based on VIU developer pdf
/// ToDo: Implement abstract base class and copy into VRKL
/// </summary>
public class PillarSelectionEventHandler : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public GameObject Root;

    public Material DefaultMat;
    public Material HoverMat;
    public Material SelectionMat;

    private HashSet<PointerEventData> hovers = new HashSet<PointerEventData>();

    public void OnPointerClick(PointerEventData eventData)
    {
        var viveEventData = eventData as VivePointerEventData;
        if(viveEventData != null)
        {
            if(viveEventData.viveButton == ControllerButton.Trigger)
            {
                MeshRenderer[] meshRenderers = Root.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer m in meshRenderers)
                {
                    //if(m.material.color == DefaultMat.color)
                    //{
                    m.material = SelectionMat;
                    //}
                }
            }
        }
        else if(eventData != null)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                // Standalone button triggered!
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hovers.Add(eventData) && hovers.Count == 1)
        {
            MeshRenderer[] meshRenderers = Root.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer m in meshRenderers)
            {
                //if(m.material.color == DefaultMat.color)
                //{
                    m.material = HoverMat;
                //}
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hovers.Remove(eventData) && hovers.Count == 0)
        {
            MeshRenderer[] meshRenderers = Root.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in meshRenderers)
            {
                //if (m.material.color == HoverMat.color)
                //{
                    m.material = DefaultMat;
                //}
            }
        }
    }

}
