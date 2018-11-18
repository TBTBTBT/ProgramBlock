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

    //gameInfo
    public int TeamId { get; set; }
    public GameManager Manager { get; set; }
    //Thinking
    public UnitCore Target { get; set; }
    //phisics

    public Vector2 Velocity { get; set; }
    public Vector2 KnockBakcVelocity { get; set; }
    public float Friction { get; set; }

    public Vector2 Direction { get; set; }

    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    public void Setup(long id,GameManager manager,int team,string program){
        Manager = manager;
        TeamId = team;
        Debug.Log("SetUpProgram");
        _program = Interpreter.Parse(program);
    }
	public void StartProcess()
	{
        //if (Current == State.Wait)
       // {
            Next(State.Process);
       // }
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
    //phisics

    void Move()
    {

        Velocity *= Friction;
        _rigidbody.velocity = Velocity;
        //_rigidbody.S
    }
    //---------------------------------------------------------
    //sequence    
    //---------------------------------------------------------
    void SetParams(){
        //params初期化
        Hp = 100;
        Atk = 1;
        Def = 1;
        Spd = 1;
        Velocity = new Vector2(0, 0);
        Direction = new Vector2(0, 1);
        Friction = 0.9f;
        _nowPointer = new Vector2Int(0, 0);
        //MasterData参照
    }
    IEnumerator Init()
    {


        //MasterdataManager.Get<MstUnitRecord>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        Next(State.Wait);
        yield return null;
    }
    IEnumerator Wait()
    {
        
        yield return null;

    }

    IEnumerator Process()
    {
        SetParams();
        int wait = 0;//masterから
        while (true)
        {
            Move();

            if (wait == 0)
            {
                _nowPointer = Interpreter.Execute(this,_program.OrderList[_nowPointer.x, _nowPointer.y],out wait);
                Debug.Log(_nowPointer);
            }

            if (wait > 0)
            {
                wait--;
            }

            yield return null;
        }
    }

}
