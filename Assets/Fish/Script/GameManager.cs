using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] GameObject _fishBase;

    private List<DeckData> _decks = new List<DeckData>();
    private Vector2 _fieldSize = new Vector2(14, 8);
    public Vector2 FieldSize
    {
        get { return _fieldSize; }
    }
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
            for (int j = 0; j < _decks[i]._fish.Count; j++)
            {
                InstantiateFish(_decks[i]._fish[j].Id, _decks[i]._fish[j].Pos, i);
            }

        }
    }
    void AddDeck(int id, bool isInternet = false)
    {
        DeckData deck = new DeckData();//Masterからロード
        List<DeckData.IdPos> ids = new List<DeckData.IdPos>()
        {
            new DeckData.IdPos(){Id = 0,Pos = new Vector2(Random.Range(-14f,14f),Random.Range(-8f,8f))}
        };
        deck._fish = ids;
        _decks.Add(deck);
    }
    void InstantiateFish(int id,Vector2 pos,int mem){
        FishData data = new FishData();//Masterからロード
        data = LocalDataManager.LoadFishData(0);
        GameObject f = Instantiate(_fishBase,pos,Quaternion.identity);
        f.GetComponent<FishBase>().InitData(data);
        f.GetComponent<FishBase>().Team = mem;
    }

}
