using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

//Stateless
//(原則、存在すればいつでもアクセスでき、いつでも目的の結果を取得できる。)
public class ProgramViewer : MonoBehaviour
{
    //---------------------------------------------------------
    //define 
    //---------------------------------------------------------
    private float rectSize = 64;
    private float padding = 10;
    private int programSize = 5;
    //private readonly string atlasPath = ""
    [SerializeField] private GridLayoutGroup _blockRoot;
    [SerializeField] private SpriteAtlas _atlas;
    private List<List<Image>> _blockList = new List<List<Image>>();

    //---------------------------------------------------------
    //Requests
    //---------------------------------------------------------
    public void SetBlock(int x, int y, ProgramFormat.OrderFormat order)
    {
        var table = MasterdataManager.Records<MstFunctionRecord>();//.FirstOrDefault(_ => _.functionkey == order.key);
        var record = table.FirstOrDefault(_ => _.functionkey == order.key);
        if (record == null)
        {
            return;
        }
        string path = record.imagepath;
        SetImage(x,y, path);
    }

    public void SetupButton(Action<int,int> cb)
    {
        for (int i = 0; i < _blockList.Count; i++)
        {
            for (int j = 0; j < _blockList[i].Count; j++)
            {
                int x = i;
                int y = j;
                var button = _blockList[i][j].GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(()=>cb(x,y));
            }
        }
    }

    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    void Awake()
    {
        MakeField(programSize, programSize);
    }

    void MakeField(int w, int h)
    {
        _blockRoot.transform.DestroyAllChildren();
        var list = new List<Image>();

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                var go = new GameObject() { name = $"{i}_{j}" };
                var rect = go.AddComponent<RectTransform>();
                var img = go.AddComponent<Image>();
                var button = go.AddComponent<Button>();

                go.transform.parent = _blockRoot.transform;
                rect.sizeDelta = new Vector2(rectSize, rectSize);
                button.targetGraphic = img;
                list.Add(img);
            }
            _blockList.Add(list);
        }
    }
    void SetImage(int x, int y, string path)
    {
        if (!_blockList.InRange(x))
        {
            return;
        }
        if (!_blockList[x].InRange(y))
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
        _blockList[x][y].sprite = sprite;
    }
}
