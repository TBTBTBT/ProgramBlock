﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Interpreter
{
    //命令を実行し次の行を返す
    public static ProgramFormat Parse(string program)
    {
        ProgramFormat ret = new ProgramFormat();
        string[] orders = program.Split(';');
        foreach (var order in orders)
        {
            string[] parse = order.Split(':');
            if (parse.Length > 4)
            {
                string[] pos = parse[0].Split(',');
                Debug.Log(order);
                Debug.Log(parse[0]);
            }
            
        }
        //ret.ParseOrders(orders);
        return ret;
    }
    //命令を実行し次の行を返す
    public static Vector2Int Execute(ProgramFormat.OrderFormat order, out int wait)
    {
        //マスターから引く
        Vector2Int next = new Vector2Int(0, 0);
        wait = 1;
        //xx:xx:xx
        return next;
    }
    public static string Stringify(ProgramFormat program)
    {
        string str = "";
        for (int i = 0; i < program.OrderList.GetLength(0);i ++){
            for (int j = 0; j < program.OrderList.GetLength(1); j++)
            {
                str += program.OrderList[i, j];
            }

        }
        return str;
    }
}
//heatX
//hp
//