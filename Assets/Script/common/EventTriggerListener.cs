//
// /********************************************************
// * 
// *　　　　　　Copyright (c) 2015  Feiyu
// *  
// * Author		: Binglei Gong</br>
// * Date		: 15-12-8下9:38</br>
// * Declare	: </br>
// * Version	: 1.0.0</br>
// * Summary	: create</br>
// *
// *
// *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
namespace common
{
    public class EventTriggerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler,
    IPointerExitHandler, IPointerUpHandler, ISelectHandler, IUpdateSelectedHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void VoidDelegate(GameObject go, PointerEventData eventData);
        public delegate void VoidBaseDelegate(GameObject go, BaseEventData eventData);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onDrag;
        public VoidBaseDelegate onSelect;
        public VoidBaseDelegate onUpdateSelect;
        public VoidDelegate onBeginDrag;


        static public EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null) listener = go.AddComponent<EventTriggerListener>();
            return listener;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null) onClick(gameObject, eventData);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject, eventData);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject, eventData);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject, eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject, eventData);
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject, eventData);
        }
        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject, eventData);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (onBeginDrag != null) onBeginDrag(gameObject, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData data)
        {
            if(onDrag != null) onDrag(gameObject, data);
        }
    }
}
