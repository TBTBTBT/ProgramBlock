using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] GameObject _fishBase;
	// Use this for initialization
	void Start () {
        InstantiateFish(_fishBase);
	}
    void InstantiateFish(GameObject _prefab){
        Instantiate(_prefab,new Vector3(0,0,0),Quaternion.identity);
        
    }
	// Update is called once per frame
	void Update () {
		
	}
}
