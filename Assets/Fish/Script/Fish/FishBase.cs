using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class FishBase : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;
    public CharaParam _param;

    Vector2 _direction;//向き


    protected abstract void Init();
    protected abstract void Move();
    //protected abstract void OnDamage();


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        ParamInit();
        Init();
    }
    void ParamInit()
    {
        _param = new CharaParam()
        {
            Hp = 10,
            Attack = 1,
            Weight = 1,
            Speed = 1,
            Agility = 1
        };
    }
    // Update is called once per frame
    void Update()
    {
        Move();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FishBase enemy = collision.gameObject.GetComponent<FishBase>();
        if (enemy!=null)
        {
            Damage(enemy._param.Attack);
        }
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
        FishBase enemy = collision.gameObject.GetComponent<FishBase>();
        if (enemy != null)
        {
            KnockBack(transform.position - collision.transform.position,enemy._param.Attack * 1.0f);
        }
	}
	void Damage(int d)
    {
        _param.Hp -= d;
    }

    void KnockBack(Vector2 dir,float kb){
        _rigidbody.velocity += kb * dir;
    }
}
public class TickEvent{
    int _time;
    int _max;
    UnityEvent OnTime = new UnityEvent();
    public TickEvent(int max){
        _max = max;
    }
    public void AddListener(UnityAction cb){
        OnTime.AddListener(cb);
    }
    public void Update(){
        _time++;
        if(_time > _max){
            _time = 0;
            OnTime.Invoke();
        }
    }
}
public class CharaParam{
    public int Hp { get; set; }
    public int Attack{ get; set; }
    public int Weight{ get; set; }
    public int Speed { get; set; }
    public int Agility { get; set; }
    public int Aggressive { get; set; }
}