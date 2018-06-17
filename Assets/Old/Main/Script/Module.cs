using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    public int _number = 0;
    private float _angle = 0;
    public bool DoFunction(bool execute)
    {
        return false;
    }
}
