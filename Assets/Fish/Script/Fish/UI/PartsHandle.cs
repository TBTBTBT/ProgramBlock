using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 配置したパーツを動かす
/// </summary>
public class PartsHandle : MonoBehaviour
{
    //[SerializeField] private RawImage _renderer;
    private int _index = 0;
    public void Init(int index)//0:body,1:eye
    {
        //_renderer.material = Resources.Load<Material>(path);
        _index = index;
    }
    public void UpdateInfo(Vector2 pos){
        transform.position = pos;
    }
    public void OnDrag(BaseEventData e)
    {
        Vector2 pos = ((PointerEventData)e).position;
        transform.position = pos;
       EditFishBase.Instance.ChangePartsPos(_index, EditFishBase.Instance.RelativePos(MainCameraSingleton.Instance.ScreenToWorld(pos)));
    }
    public void OnPointerUp(BaseEventData e)
    {
        Vector2 pos = ((PointerEventData)e).position;
        Vector2 relPos = EditFishBase.Instance.RelativePos(MainCameraSingleton.Instance.ScreenToWorld(pos));
        Debug.Log(Mathf.Abs(relPos.x));
        //範囲からはみ出したら消す
        if (Mathf.Abs(relPos.x) > FishEditManager.Instance.MaxRect.x
           || Mathf.Abs(relPos.y) > FishEditManager.Instance.MaxRect.y)
        {
            FishEditManager.Instance.RemoveParts(_index);
        }
        else
        {
            FishEditManager.Instance.ReplaceParts(_index, ((PointerEventData)e).position);
        }
    }
}
