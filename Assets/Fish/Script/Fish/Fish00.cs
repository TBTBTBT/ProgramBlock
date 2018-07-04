using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish00 : FishBase
{
    private TickEvent _onMove;
    private float _friction = 0.9f;
    private float _speed = 0;
    protected override void Init()
    {
        int moveBlank = (ParamMax+5 - Param.Agility)*6;

        _onMove = new TickEvent(moveBlank);
        _onMove.AddListener(TimingMovement);
       
        
	}

    protected override void Move()
    {
        _onMove.Update();
        Friction(_friction);
        MoveToForward();
        if (Target)
        {
            //AngleLock();
        }
    }

    protected override void OnHitEnemy(FishBase enemy)
    {

    }
    void Friction(float f)
    {
        _speed *= f;
        _rigidbody.velocity *= f;
    }

    void TimingMovement()
    {
        if (Target == null)
        {
            AngleChange();
        }
        else{
            AngleLock();
        }
        AddSpeed();
        UpdateEmotion();

    }
    void AddSpeed()
    {
        
        _speed = 1;
    }

    void UpdateEmotion()
    {
        Emotion.UpdateEmotion();
    }
    void AngleLock()
    {
        AimDirection = MathUtil.GetAim(transform.position, Target.transform.position);
        Mathf.Repeat(AimDirection, 360);
    }
    void AngleChange()
    {
  
            AimDirection += Random.Range(-60, 60);
            Mathf.Repeat(AimDirection, 360);


    }
    void MoveToForward()
    {
        
        _rigidbody.velocity += new Vector2(Mathf.Cos(ActualDirection * Mathf.PI/180), Mathf.Sin(ActualDirection * Mathf.PI / 180)) * _speed;
    }
}
