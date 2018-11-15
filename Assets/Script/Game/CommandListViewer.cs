using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

//stateless
public class CommandListViewer : MonoBehaviour
{
    //---------------------------------------------------------
    //define 
    //---------------------------------------------------------
    private float rectSize = 64;
    private float padding = 10;
    private int listSize = 5;
    //private readonly string atlasPath = ""
    [SerializeField] private GridLayoutGroup _listRoot;
    [SerializeField] private SpriteAtlas _atlas;
    private List<Image> _blockList = new List<Image>();
    //---------------------------------------------------------
    //Requests
    //---------------------------------------------------------
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

    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    void Awake()
    {
        MakeList(listSize);
    }

    void MakeList(int num)
    {
        _listRoot.transform.DestroyAllChildren();

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
        }
    }
    void SetImage(int x, string path)
    {
        if (!_blockList.InRange(x))
        {
            return;
        }
        if (_atlas == null)
        {
            return;
        }
        var sprite = _atlas?.GetSprite(path);
        if (sprite == null)
        {
            return;
        }
        _blockList[x].sprite = sprite;
    }
}
