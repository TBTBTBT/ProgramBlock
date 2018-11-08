using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourWithStatemachine<GameManager.State>
{
    private List<UnitCore> _units = new List<UnitCore>();

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
            unit.Setup("");
        }

        yield return null;
    }
}
