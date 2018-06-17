using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    public enum BattleState
    {
        Start,
        Battle,
        End,
        Result
    }
    public DeckData _myDeck;

    public DeckData _enemyDeck;

    public GameObject _towerPrefab;

    private List<TowerObject> _allTower = new List<TowerObject>();

    private int _battleTime = 0;

    private BattleState _battleState = BattleState.Start;
    public UnityEvent OnExecute; 
    IEnumerator Battle()
    {
        while (_battleState == BattleState.Battle)
        {
            OnExecute.Invoke();
            yield return new WaitForSeconds(1f);
        }
        
    }
    void Start()
    {
        Init();
        _battleState = BattleState.Battle;
        EnterBattle();
    }

    void Update()
    {
     
    }

    void EnterBattle()
    {
        StartCoroutine(Battle());
    }
    void Init()
    {
        _myDeck = GetMyDeckData();
        _enemyDeck = GetEnemyDeckData();
        _allTower.AddRange(SpawnTower(_myDeck, 0));
        _allTower.AddRange(SpawnTower(_enemyDeck,1));
    }

    #region Init

    TowerObject[] SpawnTower(DeckData d,int mem)
    {
        List<TowerObject> ret = new List<TowerObject>();
        d._towerList.ForEach(t =>
        {
            TowerObject o = Instantiate(_towerPrefab, t._initPos, Quaternion.identity).GetComponent<TowerObject>();
            o.SetId(ret.Count,mem);
            o.InitModules(t._modList);
            ret.Add(o);
        });
        return ret.ToArray();
    }
    DeckData GetMyDeckData()
    {
        return DataManager.LoadDeckData(0);
    }

    DeckData GetEnemyDeckData()
    {
        return DataManager.LoadEnemyDeckData();
    }
    #endregion

    public TowerObject Find(int id)
    { 

        TowerObject tower = _allTower.Where(t => t.Id() == id).ToList()[0];
        return tower;
    }

    public TowerObject FindNearBy(int myId,int myMen)
    {
        TowerObject my = Find(myId);
        List<TowerObject> res = _allTower.Where(t => t.Member() != myMen).OrderBy(t =>
            {
                return (t.transform.position - my.transform.position).magnitude;
            }).ToList();
        if (res.Count > 0)
        {
            return res[0];
        }

        return null;
    }
}

