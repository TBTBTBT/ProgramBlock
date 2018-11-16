using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndlessScroller : UIBehaviour
{
    [System.Serializable]
    enum Direction
    {
        Horizontal,
        Vertical
    }
    [SerializeField]
    private GameObject _itemPrefab;

    [SerializeField]
    private Direction _direction;

    [SerializeField]
    private RectTransform _scrollContents;
    [System.Serializable]
    public class OnUpdate : UnityEngine.Events.UnityEvent<int, GameObject> { }
    public OnUpdate OnItemUpdate = new OnUpdate();
    //Contentsのサイズを調整
    void SetContentsSize()
    {

    }
}
