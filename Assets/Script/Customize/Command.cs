using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.U2D;
//stateless
public class Command : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Button _button;
    [SerializeField] List<Image> _arrow;// 0 yes 1 no
    [SerializeField] Animator _anim;
    [SerializeField] SpriteAtlas _atlas;
    void Awake()
    {
        
        //dummy
    }
    public void Set(long id){

    }
    public void Set(ProgramFormat.OrderFormat order)
    {
        var table = MasterdataManager.Records<MstFunctionRecord>();//.FirstOrDefault(_ => _.functionkey == order.key);
        var record = table.FirstOrDefault(_ => _.functionkey == order.key);
        if (record == null)
        {
            Debug.Log("MstFunctionIsNull");
            return;
        }
        string path = record.imagepath;
        SetImage(path);
    }
    public void SetButtonCallback(UnityAction cb)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(cb);
    }
    void SetImage(string path){
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
    void SetArrow(){

    }
}
