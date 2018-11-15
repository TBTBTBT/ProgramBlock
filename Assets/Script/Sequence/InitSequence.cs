using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 
    InitSequence : MonoBehaviourWithStatemachine<InitSequence.State>
{
    public enum State
    {
        Init,
        Load,
        End,
    }

    protected void Awake()
    {
        base.Awake();
        //MasterdataManager.Instance.InitMasterdata();
    }
    IEnumerator Init()
    {
        yield return null;
        Next(State.Load);
    }

    IEnumerator Load()
    {
        yield return null;
        // yield return MasterdataManager.Instance.InitMasterdataAsync();
    }
}
