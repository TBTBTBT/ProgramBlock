using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Events;
using UnityEngine.UI;

//stateless
public class CommandListViewer : MonoBehaviour
{
    //---------------------------------------------------------
    //define 
    //---------------------------------------------------------
    private float rectSize = 64;
    private float padding = 10;
    private int listSize = 8;
    //private readonly string atlasPath = ""
    [SerializeField] private EndlessScroller _listRoot;
    [SerializeField] private SpriteAtlas _atlas;
    //---------------------------------------------------------
    //Requests
    //---------------------------------------------------------

    public void SetupList(int max, UnityAction<int, GameObject> cb){
        MakeList(max,cb);
    }
    public void SetImage(Image image ,ProgramFormat.OrderFormat order)
    {
        var table = MasterdataManager.Records<MstFunctionRecord>();
        var record = table.FirstOrDefault(_ => _.functionkey == order.key);
       // Debug.Log(table[0].functionkey +" " +order.key);
        if (record == null)
        {
            return;
        }
        string path = record.imagepath;
        SetImage(image, path);
    }
    public void SetupButton(int num,Button button,Action<int> cb)
    {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => cb(num));

    }
    /*
    public void SetBlock(int x,ProgramFormat.OrderFormat order)
    {
        //Master
        var record = MasterdataManager.Records<MstFunctionRecord>().FirstOrDefault(_ => _.functionkey == order.key);
        if (record == null)
        {
            return;
        }
        string path = record.imagepath;
        SetImage(x,path);
    }

    public void SetupButton(Action<int> cb)
    {
        for (int i = 0; i < _blockList.Count; i++)
        {
            int x = i;
            var button = _blockList[i].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => cb(x));

        }
    }
*/
    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    void Awake()
    {
        //MakeList(listSize);
    }

    void MakeList(int num, UnityAction<int, GameObject> cb)
    {
       
        if(_listRoot == null){
            return;
        }
        //_listRoot.transform.DestroyAllChildren();
        //_listRoot.OnItemUpdate.AddListener(SetItem);
        _listRoot.OnItemUpdate.AddListener(cb);
        _listRoot.SetContents(num);
        /*
        for (int i = 0; i < num; i++)
        {

            var go = new GameObject() { name = $"c_{i}" };
            var rect = go.AddComponent<RectTransform>();
            var img = go.AddComponent<Image>();
            var button = go.AddComponent<Button>();

            go.transform.parent = _listRoot.transform;
            rect.sizeDelta = new Vector2(rectSize, rectSize);
            button.targetGraphic = img;
            _blockList.Add(img);
        }*/
    }
    void SetItem(int num, GameObject go)
    {

    }

    void SetImage(Image img, string path)
    {
        if (_atlas == null)
        {
            return;
        }
        var sprite = _atlas?.GetSprite(path);
        Debug.Log(sprite);
        if (sprite == null)
        {
            return;
        }
        img.sprite = sprite;
    }
}
