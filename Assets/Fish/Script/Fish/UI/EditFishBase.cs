﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 編集する魚のRootに配置すること
/// </summary>
public class EditFishBase : SingletonMonoBehaviour<EditFishBase> {
    
    [SerializeField] private List<Renderer> _partsRenderer;

    [SerializeField] private List<PartsHandle> _partsHandles;

    private bool DebugRender = true;

    int _finCount = 0;
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
    public void RenderParts(int i, string path, Vector2 pos)
    {
            _partsRenderer[i].material = Resources.Load<Material>(path);
            _partsRenderer[i].material.SetVector("_RotateCenter", new Vector4(pos.x, pos.y, 0, 0));
            _partsRenderer[i].gameObject.SetActive(true);
    }
    /// <summary>
    /// レンダリングするパーツを増やす　フィンは４つまで
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="pos">スクリーンポジション</param>
    public void AddParts(PartsType type,int id,Vector2 pos){
        Vector2 wpos = MainCameraSingleton.Instance.ScreenToWorld(pos);
        switch (type){
            case PartsType.Body:
                RenderParts(0, FishMasterData.MaterialPath[PartsType.Body] + id, new Vector2(0,0));

                break;
            case PartsType.Eye:
                RenderParts(1, FishMasterData.MaterialPath[PartsType.Eye] + id, RelativePos(wpos));
                ActiveHandle(1,id, FishMasterData.MaterialPath[PartsType.Eye] + id, pos);
                break;
            case PartsType.Fin:
                if (_finCount < 4)
                {
                    RenderParts(2 + _finCount, FishMasterData.MaterialPath[PartsType.Fin] + id, RelativePos(wpos));
                    ActiveHandle(2 + _finCount, id, FishMasterData.MaterialPath[PartsType.Fin] + id, pos);
                    _finCount++;
                }
                break;
        }
    }
    /// <summary>
    /// パーツ操作ハンドルを表示
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="path">Path.</param>
    /// <param name="pos">Position.</param>
    public void ActiveHandle(int index,int id,string path,Vector2 pos)
    {
        _partsHandles[index].gameObject.SetActive(true);
        _partsHandles[index].Init(index,id,path);
        _partsHandles[index].transform.position = pos;
    }
    public void ChangePartsPos(int index, Vector2 pos)
    {
        if (DebugRender)
        {
            _partsRenderer[index].material.SetVector("_RotateCenter", new Vector4(pos.x, pos.y, 0, 0));
            _partsRenderer[index].gameObject.SetActive(true);
        }
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
