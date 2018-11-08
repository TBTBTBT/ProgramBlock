using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourWithStatemachine<GameManager.State>
{
    private List<UnitCore> _units;

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
