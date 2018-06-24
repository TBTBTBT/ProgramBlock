using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class PartsFrame : MonoBehaviour {
    public byte Id { get; set; }
    [SerializeField] RawImage _renderer;
    [SerializeField] EventTrigger _eventTrigger;
	// Use this for initialization
	void Start () {
        Init();
	}
    public void AddListener(UnityAction<BaseEventData> cb){
        EventTrigger.Entry entry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Drag
        };
        entry.callback.AddListener(cb);
        _eventTrigger.triggers.Add(entry);
    }
	// Update is called once per frame
	void Update () {
		
	}
	void Init()
	{
        Id = 0;
	}
    public void OnPointerDown(BaseEventData e){
        
    }
    public void OnDrag(BaseEventData e)
	{
        Vector2 pos = ((PointerEventData)e).position;
        _renderer.transform.position = pos;
	}
    public void OnPointerUp(BaseEventData e)
    {
        FishEditManager.Instance.PlaceParts(((PointerEventData)e).position);
        _renderer.transform.localPosition = Vector2.zero;
    }
	public void UpdateData(byte id,PartsType type){
        Id = Id;
        string path = FishMasterData.MaterialPath[type] + id;
        Debug.Log(path);
        _renderer.material = Resources.Load<Material>(path);
    }
}
