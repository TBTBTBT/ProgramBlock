using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class ObjectCore : MonoBehaviourWithStatemachine<ObjectCore.State> {
    public enum State
    {
        Init,
        Process,
        End
    }
    public int Order { get; set; }
    public int Atk { get; set; }
    public int Spd { get; set; }
    public int TeamId { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 KnockBakcVelocity { get; set; }
    public float Friction { get; set; }

    public Vector2 Direction { get; set; }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }
    public void SetUp(long id, int teamId)
    {
        TeamId = teamId;
        Next(State.Process);
    }
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    void Move()
    {

        Velocity *= Friction;
        _rigidbody.velocity = Velocity;
    }

    IEnumerator Process()
    {
        yield return null;
    }

    IEnumerator End()
    {
        yield return null;
    }
}
