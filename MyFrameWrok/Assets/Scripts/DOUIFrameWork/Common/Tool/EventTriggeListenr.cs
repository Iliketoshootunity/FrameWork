using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace DOUIFrame
{
    public class TouchHandle
    {
        private event OnTouchEventHandle touchEvent;
        private object[] paramss;

        public TouchHandle()
        {

        }
        public TouchHandle(OnTouchEventHandle handle, params object[] paramss)
        {
            touchEvent += handle;
            this.paramss = paramss;
        }
        public void SetHandle(OnTouchEventHandle handle, params object[] paramss)
        {
            DestoryHandle();
            touchEvent += handle;
            this.paramss = paramss;
        }
        public void CallEventHandle(GameObject listener, object args)
        {
            if (null != touchEvent)
            {
                touchEvent(listener, args, paramss);
            }
        }
        public void DestoryHandle()
        {
            if (null != touchEvent)
            {
                touchEvent -= touchEvent;
                touchEvent = null;
            }
        }
    }


    public class EventTriggeListenr : MonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler,

        ISelectHandler,
        IUpdateSelectedHandler,
        IDeselectHandler,

        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IScrollHandler,
        IMoveHandler
    {

        public TouchHandle OnClickHandle;
        public TouchHandle OnDoubleClickHandle;
        public TouchHandle OnDownHandle;
        public TouchHandle OnEnterHandle;
        public TouchHandle OnExitHandle;
        public TouchHandle OnUpHandle;
        public TouchHandle OnSelectHandle;
        public TouchHandle OnUpdateSelectHandle;
        public TouchHandle OnDeSelectHandle;
        public TouchHandle OnDragHandle;
        public TouchHandle OnDragEndHandle;
        public TouchHandle OnDropHandle;
        public TouchHandle OnScrollHandle;
        public TouchHandle OnMoveHandle;

        public static EventTriggeListenr Get(GameObject go)
        {
            EventTriggeListenr listener = go.GetComponent<EventTriggeListenr>();
            if (null == listener)
            {
                listener = go.AddComponent<EventTriggeListenr>();
            }
            return listener;
        }
        private void OnDestroy()
        {
            removeAllHandle();
        }
        private void removeAllHandle()
        {
            removeHandle(OnClickHandle);
            removeHandle(OnDoubleClickHandle);
            removeHandle(OnDownHandle);
            removeHandle(OnEnterHandle);
            removeHandle(OnExitHandle);
            removeHandle(OnUpHandle);
            removeHandle(OnSelectHandle);
            removeHandle(OnUpdateSelectHandle);
            removeHandle(OnDeSelectHandle);
            removeHandle(OnDragHandle);
            removeHandle(OnDragEndHandle);
            removeHandle(OnDropHandle);
            removeHandle(OnScrollHandle);
            removeHandle(OnMoveHandle);
        }
        private void removeHandle(TouchHandle handle)
        {
            if(null!=handle)
            {
                handle.DestoryHandle();
                handle = null;
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (null != OnClickHandle)
            {
                OnClickHandle.CallEventHandle(this.gameObject, eventData);
            }
        }
        public void OnDeselect(BaseEventData eventData)
        {
            if (null != OnDeSelectHandle)
            {
                OnDeSelectHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (null != OnDragHandle)
            {
                OnDragHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (null != OnDropHandle)
            {
                OnDropHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (null != OnEnterHandle)
            {
                OnEnterHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnMove(AxisEventData eventData)
        {
            if (null != OnMoveHandle)
            {
                OnMoveHandle.CallEventHandle(this.gameObject, eventData);
            }
        }



        public void OnPointerDown(PointerEventData eventData)
        {
            if (null != OnDownHandle)
            {
                OnDownHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (null != OnEnterHandle)
            {
                OnEnterHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (null != OnExitHandle)
            {
                OnExitHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (null != OnUpHandle)
            {
                OnUpHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (null != OnScrollHandle)
            {
                OnScrollHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (null != OnSelectHandle)
            {
                OnSelectHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (null != OnUpdateSelectHandle)
            {
                OnUpdateSelectHandle.CallEventHandle(this.gameObject, eventData);
            }
        }

        public void SetEventHandle(EnumTouchEventType type, OnTouchEventHandle handle, params object[] paramss)
        {
            switch (type)
            {
                case EnumTouchEventType.OnClickHandle:
                    if (null == OnClickHandle)
                    {
                        OnClickHandle = new TouchHandle();
                    }
                    OnClickHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnDoubleClickHandle:
                    if (null == OnDoubleClickHandle)
                    {
                        OnDoubleClickHandle = new TouchHandle();
                    }
                    OnDoubleClickHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnDownHandle:
                    if (null == OnDownHandle)
                    {
                        OnDownHandle = new TouchHandle();
                    }
                    OnDownHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnUpHandle:
                    if (null == OnUpHandle)
                    {
                        OnUpHandle = new TouchHandle();
                    }
                    OnUpHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnEnterHandle:
                    if (null == OnEnterHandle)
                    {
                        OnEnterHandle = new TouchHandle();
                    }
                    OnEnterHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnExitHandle:
                    if (null == OnExitHandle)
                    {
                        OnExitHandle = new TouchHandle();
                    }
                    OnExitHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnDragHandle:
                    if (null == OnDragHandle)
                    {
                        OnDragHandle = new TouchHandle();
                    }
                    OnDragHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnDropHandle:
                    if (null == OnDropHandle)
                    {
                        OnDropHandle = new TouchHandle();
                    }
                    OnDropHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnDragEndHandle:
                    if (null == OnDragEndHandle)
                    {
                        OnDragEndHandle = new TouchHandle();
                    }
                    OnDragEndHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnSelectHandle:
                    if (null == OnSelectHandle)
                    {
                        OnSelectHandle = new TouchHandle();
                    }
                    OnSelectHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnUpdateSelectHandle:
                    if (null == OnUpdateSelectHandle)
                    {
                        OnUpdateSelectHandle = new TouchHandle();
                    }
                    OnUpdateSelectHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnDeSelectHandle:
                    if (null == OnDeSelectHandle)
                    {
                        OnDeSelectHandle = new TouchHandle();
                    }
                    OnDeSelectHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnScrollHandle:
                    if (null == OnScrollHandle)
                    {
                        OnScrollHandle = new TouchHandle();
                    }
                    OnScrollHandle.SetHandle(handle, paramss);
                    break;
                case EnumTouchEventType.OnMoveHandle:
                    if (null == OnMoveHandle)
                    {
                        OnMoveHandle = new TouchHandle();
                    }
                    OnMoveHandle.SetHandle(handle, paramss);
                    break;
            }

        }
    }

}
