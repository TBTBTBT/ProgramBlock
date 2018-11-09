using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class UnitCore : MonoBehaviourWithStatemachine<UnitCore.State> {

    //---------------------------------------------------------
    //states
    //---------------------------------------------------------
    public enum State
    {
        Init,
        Wait,
        Process,
    }
    //---------------------------------------------------------
    //class
    //---------------------------------------------------------

    //---------------------------------------------------------
    //fields
    //---------------------------------------------------------
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
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
    public Vector2 KnockBakcVelocity { get; set; }
    public float Friction { get; set; }

    public Vector2 Direction { get; set; }

    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    public void Setup(string program){
        Debug.Log("SetUpProgram");
        _program = Interpreter.Parse(program);
    }
	public void StartProcess()
	{
        if (Current == State.Wait)
        {
            Next(State.Process);
        }
	}

    //phisics

    void Move()
    {

        Velocity *= Friction;

    }

    //basicCommand

    public void AddVelocity(Vector2 velocity)
    {
        Velocity += velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        Velocity = velocity;
    }

    public void KnockBack(Vector2 velocity)
    {
        KnockBakcVelocity += velocity;
    }
    public void Action(int num)
    {
        ObjectManager.Instance.InstantiateObject(num);
    }

    public void SetDirection(Vector2 dir)
    {

    }
    //sequence

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
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
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
        while (true)
        {
            Move();

            if (wait == 0)
            {
                _nowPointer = Interpreter.Execute(_program.OrderList[_nowPointer.x, _nowPointer.y],out wait);
            }

            if (wait > 0)
            {
                wait--;
            }

            yield return null;
        }
    }

}
