using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramFormat
{

    public string[,] OrderList = new string[5,5];

    public void AddOrder(Vector2Int pos, string order, Vector2Int next, Vector2Int next2 )
    {
        OrderList[pos.x, pos.y] = order + $":{next.x},{next.y}:{next2.x}:{next2.y}";
    }
}
