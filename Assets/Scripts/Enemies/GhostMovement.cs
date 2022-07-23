using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostMovement : BaseGameObject
{
    public LichtPhysicsObject PhysicsObject;
    public SpriteRenderer SpriteRenderer;
    private bool _enabled;

    private Player _player;
    protected override void OnAwake()
    {
        base.OnAwake();
        _player = SceneObject<Player>.Instance();
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(AdjustTurn());
        DefaultMachinery.AddBasicMachine(CircleAroundCharacter());
        DefaultMachinery.AddBasicMachine(MoveTowardsCharacter());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> AdjustTurn()
    {
        while (_enabled)
        {
            SpriteRenderer.flipX = _player.transform.position.x < transform.position.x;
            yield return TimeYields.WaitOneFrameX;
        }
    }

    private IEnumerable<IEnumerable<Action>> MoveTowardsCharacter()
    {
        while (_enabled)
        {
            var dist = Vector2.Distance(_player.transform.position, transform.position);
            while (dist > 0.1f)
            {
                dist = Vector2.Distance(_player.transform.position, transform.position);

                PhysicsObject.ApplySpeed(Vector2.MoveTowards(transform.position, _player.transform.position,
                    0.2f) - (Vector2)transform.position);

                yield return TimeYields.WaitOneFrameX;
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }

    private IEnumerable<IEnumerable<Action>> CircleAroundCharacter()
    {
        var angle = 0f;
        var face = Random.value>0.5f;

        while (_enabled)
        {
            var radius = Vector2.Distance(_player.transform.position, transform.position) * 0.35f;
            angle += (float) GameTimer.UpdatedTimeInMilliseconds * 0.0025f;

            PhysicsObject.ApplySpeed(face
                ? new Vector2(Mathf.Cos(angle * 0.5f) * radius, Mathf.Sin(angle * 0.5f) * radius)
                : new Vector2(Mathf.Sin(angle * 0.5f) * radius, Mathf.Cos(angle * 0.5f) * radius));

            yield return TimeYields.WaitOneFrameX;
        }
    }

    private IEnumerable<IEnumerable<Action>> Attack()
    {
        yield return TimeYields.WaitOneFrameX;
    }
}
