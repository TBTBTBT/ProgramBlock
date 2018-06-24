using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _sInstance;

    public static T Instance
    {
        get
        {
            if (_sInstance == null)
            {
                Type t = typeof(T);
                _sInstance = (T)FindObjectOfType(t);
            }
            if (_sInstance == null)
            {
                Debug.Log(typeof(T) + "が存在しません");
            }
           
            return _sInstance;
        }
    }

    //public static bool Exists => _sInstance != null;

    protected virtual void Awake()
    {
        if (_sInstance != null)
        {
            // 多重起動
            
            Debug.Log(typeof(T) + "Singletonが多重起動されています");
            Destroy(this);
            return;
        }

        _sInstance = (T)this;


    }


}