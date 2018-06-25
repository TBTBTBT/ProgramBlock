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
    [SerializeField] private RawImage _renderer;
    private int _index = 0;
    public void Init(int index,int id)//0:body,1:eye
    {

    }
    public void OnDrag(BaseEventData e)
    {
        Vector2 pos = ((PointerEventData)e).position;
        transform.position = pos;
    }
    public void OnPointerUp(BaseEventData e)
    {
        FishEditManager.Instance.ReplaceParts(_index,((PointerEventData)e).position);
    }
}
