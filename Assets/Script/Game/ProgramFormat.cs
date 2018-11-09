using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProgramFormat
{
    public class OrderFormat
    {
        public Vector2Int yes = new Vector2Int(0,0);
        public Vector2Int no = new Vector2Int(0, 0);
        public string key = "";
    }
    public OrderFormat[,] OrderList = new OrderFormat[5,5];

    public void AddOrder(Vector2Int pos, string order, Vector2Int next, Vector2Int next2 )
    {
        OrderList[pos.x, pos.y] = new OrderFormat()
        {
            key = order,
            yes = next,
            no = next2,
        };
        //OrderList[pos.x, pos.y] = $"{pos.x},{pos.y}:{order}:{next.x},{next.y}:{next2.x}:{next2.y};";
    }

}
