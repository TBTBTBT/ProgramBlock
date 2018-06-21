using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class FishBase : MonoBehaviour
{
    protected const int ParamMax = 20;
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
    public CharaParam Param { get; set; }
    private float _direction = 0;
    public float ActualDirection
    {
        get { return _direction; }
    }
    public float AimDirection { get; set; }//向き 0 = 右
    public GameObject Target { get; set; }//敵

    List<GameObject> _parts;
    
    private bool isInit = false;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        DefaultParam();
    }

    void Update()
    {
        if (isInit)
        {
            Rotate();
            Move();
            CheckField();
        }
    }



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
        Init();
        isInit = true;
    }
    void DefaultParam()
    {
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
    
    void InstantiateParts(string path,Vector3 pos){
        //プレハブが整ったら
        //        GameObject go = (GameObject)Instantiate(Resources.Load(path), _partsRoot);
        //        go.transform.localPosition = pos;
        //        _parts.Add(go);
        
    }

    float OverCheck(float input, float max)
    {
        if (Mathf.Abs(input) > Mathf.Abs(max))
        {
            return  max * Mathf.Sign(input);

        }

        return input;
    }
    void CheckField()
    {
        Vector2 pos = transform.position;
        Vector2 field = GameManager.Instance.FieldSize;
        pos.x = OverCheck(pos.x, field.y);
        pos.y = OverCheck(pos.y, field.y);
        transform.position = pos;
    }
    void Rotate()
    {
        _direction = Mathf.LerpAngle(_direction, AimDirection, 0.2f);
        transform.localRotation =Quaternion.AngleAxis(_direction, new Vector3(0, 0, 1));
    }

    void CheckHitEnemy(Collider2D collision,UnityAction<FishBase> cb)
    {
        FishBase enemy = collision.gameObject.GetComponent<FishBase>();
        if (enemy != null)
        {
            cb(enemy);
        }

        Target = enemy.gameObject;
    }
    void Damage(FishBase enemy)
    {
        Param.Hp -= enemy.Param.Attack;
    }
    // Update is called once per frame

    void KnockBack(FishBase enemy)
    {

        _rigidbody.velocity += (Vector2)(transform.position - (enemy.transform.position)).normalized * (enemy.Param.Attack * 2.0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInit)
        {
            CheckHitEnemy(collision,Damage);


        }
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
	    if (isInit)
	    {
	       CheckHitEnemy(collision, KnockBack);
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
