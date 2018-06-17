using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 命令を保存しておくクラス
/// </summary>
public static class ModuleMaster
{

    private static readonly List<Func<TowerObject,bool>> _functions = new List<Func< TowerObject, bool>>()
    {
        //前進
        tower => {
                tower.Move(tower.transform.forward,5);
                return true;
        },
        //後退
        tower =>
        {
                tower.Move(-tower.transform.right,5);
                return true;
        },
        //注目
        tower =>
        {
                tower.LookNearBy();
                return true;
        },
        //注目
        tower =>
        {
            tower.LookNearBy();
            return true;
        }

    };

    public static Func<TowerObject, bool> GetFunction(int m)
    {
        if (_functions.Count>m)
        {
            return _functions[m];
        }

        return null;
    }

}
