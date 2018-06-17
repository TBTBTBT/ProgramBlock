using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FishBase))]
public class FishAnimationBase : MonoBehaviour {
    [SerializeField] Animator _animator;
    FishBase _fish;

	private void Awake()
	{
        _fish = GetComponent<FishBase>();
	}
	private void CheckRotate()
    {
        _animator.SetTrigger("Left");
    }

}
