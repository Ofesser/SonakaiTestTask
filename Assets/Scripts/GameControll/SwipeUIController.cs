using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Обработка свайпа по экрану
public class SwipeUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler{

    [HideInInspector]
    public Vector2 touchPosition;
    [HideInInspector]
    public bool touchPressed;
    private int currentPointerId;
	

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!touchPressed)
        {
            touchPosition = eventData.position;
            touchPressed = true;
            currentPointerId = eventData.pointerId;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == currentPointerId)
        {
            touchPressed = false;
            touchPosition = Vector2.zero;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == currentPointerId)
        {
            touchPosition = eventData.position;
        }
    }

}
