using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MainCameraSingleton : SingletonMonoBehaviour<MainCameraSingleton> {
    private Camera Camera { get; set; }
    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    public Vector2 WorldMax()
    {
        return Camera.ViewportToWorldPoint(new Vector2(1, 1));
    }
    public Vector2 ScreenToWorld(Vector2 screen)
    {
        return Camera.ScreenToWorldPoint(screen);
    }
}
