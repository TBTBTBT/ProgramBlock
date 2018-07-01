using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.Input;

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

    public enum BattleState
    {
        None,
        Attack,
        Damage,
        Run
    }
    [SerializeField] private Transform _partsRoot;
    [SerializeField] private List<Renderer> _partsRenderer;
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

   // List<GameObject> _parts;
    
    private bool isInit = false;
    protected AggressiveState _aggressiveState { get; set; }
    protected BattleState _battleState { get; set; }
    protected EmotionModule Emotion { get; set; }//+ : 怒り - : 逃げ
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        DefaultParam();
        ChangeAggressiveState();
        Emotion = new EmotionModule(Param.Aggressive,ParamMax);
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

#region Abstract

    protected abstract void Init();

    protected abstract void Move();

    protected abstract void OnHitEnemy(FishBase enemy);

    //    protected abstract void WhileHitEnemy();
    #endregion


    /// <summary>
    /// パーツデータを魚のパラメータに変換
    /// </summary>
    public void InitData(FishData data){
        int fixedPartsCount = 2;
        if (data.Body._id >= 0 && data.Eye._id >= 0)
        {
            Param = new CharaParam()
            {

                Height = FishMasterData.GetBody(data.Body._id).Height,
                Attack = FishMasterData.GetBody(data.Body._id).Attack + FishMasterData.GetEye(data.Eye._id).Attack,
                Weight = FishMasterData.GetBody(data.Body._id).Weight,
                Aggressive = FishMasterData.GetEye(data.Eye._id).Aggressive,
                Hp = 0,
                Speed = 0,
                Agility = 0
            };


            for (int i = 0; i < data.Fin.Count; i++)
            {
                Param.Weight += FishMasterData.GetFin(data.Fin[i]._id).Weight;
                Param.Height += FishMasterData.GetFin(data.Fin[i]._id).Height;
                Param.Sight += FishMasterData.GetFin(data.Fin[i]._id).Sight;
                Param.Attack += FishMasterData.GetFin(data.Fin[i]._id).Attack;
            }
            ParamInit();


            InstantiateParts(0,FishMasterData.MaterialPath[PartsType.Body] + data.Body._id, data.Body._pos);
            InstantiateParts(1,FishMasterData.MaterialPath[PartsType.Eye] + data.Eye._id, data.Eye._pos);
            for (int i = 0; i < data.Fin.Count; i++)
            {
                InstantiateParts(i + fixedPartsCount,FishMasterData.MaterialPath[PartsType.Fin] + data.Fin[i]._id, data.Fin[i]._pos);
            }
        }
        else
        {
            DefaultParam();
            ParamInit();
            DefaultMaterial();
        }
        Init();
        isInit = true;
    }
    /// <summary>
    /// 初期値
    /// </summary>
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

    void DefaultMaterial()
    {
        InstantiateParts(0,FishMasterData.MaterialPath[PartsType.Body]  + 0, new Vector2(0,0));
        InstantiateParts(1,FishMasterData.MaterialPath[PartsType.Eye]  + 0 , new Vector2(1,0.5f));
    }
    void ParamInit()
    {
        Param.Init();

    }
    
    void InstantiateParts(int i,string path,Vector2 pos)
    {
        if (i < _partsRenderer.Count)
        {
            _partsRenderer[i].material = Resources.Load<Material>(path);
            _partsRenderer[i].material.SetVector("_RotateCenter", new Vector4(pos.x, pos.y, 0, 0));
            _partsRenderer[i].gameObject.SetActive(true);
        }

        //プレハブが整ったら
        //        GameObject go = (GameObject)Instantiate(Resources.Load(path), _partsRoot);
        //        go.transform.localPosition = pos;
        //        _parts.Add(go);
    }
    void Rotate()
    {
        _direction = Mathf.LerpAngle(_direction, AimDirection, 0.2f);
        transform.localRotation = Quaternion.AngleAxis(_direction, new Vector3(0, 0, 1));
    }
    void ChangeAggressiveState()
    {
        _aggressiveState = AggressiveState.None;
        if (Target)
        {

        }
    }


    /// <summary>
    /// はみ出しチェック
    /// </summary>
    /// <param name="input"></param>
    /// <param name="max"></param>
    /// <returns></returns>
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



    /// <summary>
    /// 敵との接触
    /// </summary>
    void CheckHitEnemy(Collider2D collision,UnityAction<FishBase> cb,bool isStay = false)
    {
        FishBase enemy = collision.gameObject.GetComponent<FishBase>();
        if (enemy != null)
        {
            cb(enemy);
        }

        if (!isStay)
        {
            OnHitEnemy(enemy);
        }

    }

    void Damage(FishBase enemy)
    {
        if (enemy.Team != Team)
        {
            Param.Hp -= enemy.Param.Attack;
            Emotion.AddAngry(Param.Aggressive - enemy.Param.Attack);
            Target = enemy.gameObject;
        }
    }
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
	       CheckHitEnemy(collision, KnockBack,true);
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
//感情システム
public class EmotionModule
{
    private const int EmotionMax = 100;
    public int Angry { get; set; }
    public int Emotion { get; set; }

    public EmotionModule(int aggressive,int maxaggressive)
    {
        Angry = aggressive - maxaggressive / 2;
        Emotion = 0;
    }
    public void UpdateEmotion()
    {
        Emotion += Angry;
        if (Mathf.Abs(Emotion) > EmotionMax)
        {
            Emotion = (int)Mathf.Sign(Emotion) * EmotionMax;
        }
    }
    public void AddAngry(int num)
    {
        Angry++;
    }
}