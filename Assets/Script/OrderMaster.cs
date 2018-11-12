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
        {"v0", (unit,param) =>
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
        //ロックオン(近い敵)
        {"v30", (unit,param) => {
            UnitCore target = unit.Manager.Units.FindAll(_=>_.TeamId != unit.TeamId)
                .OrderBy(_=>(_.transform.position - unit.transform.position).magnitude)//ここの条件次第
                .FirstOrDefault();
            if(target!= null)
            {

                unit.Target = target;
            }
            return true;
        }},
        {"b0", (unit,param)  => {
            if (unit.Target == null)
            {
                return false;
            }
            return (unit.Target.transform.position - unit.transform.position).magnitude < param;
        }},
        {"b1", (unit,param)  => {
            if (unit.Target == null)
            {
                return false;
            }
            return (unit.Target.transform.position - unit.transform.position).magnitude < param;
        }},
        {"b10", (unit,param)  => {
            if (unit.Target == null)
            {
                return false;
            }
            return (unit.Target.transform.position - unit.transform.position).magnitude < param;
        }},
        {"b11", (unit,param)  => {
            if (unit.Target == null)
            {
                return false;
            }
            return (unit.Target.transform.position - unit.transform.position).magnitude < param;
        }},
        {"b12", (unit,param)  => {
            if (unit.Target == null)
            {
                return false;
            }
            return (unit.Target.transform.position - unit.transform.position).magnitude < param;
        }},
        {"b20", (unit,param)  => {
            if (unit.Target == null)
            {
                return false;
            }
            return (unit.Target.transform.position - unit.transform.position).magnitude < param;
        }},
    };
    
}
//必要な命令

//前進後退左右移動
//ターゲット(近、遠、HP、MP、攻撃力大小、こうげきしてきたやつ)
//ターゲット(味方)
//ターゲット（）

//アクション一覧（攻防魔）

//自分の前を攻撃
//突進
//遠距離直進
//遠距離挟み込み
//対象回復
//範囲回復

//しらべる一覧

//ターゲットとの距離(一定以下か)
//ターゲットの状態（HP,MP）
//自分の状態
//敵弾との距離

//没案

//(注目(近、遠、HP、MP、攻撃力大小))