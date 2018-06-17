using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager> {
    void Load()
    {

    }

    void Save()
    {

    }

    void GetNetworkData()
    {

    }
    public static DeckData LoadDeckData(int num)
    {
        DeckData ret = new DeckData();
        ret._towerList.Add(new TowerData(){_modList = new List<int>(){2,1,0},_initPos = new Vector3(0,0,0)});
        return ret;
    }
    public static DeckData LoadEnemyDeckData()
    {
        DeckData ret = new DeckData();
        ret._towerList.Add(new TowerData() { _modList = new List<int>() { 1, 1 }, _initPos = new Vector3(0, 0, 2) });
        return ret;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
public class DeckData
{
    public List<TowerData> _towerList = new List<TowerData>();
    
}

public class TowerData
{
    public List<int> _modList = new List<int>();
    public Vector3 _initPos = new Vector3(0,0,0);
}