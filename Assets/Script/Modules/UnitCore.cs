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

    //---------------------------------------------------------
    //fields
    //---------------------------------------------------------

    //Program

    ProgramFormat _program;
    Vector2Int _nowPointer = new Vector2Int(0, 0);
    //---------------------------------------------------------
    //properties
    //---------------------------------------------------------
    public int Hp { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Spd { get; set; }

    //phisics

    public Vector2 Velocity { get; set; }
    public float Friction { get; set; }



    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    public void Setup(string program){
        _program = Interpreter.Parse(program);
    }
	public void StartProcess()
	{
        if (Current == State.Wait)
        {
            Next(State.Process);
        }
	}
	IEnumerator Init()
    {
        yield return null;
        //params初期化
        Hp = 100;
        Atk = 1;
        Def = 1;
        Spd = 1;
        Velocity = new Vector2(0,0);
        Friction = 1;
        _nowPointer = new Vector2Int(0, 0);
        //MasterData参照
        //MasterdataManager.Get<MstUnitRecord>();
        while(_program == null){
            yield return null;
        } 
    }
    IEnumerator Wait()
    {
        
        yield return null;

    }

    IEnumerator Process()
    {
        int wait = 0;//masterから
        _nowPointer = Interpreter.Execute(_program.OrderList[_nowPointer.x, _nowPointer.y]);
        while(wait > 0){
            wait--;
            yield return null;
        }
        yield return null;
    }

    IEnumerator Do()
    {
        yield return null;
    }
}
