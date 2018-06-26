using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEditManager : SingletonMonoBehaviour<FishEditManager>
{

    private FishData _data;
    [SerializeField] GameObject _partsFramePrefab;
    [SerializeField] Transform _partsFrameRoot;
    private List<PartsFrame> _partsFrame = new List<PartsFrame>();
    private Dictionary<string,byte> _partsMax = new Dictionary<string, byte>();
    private PartsType _nowEdit = PartsType.Body;
	// Use this for initialization
	void Start ()
	{
	    _data = new FishData();
        InitData();
        InitFrame();
	}

    void InitData(){
        _partsMax.Add(PartsType.Body.ToString(),(byte)FishMasterData.BodyDatas.Count);
        _partsMax.Add(PartsType.Eye.ToString(), (byte)FishMasterData.EyeDatas.Count);
        _partsMax.Add(PartsType.Fin.ToString(), (byte)FishMasterData.FinDatas.Count);

    }
    void InitFrame(){
        
        UpdateFrameData(PartsType.Body);


    }
    public void UpdateFrameData(PartsType edit){
        _nowEdit = edit;
        AddFrame(_partsMax[_nowEdit.ToString()]);
        for (int i = 0; i < _partsFrame.Count;i++){
            Debug.Log(i);
            _partsFrame[i].UpdateData(i,_nowEdit);
        }
    }
    public void ChangeNowEdit(int edit){
        
        UpdateFrameData((PartsType)edit);

    }
    
    PartsFrame InstantiateFrame(int id){

        PartsFrame result = Instantiate(_partsFramePrefab, _partsFrameRoot).GetComponent<PartsFrame>();
        return result;
    }
    void AddFrame(int max)
    {
        int id = 0;
        while(max > _partsFrame.Count){
            _partsFrame.Add(InstantiateFrame(id));

        }
    }
    public void PlaceParts(int id,Vector2 pos)
    {
        
        switch(_nowEdit){
            case PartsType.Body:
                Debug.Log("Place Body" + id);
                _data.AddParts(_nowEdit, id, new Vector2(0, 0));
                EditFishBase.Instance.AddParts(_nowEdit,id,new Vector2(0,0));
        break;
            case PartsType.Eye:
                Debug.Log("Place Eye");
                _data.AddParts(_nowEdit, id, pos);
                EditFishBase.Instance.AddParts(_nowEdit, id, pos);
                break;
            case PartsType.Fin:
                break;
        }
    }

    public void ReplaceParts(int index, Vector2 pos)
    {
        Vector2 relPos = EditFishBase.Instance.RelativePos(MainCameraSingleton.Instance.ScreenToWorld(pos));
        Debug.Log("Replace p" + index);
        _data.ChangePartsPos(index, relPos);
        EditFishBase.Instance.ChangePartsPos(index, relPos);
    }
}
