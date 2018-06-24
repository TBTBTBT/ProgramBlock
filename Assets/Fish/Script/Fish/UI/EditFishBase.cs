using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditFishBase : SingletonMonoBehaviour<EditFishBase> {
    
    [SerializeField] private List<Renderer> _partsRenderer;
    protected override void Awake()
	{
        base.Awake();
        Init();
	}
	void Init()
	{
        _partsRenderer.ForEach(renderer => { renderer.gameObject.SetActive(false); });
	}
    void InstantiateParts(int i, string path, Vector2 pos)
    {

        _partsRenderer[i].material = Resources.Load<Material>(path);
        _partsRenderer[i].material.SetVector("_RotateCenter", new Vector4(pos.x, pos.y, 0, 0));
        _partsRenderer[i].gameObject.SetActive(true);
    }
    public void AddParts(PartsType type,int id,Vector2 pos){
        switch(type){
            case PartsType.Body:
                InstantiateParts(0, FishMasterData.MaterialPath[PartsType.Body] + id, pos);
                break;
            case PartsType.Eye:
                InstantiateParts(1, FishMasterData.MaterialPath[PartsType.Eye] + id, pos);
                break;
            case PartsType.Fin:
                break;
        }
    }
    public void DeleteParts(int i){
        _partsRenderer[i].gameObject.SetActive(false);
    }
	public void DragParts(){
        
    }


}
