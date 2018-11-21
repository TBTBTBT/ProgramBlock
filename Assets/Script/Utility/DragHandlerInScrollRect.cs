using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// スクローラーの子要素で、ドラッグハンドラを実装する
/// </summary>
public class DragHandlerInScrollRect : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler
{
    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
        //Debug.Log("Init");
        Execute<IInitializePotentialDragHandler>(transform.parent.gameObject, eventData,
            ExecuteEvents.initializePotentialDrag);
    }


    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("BEgin");
        var handler = (IBeginDragHandlerInScrollRect)gameObject.GetComponent(typeof(IBeginDragHandlerInScrollRect));
        handler?.OnBeginDrag(eventData);
        Execute<IBeginDragHandler>(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("End");
        var handler = (IEndDragHandlerInScrollRect)gameObject.GetComponent(typeof(IEndDragHandlerInScrollRect));
        handler?.OnEndDrag(eventData);
        Execute<IEndDragHandler>(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("Drag");
        var handler = (IDragHandlerInScrollRect)gameObject.GetComponent(typeof(IDragHandlerInScrollRect));
        handler?.OnDrag(eventData);
        Execute<IDragHandler>(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
    }


    void Execute<T>(GameObject go, PointerEventData e, ExecuteEvents.EventFunction<T> exec)
        where T : IEventSystemHandler
    {
        var handler = ExecuteEvents.GetEventHandler<IInitializePotentialDragHandler>(go);
        if (handler != null)
        {
            ExecuteEvents.ExecuteHierarchy(handler, e, exec);
        }
    }




}
public interface IEventSystemHandlerInScrollRect
{

}
public interface IBeginDragHandlerInScrollRect : IEventSystemHandlerInScrollRect
{
    void OnBeginDrag(PointerEventData eventData);
}

public interface IEndDragHandlerInScrollRect : IEventSystemHandlerInScrollRect
{
    void OnEndDrag(PointerEventData eventData);
}

public interface IDragHandlerInScrollRect : IEventSystemHandlerInScrollRect
{
    void OnDrag(PointerEventData eventData);

}
