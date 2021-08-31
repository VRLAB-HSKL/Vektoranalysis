using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AbstractCanvasRaycastEventHandler : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        HandlePointerClick(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HandlePointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandlePointerExit(eventData);
    }

    protected abstract void HandlePointerClick(PointerEventData eventData);
    protected abstract void HandlePointerEnter(PointerEventData eventData);
    protected abstract void HandlePointerExit(PointerEventData eventData);
}
