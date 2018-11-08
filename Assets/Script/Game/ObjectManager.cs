using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    private Dictionary<int, GameObject> _prefabs = new Dictionary<int, GameObject>();
    protected override void Awake()
    {
        //参照
        //MstObjectRecord
        //Resources.Load<GameObject>
    }

    public GameObject InstantiateObject(int id)
    {
        if (_prefabs.ContainsKey(id))
        {
            return Instantiate(_prefabs[id]);
        }

        return null;
    }


}
