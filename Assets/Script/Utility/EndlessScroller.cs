using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Todo:TOAST採用検討
//stateless
public class EndlessScroller : UIBehaviour
{
    [System.Serializable]
    enum Direction
    {
        Horizontal,
        Vertical
    }
    [System.Serializable]
    public class OnUpdate : UnityEngine.Events.UnityEvent<int, GameObject> { }

    [SerializeField]
    private RectTransform _itemPrefab;

    [SerializeField]
    private Direction _direction;

    [SerializeField]
    private ScrollRect _scroller;
    [SerializeField]
    private RectTransform _scrollContents;
    [SerializeField]
    private Vector2 _cellSize;
    [SerializeField]
    private Vector2 _spacing;
    [SerializeField]
    private int _maxItemNum = 1;
    [SerializeField]
    private /*Vector2Int*/int _actualItemNum;
    private Vector4 _loopPos;//min max
    private List<RectTransform> _itemList = new List<RectTransform>();
    private int _lastUpdateItemIndex = 0;
    public OnUpdate OnItemUpdate = new OnUpdate();

    protected override void Awake(){
        base.Awake();

    }
    public void SetContents(int max){
        _maxItemNum = max;
        SetContentsSize();
        SetItem();
        _scroller.onValueChanged.AddListener(UpdateItemList);
    }
    //Contentsのサイズを調整
    void SetContentsSize()
    {
        switch(_direction){
            case Direction.Horizontal:
                SetContentsSizeHorizontal();

                break;
        }
    }
    void SetContentsSizeHorizontal(){
       // Vector2 size = _scroller.viewport.sizeDelta;
       // int maxSize = (int)(size.x / (_cellSize.x + _spacing.x));
        _scroller.content.sizeDelta = new Vector2((_cellSize.x + _spacing.x) * _maxItemNum, _scrollContents.sizeDelta.y);

    }
    void SetItem(){
        switch (_direction)
        {
            case Direction.Horizontal:
                SetItemHorizontal();

                break;
        }
    }
    void SetItemHorizontal(){
        for (int i = 0; i < _actualItemNum; i++)
        {
            var go = Instantiate(_itemPrefab.gameObject, _scrollContents);
            var rect = go.GetComponent<RectTransform>();
            if (rect == null)
            {
                Debug.Log("Error");
                return;
            }
            rect.sizeDelta = _cellSize;
            rect.anchoredPosition = new Vector2((_cellSize.x + _spacing.x) * i, 0);
            _itemList.Add(rect);
            if (i < _maxItemNum)
            {
                go.gameObject.SetActive(true);
                OnItemUpdate.Invoke(i, go);
            }else{
                go.gameObject.SetActive(false);
            }
        }
        _lastUpdateItemIndex = 0;
    }
    void UpdateItemList(Vector2 scroll){
        switch (_direction)
        {
            case Direction.Horizontal:
                UpdateItemListHorizontal(scroll);

                break;
        }
    }
    void UpdateItemListHorizontal(Vector2 scroll)
    {
        int start = Mathf.Max( (int)(scroll.x * (_maxItemNum - _actualItemNum)),0 );//その地点のスタートインデックス切り捨て
        //int diff = _lastUpdateItemIndex - start;
        //Debug.Log(start);
        for (int i = 0; i < _actualItemNum; i++)
        {
            int beforeNum = Mathf.CeilToInt((float)(_lastUpdateItemIndex - i) / _actualItemNum);
            int num = Mathf.CeilToInt((float)(start - i) / _actualItemNum);
            if (beforeNum != num)
            {
                ItemUpdateHorizontal(num * _actualItemNum + i, _itemList[i]);

            }

        }
        _lastUpdateItemIndex = start;

    }

    void ItemUpdateHorizontal(int next,RectTransform rect){
        Debug.Log(next);
        rect.anchoredPosition = new Vector2( (_cellSize.x + _spacing.x) * next, 0);
        if (next >= _maxItemNum){
            rect.gameObject.SetActive(false);
        }else{
            rect.gameObject.SetActive(true);
            OnItemUpdate.Invoke(next,rect.gameObject);
        }

    }
    private void LateUpdate()
    {
        //UpdateItemList();
    }
}
