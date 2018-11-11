using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public static class OrderMaster{
    public static readonly Dictionary<string,Func<UnitCore,int, bool>> Functions = new Dictionary<string, Func<UnitCore,int, bool>>
    {
        //Move
        {"v1", (unit,param) =>
        {
            unit.SetVelocity(unit.Direction * ((float)unit.Spd));
                Debug.Log(unit.Velocity);
            return true;
        }},
        //注目(近い敵)
        {"v10", (unit,param) => {
                UnitCore target = unit.Manager.Units.FindAll(_=>_.TeamId != unit.TeamId)
                                      .OrderBy(_=>(_.transform.position - unit.transform.position).magnitude)//ここの条件次第
                                      .FirstOrDefault();
                if(target!= null){
                    
                    unit.Direction = (target.transform.position - unit.transform.position).normalized;
                }
            return true;
        }},
        //行動
        {"v20", (unit,param)  => {
                ObjectManager.Instance.InstantiateObject(param);
                return true;
        }},
        {"b1", (unit,param)  => {
            if (true)
            {
                return true;
            }
            return false;
        }},

    };

}
