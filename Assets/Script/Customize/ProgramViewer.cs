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
    // int programSize = 5;
    //private readonly string atlasPath = ""
    [SerializeField] private GridLayoutGroup _commandRoot;
    [SerializeField] private SpriteAtlas _atlas;
    [SerializeField] private Command _commandPrefab;
    private List<List<Command>> _commandList = new List<List<Command>>();

    //---------------------------------------------------------
    //Requests
    //---------------------------------------------------------
    public void SetCommandImage(int x, int y, ProgramFormat.OrderFormat order)

    {
        if (!_commandList.InRange(x))
        {
            return;
        }
        if (!_commandList[x].InRange(y))
        {
            return;
        }
        _commandList[x][y].Set(order);

    }


    public void SetCommandCallback(int x,int y,Command.CallbackType type ,UnityAction<Vector2> cb)
    {
        _commandList[x][y].SetCallback(type, cb);
       
    }

    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    void Awake()
    {
        MakeField(ProgramFormat.width, ProgramFormat.height);
    }

    void MakeField(int w, int h)
    {
        _commandRoot.transform.DestroyAllChildren();


        for (int i = 0; i < w; i++)
        {
            var list = new List<Command>();
            for (int j = 0; j < h; j++)
            {
                var command = Instantiate(_commandPrefab, _commandRoot.transform).GetComponent<Command>();
                command.Set(0);
                list.Add(command);
  
            }
            _commandList.Add(list);
        }
    }
}

/*
Debug.Log("SetButtons");
for (int i = 0; i < _commandList.Count; i++)
{
for (int j = 0; j < _commandList[i].Count; j++)
{

int x = i;
int y = j;
SetCallback(Command.CallbackType.PointerDown,pos =>
{
cb(x, y);
});
}
}*/
/*
var go = new GameObject() { name = $"{i}_{j}" };
var rect = go.AddComponent<RectTransform>();
var img = go.AddComponent<Image>();
var button = go.AddComponent<Button>();

go.transform.parent = _blockRoot.transform;
rect.sizeDelta = new Vector2(rectSize, rectSize);
button.targetGraphic = img;
list.Add(img);*/
/*
var table = MasterdataManager.Records<MstFunctionRecord>();//.FirstOrDefault(_ => _.functionkey == order.key);
var record = table.FirstOrDefault(_ => _.functionkey == order.key);
if (record == null)
{
Debug.Log("MstFunctionIsNull");
return;
}
string path = record.imagepath;*/

//SetImage(x,y, path);
/*
public void SetCommandCallbackAll(Command.CallbackType type, UnityAction<int,int,Vector2> cb){
    for (int i = 0; i < _commandList.Count; i++)
    {
        for (int j = 0; j < _commandList[i].Count; j++)
        {

            int x = i;
            int y = j;
            _commandList[i][j].SetCallback(Command.CallbackType.PointerDown, pos =>
            {
                cb(x, y,pos);
            });
        }
    }
}*/
/*
void SetImage(int x, int y, string path)
{
    //Debug.Log("SetBlock");
    if (!_commandList.InRange(x))
    {
        return;
    }
    //Debug.Log(_blockList.Count);
    if (!_commandList[x].InRange(y))
    {
        return;
    }
    //Debug.Log("SetBlock");
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
    _blockList[x][y].sprite = sprite;
}*/
