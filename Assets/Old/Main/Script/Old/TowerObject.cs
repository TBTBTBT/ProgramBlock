using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Analytics;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class TowerObject : MonoBehaviour {
    List<Module> _modules = new List<Module>();

    [SerializeField]private Transform _modParent;

    [SerializeField] private BoxCollider _collider;
    private int _id = -1;
    private int _member = -1;
    public Rigidbody _rigidbody;
    private int _nowModuleNum = 0;

    private float _angle = 0;
    private float _aimAngle = 0;
	// Use this for initialization
	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
		BattleManager.Instance.OnExecute.AddListener(Execute);
	}

    #region Moduleが関数内で呼び出せる関数

    public void Shot()
    {

    }

    public void SetId(int id,int mem)
    {//id,所属
        _id = id;
        _member = mem;
    }

    public int Id()
    {
        return _id;
    }

    public int Member()
    {
        return _member;
    }
    public void Move(Vector3 dir,float speed)
    {
        
        _rigidbody.velocity += (speed * dir)+new Vector3(0,3,0);

    }

    public void LookNearBy()
    {
        TowerObject target = BattleManager.Instance.FindNearBy(_id, _member);
        transform.LookAt(target.transform,new Vector3(0,1,0));
    }

    #endregion

    void IncrementModNum(int num)
    {
        _nowModuleNum += num;
        if (_nowModuleNum >= _modules.Count) _nowModuleNum = 0;
        if (_nowModuleNum < 0) _nowModuleNum = _modules.Count-1;
    }
    void Execute()
    {

        ModuleMaster.GetFunction(_modules[_nowModuleNum]._number)(this);
        IncrementModNum(1);
    }

    public void InitModules(List<int> modnum)
    {
        for (var i = 0; i < modnum.Count; i++)
        {
            Module m = Instantiate(Resources.Load<GameObject>("Modules/Module_" + modnum[i]), _modParent)
                .GetComponent<Module>();
            m._number = modnum[i];
            _modules.Add(m);
        }

        AlignModule();
    }

    void AlignModule()
    {
        for (var i = 0; i < _modules.Count; i++)
        {
            _modules[i].gameObject.transform.localPosition = new Vector3(0, _modules.Count - i - 1, 0);
        }
        float h = (float)_modules.Count;
        _collider.size = new Vector3(1,h,1);
        
        _collider.center = new Vector3(0,h/2 - 0.5f, 0);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
