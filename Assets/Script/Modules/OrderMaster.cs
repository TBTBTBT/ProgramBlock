using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public static class OrderMaster{
    private static readonly Dictionary<string,Func<UnitCore,int, bool>> _functions = new Dictionary<string, Func<UnitCore,int, bool>>
    {
        //Move
        {"v1", (unit,param) =>
        {
            unit.SetVelocity(unit.Direction * ((float)unit.Spd/10));
            return true;
        }},
        //注目
        {"v10", (unit,param)  => {
            //GameObject.FindAll(Enemy的な
            return true;
        }},
        //行動
        {"v20", (unit,param)  => {
            //GameObject.FindAll(Enemy的な
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
