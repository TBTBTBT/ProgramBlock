using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 編集する魚のRootに配置すること
/// </summary>
public class EditFishBase : SingletonMonoBehaviour<EditFishBase> {
    
    [SerializeField] private List<Renderer> _partsRenderer;

    [SerializeField] private List<PartsHandle> _partsHandles;
//    private readonly _partsTextureSize;
    protected override void Awake()
	{
        base.Awake();
        Init();
	}
	void Init()
	{
        _partsRenderer.ForEach(renderer => { renderer.gameObject.SetActive(false); });
	}
    public void AddParts(int i, string path, Vector2 pos)
    {

        _partsRenderer[i].material = Resources.Load<Material>(path);
        _partsRenderer[i].material.SetVector("_RotateCenter", new Vector4(pos.x, pos.y, 0, 0));
        _partsRenderer[i].gameObject.SetActive(true);
    }
    public void AddParts(PartsType type,int id,Vector2 pos){
        switch(type){
            case PartsType.Body:
                AddParts(0, FishMasterData.MaterialPath[PartsType.Body] + id, RelativePos(pos));
                ActiveHandle(0,id,pos);
                break;
            case PartsType.Eye:
                AddParts(1, FishMasterData.MaterialPath[PartsType.Eye] + id, RelativePos(pos));
                ActiveHandle(1,id,pos);
                break;
            case PartsType.Fin:
                break;
        }
    }

    public void ActiveHandle(int index,int id,Vector2 pos)
    {
        _partsHandles[index].gameObject.SetActive(true);
        _partsHandles[index].Init(index,id);
        _partsHandles[index].transform.position = pos;
    }
    public void ChangePartsPos(int index, Vector2 pos)
    {
        _partsRenderer[index].material.SetVector("_RotateCenter", new Vector4(pos.x, pos.y, 0, 0));
        _partsRenderer[index].gameObject.SetActive(true);
    }
    public void DeleteParts(int i){
        _partsRenderer[i].gameObject.SetActive(false);
    }

    private float _unitWorldSize = 2;
    private float _scale = 1.5f;
    public Vector2 RelativePos(Vector2 worldPos)
    {
        Vector2 r = worldPos - (Vector2)transform.position;
        //Debug.Log(r);
        r = (r / MainCameraSingleton.Instance.WorldMax().x) * (_unitWorldSize * _scale);
        return r;
    }

}
