using System.Collections;
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
            if (parse.Length > 3)
            {
                string[] spos = parse[0].Split(',');
                string func = parse[1];
                string sparam = parse[2];
                string[] yes = parse[3].Split(',');
                Vector2Int pos = new Vector2Int(int.Parse(spos[0]), int.Parse(spos[1]));
                int param = int.Parse(sparam);
                Vector2Int next = new Vector2Int(int.Parse(yes[0]), int.Parse(yes[1]));
                Vector2Int next2 = new Vector2Int(0, 0);
                if (parse.Length > 4)
                {
                    string[] no = parse[4].Split(',');
                    next2 = new Vector2Int(int.Parse(no[0]), int.Parse(no[1]));
                }
                ret.AddOrder(pos,func,param,next,next2);
                Debug.Log(order);
            }
            
        }
        //ret.ParseOrders(orders);
        return ret;
    }
    //命令を実行し次の行を返す
    public static Vector2Int Execute(UnitCore self,ProgramFormat.OrderFormat order, out int wait)
    {
        //マスターから引く 
        Vector2Int next = new Vector2Int(0, 0);
        wait = 40;
        if(order == null){
            return next;
        }
        if(OrderMaster.Functions.ContainsKey(order.key)){
            if (OrderMaster.Functions[order.key](self, order.param))
            {
                next = order.yes;
            }
            else{
                next = order.no;
            }
        }
        wait = 20;
        //xx:xx:xx
        return next;
    }
    public static string Stringify(ProgramFormat program)
    {
        string str = "";
        for (int i = 0; i < program.OrderList.GetLength(0);i ++){
            for (int j = 0; j < program.OrderList.GetLength(1); j++)
            {
                if(program.OrderList[i, j] == null){
                    continue;
                }
                str += $"{i},{j}:";
                str += $"{ program.OrderList[i, j].key}:";
                str += $"{ program.OrderList[i, j].param}:";
                str += $"{ program.OrderList[i, j].yes.x},{program.OrderList[i, j].yes.y}:";
                str += $"{ program.OrderList[i, j].no.x},{program.OrderList[i, j].no.y};";
            }

        }
        return str;
    }
}
//heatX
//hp
//