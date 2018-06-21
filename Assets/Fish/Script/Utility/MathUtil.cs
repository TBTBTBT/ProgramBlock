using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtil {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static float GetAim(Vector2 p1, Vector2 p2)
    {
        var dx = p2.x - p1.x;
        var dy = p2.y - p1.y;
        var rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }
    /// <summary>
    /// ランダムな方向の単位行列*半径を返す
    /// </summary>
    /// <param name="rad"></param>
    /// <returns></returns>
    public static Vector2 RandomAngle(float rad)
    {
        float angle = Random.Range(0, 360);
        return new Vector2(rad * Mathf.Cos(angle * Mathf.PI / 180), rad * Mathf.Sin(angle * Mathf.PI / 180));
    }
    /// <summary>
    /// 角度を近似する。
    /// </summary>
    /// <returns></returns>
}
