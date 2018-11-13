using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourWithStatemachine<GameManager.State>
{
    private List<UnitCore> _units = new List<UnitCore>();

    public List<UnitCore> Units => _units;
    public enum State
    {
        Init,
        Wait,
        Running,
        Stop,
        Step
    }


    public void Run()
    {
        Next(State.Running);
    }
    IEnumerator Init()
    {
        _units.ForEach(_ => Destroy(_));
        _units.Clear();
        _units.Add(Instantiate(Resources.Load<GameObject>("Unit/Unit_00")).GetComponent<UnitCore>());

        yield return null;
    }

    IEnumerator Running()
    {
        foreach (var unit in _units)
        {
            unit.Setup(0,this,1,"0,0:v1:0:1,0;1,0:v10:0:0,0");
            unit.StartProcess();
        }

        yield return null;
    }
}
