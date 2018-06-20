using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] GameObject _fishBase;

    private List<DeckData> _decks = new List<DeckData>();
	// Use this for initialization
    void Init()
    {

    }
	void Start () {
        AddDeck(0);
	    AddDeck(0);
	    SpawnAll();
	}

    void SpawnAll()
    {
        for (int i = 0; i < _decks.Count; i++)
        {
            for (int j = 0; j < _decks[i]._fishId.Count; j++)
            {
                InstantiateFish(_decks[i]._fishId[j],i);
            }

        }
    }
    void AddDeck(int id, bool isInternet = false)
    {
        DeckData deck = new DeckData();//Masterからロード
        List<int> ids = new List<int>(){0};
        deck._fishId = ids;
        _decks.Add(deck);
    }
    void InstantiateFish(int id,int mem){
        FishData data = new FishData();//Masterからロード
        Instantiate(_fishBase,new Vector3(0,0,0),Quaternion.identity);
        _fishBase.GetComponent<FishBase>().InitData(data._parts);
        _fishBase.GetComponent<FishBase>().Team = mem;
    }

}
