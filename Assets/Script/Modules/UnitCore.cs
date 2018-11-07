using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCore : MonoBehaviourWithStatemachine<UnitCore.State> {

    //---------------------------------------------------------
    //states
    //---------------------------------------------------------
    public enum State
    {
        Init,
        Wait,
        Process,
        Do
    }
    //---------------------------------------------------------
    //class
    //---------------------------------------------------------
    /*
    class Param
    {
        
    }*/
    //---------------------------------------------------------
    //fields
    //---------------------------------------------------------
    //---------------------------------------------------------
    //properties
    //---------------------------------------------------------
    public int Hp { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Spd { get; set; }

    //phisics

    public Vector2 Velocity { get; set; }
    public float friction { get; set; }
    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------

    IEnumerator Init()
    {
        yield return null;
        //params初期化
        Hp = 100;
        Atk = 1;
        Def = 1;
        Spd = 1;
        Velocity = new Vector2(0,0);
        //MasterData参照
        //MasterdataManager.Get<MstUnitRecord>();

    }

    IEnumerator Wait()
    {
        yield return null;

    }

    IEnumerator Process()
    {
        yield return null;
    }

    IEnumerator Do()
    {
        yield return null;
    }
}
