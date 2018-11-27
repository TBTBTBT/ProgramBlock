using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.U2D;
//stateless
public class Command : MonoBehaviour,IDragHandlerInScrollRect,IPointerDownHandler,IPointerClickHandler, IPointerUpHandler
{
    [SerializeField] Image _base;
    [SerializeField] Image _icon;
   // [SerializeField] Button _button;
    [SerializeField] List<GameObject> _arrow;// 0 yes 1 no
    [SerializeField] Animator _anim;
    [SerializeField] SpriteAtlas _atlas;
    public enum CallbackType {
        Click,
        PointerUp,
        Drag,
        PointerDown

    }
    private Dictionary<CallbackType, UnityAction<Vector2>> Callback = new Dictionary<CallbackType, UnityAction<Vector2>>();


    public void OnDrag(PointerEventData pe)
    {
        CallbackInvoke(CallbackType.Drag, pe.position);
        Debug.Log("Drag");

    }
    public void OnPointerDown(PointerEventData pe)
    {
        CallbackInvoke(CallbackType.PointerDown, pe.position);
        Debug.Log("Pointer");

    }
    public void OnPointerUp(PointerEventData pe)
    {
        CallbackInvoke(CallbackType.PointerUp, pe.position);
        Debug.Log("Release");

    }
    public void OnPointerClick(PointerEventData pe)
    {
        CallbackInvoke(CallbackType.Click, pe.position);
        Debug.Log("Click");

    }
    void CallbackInvoke(CallbackType type,Vector2 pos){
        if(Callback.ContainsKey(type)){
            Callback[type]?.Invoke(pos);
        }
    }
    void Awake()
    {
        
        //dummy
    }

    public void Set(long id){

    }
    public void Set(ProgramFormat.OrderFormat order)
    {
        if(order == null){
            SetActive(false);
            return;
        }
        SetActive(true);
        var table = MasterdataManager.Records<MstFunctionRecord>();//.FirstOrDefault(_ => _.functionkey == order.key);
        var record = table.FirstOrDefault(_ => _.functionkey == order.key);
        if (record == null)
        {
            SetActive(false);
            Debug.Log("MstFunctionIsNull");
            return;
        }
        string path = record.imagepath;
        SetImage(path);
    }
    void SetActive(bool flag){
        _icon.gameObject.SetActive(flag);
        _base.gameObject.SetActive(flag);
        _arrow[0].gameObject.SetActive(flag);
        _arrow[1].gameObject.SetActive(flag);
    }
    /*
    public void SetButtonCallback(UnityAction cb)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(cb);
    }*/

    public void SetArrow(bool yes,Vector2Int dist){
        int index = yes ? 0 : 1;
        if(_arrow.InRange(index)){
            if (dist.sqrMagnitude == 0)
            {
                _arrow[index].SetActive(false);
                return;
            }
            _arrow[index].SetActive(true);
            _arrow[index].transform.localRotation = Quaternion.AngleAxis(MathUtil.PointToAngle(Vector2.zero,dist), new Vector3(0, 0, 1));
        }
    }
    public void SetCallback(CallbackType type, UnityAction<Vector2> cb){
        if (Callback.ContainsKey(type))
        {
            Callback[type] = cb;
        }
        else
        {
            Callback.Add(type, cb);
        }

    }
    public void SetArrow(ProgramFormat.OrderFormat order, bool yes, bool loop)
    {
        //order.next
    }
    void SetImage(string path)
    {
        if (_atlas == null)
        {
            return;
        }
        //Debug.Log("SetBlock");
        var sprite = _atlas?.GetSprite(path);
        if (sprite == null)
        {
            return;
        }
        _icon.sprite = sprite;
    }
}
