using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class FishBase : MonoBehaviour
{
    public enum AggressiveState
    {
        None,
        Attack,
        Angry,
        Escape
    }
    [SerializeField] private Transform _partsRoot;
    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;

    public int Team { get; set; }//所属
    private CharaParam _param;
    public CharaParam Param
    {
        get
        {
            Debug.Log(_param);
            return _param;
        }
        set
        {
            Debug.Log(value);
            _param = value; } }

    List<GameObject> _parts;
    Vector2 _direction;//向き
    GameObject _target;//敵



    protected abstract void Init();
    protected abstract void Move();
	//protected abstract void OnDamage();

    public void InitData(List<PartsData> parts){
        if (parts.Count > 2)
        {
            Param = new CharaParam()
            {

                Height = FishMasterData.GetBody(parts[0]._id).Height,
                Attack = FishMasterData.GetBody(parts[0]._id).Attack + FishMasterData.GetEye(parts[1]._id).Attack,
                Weight = FishMasterData.GetBody(parts[0]._id).Weight,
                Aggressive = FishMasterData.GetEye(parts[1]._id).Aggressive,
                Hp = 0,
                Speed = 0,
                Agility = 0
            };


            for (int i = 2; i < parts.Count; i++)
            {
                Param.Weight += FishMasterData.GetFin(parts[i]._id).Weight;
                Param.Height += FishMasterData.GetFin(parts[i]._id).Height;
                Param.Sight += FishMasterData.GetFin(parts[i]._id).Sight;
                Param.Attack += FishMasterData.GetFin(parts[i]._id).Attack;
            }

            ParamInit();
            InstantiateParts("Body:" + parts[0]._id, parts[0]._pos);
            InstantiateParts("Eye:" + parts[1]._id, parts[0]._pos);
            for (int i = 2; i < parts.Count; i++)
            {
                InstantiateParts("Fin:" + parts[i]._id, parts[0]._pos);
            }
        }
        else
        {
            DefaultParam();
            ParamInit();
        }
    }
    void InstantiateParts(string path,Vector3 pos){
        //プレハブが整ったら
        //        GameObject go = (GameObject)Instantiate(Resources.Load(path), _partsRoot);
        //        go.transform.localPosition = pos;
        //        _parts.Add(go);
        
    }
	void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        //DummyParam();
       
        Init();
        
    }

    public void SetTarget(GameObject go)
    {
        _target = go;
    }
    void DefaultParam(){
        Param = new CharaParam()
        {
            Hp = 1,
            Height = 1,
            Attack = 1,
            Weight = 1,
            Speed = 1,
            Agility = 1,
            Sight = 1
        };
    }
    void ParamInit()
    {
        Param.Init();   
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
            if(enemy.Param!=null)
            Damage(enemy.Param.Attack);
        }
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
        FishBase enemy = collision.gameObject.GetComponent<FishBase>();
        if (enemy != null)
        {
            if (enemy.Param != null)
                KnockBack(transform.position - collision.transform.position,enemy.Param.Attack * 1.0f);
        }
	}
	void Damage(int d)
    {
        Param.Hp -= d;
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
    //20段階
    public int Weight { get; set; }
    public int Height { get; set; }
    public int Aggressive { get; set; }
    public int Attack { get; set; }
    public int Sight { get; set; }

    public int Hp { get; set; }
    public int Speed { get; set; }
    public int Agility { get; set; }

    public void Init(){
        Hp = (int)(Weight + Height*1.5f);
        Speed = 20 - Weight;
        Agility = 20 - Height;

    }
}
