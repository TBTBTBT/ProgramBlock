using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class OrderMaster : MonoBehaviour {
    private static readonly Dictionary<string,Func<UnitCore, bool>> _functions = new Dictionary<string, Func<UnitCore, bool>>()
    {
        //前進
        {"v1", _ => {
         //  _.Velocity
            return true;
        }},
        //後退
        {"v2", _ => {
            //  _.Velocity
            return true;
        }},
        //注目
        {"v3", _ => {
            //  _.Velocity
            return true;
        }},
        {"b1", _ => {
            //  _.Velocity
            if (true)
            {
                return true;
            }
            return false;
        }},

    };
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
