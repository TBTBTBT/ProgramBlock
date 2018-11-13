using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

//Stateless
public class ProgramViewer : MonoBehaviour
{
    //define 
    private float rectSize = 64;
    private float padding = 10;
    private int programSize = 5;
    //private readonly string atlasPath = ""
    [SerializeField] private GridLayout _blockRoot;
    [SerializeField] private SpriteAtlas _atlas;
    private List<List<Image>> _blockList = new List<List<Image>>();

    //Requests
    public void SetBlock(int x, int y, ProgramFormat.OrderFormat order)
    {
        //Master
        string path = "";
        SetImage(x,y,path);
    } 
    
    //private
    void Awake()
    {
        MakeField(programSize, programSize);
    }

    void MakeField(int w, int h)
    {
        var list = new List<Image>();

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                var go = new GameObject() { name = $"{i}_{j}" };
                var rect = go.AddComponent<RectTransform>();
                var img = go.AddComponent<Image>();
                rect.sizeDelta = new Vector2(rectSize, rectSize);
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

        var sprite = _atlas.GetSprite(path);
        if (sprite == null)
        {
            return;
        }
        _blockList[x][y].sprite = sprite;
    }
}
