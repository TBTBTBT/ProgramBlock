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
            _partsFrame[i].UpdateData((byte)i,_nowEdit);
        }
    }
    public void ChangeNowEdit(PartsType edit){
        
        UpdateFrameData(edit);

    }
    PartsFrame InstantiateFrame(){

        PartsFrame result = Instantiate(_partsFramePrefab, _partsFrameRoot).GetComponent<PartsFrame>();
        return result;
    }
    void AddFrame(int max){
        while(max > _partsFrame.Count){
            _partsFrame.Add(InstantiateFrame());
        }
    }
    public void PlaceParts(Vector2 pos){
        switch(_nowEdit){
            case PartsType.Body:
                _data.AddParts(_nowEdit, 0, pos);
        break;
            case PartsType.Eye:
        break;
            case PartsType.Fin:
                break;
        }
    }
}
