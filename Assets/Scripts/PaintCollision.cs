using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Paint;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaintCollision : EffectPoolable
{
    public LichtPhysicsObject PhysicsObject;
    public ContactFilter2D ContactFilter2D;
    public Collider2D Collider;
    private Collider2D[] _results;
    public Vector2 InitialSpeed;

    public ScriptPrefab LightweightPaint;
    private LightweightPaintPool _lightweightPaintPool;

    protected override void OnAwake()
    {
        base.OnAwake();
        _results = new Collider2D[1];
        _lightweightPaintPool = SceneObject<LightweightPaintPoolManager>.Instance().GetEffect(LightweightPaint);
    }

    private void OnEnable()
    {
        DefaultMachinery.AddBasicMachine(WatchCollision());
    }
    private IEnumerable<IEnumerable<Action>> WatchCollision()
    {
        while (isActiveAndEnabled)
        {
            if (PhysicsObject.enabled && Collider.OverlapCollider(ContactFilter2D, _results) > 0)
            {
                PhysicsObject.enabled = false;
                var dist = Physics2D.Distance(Collider, _results[0]);

                DefaultMachinery.AddBasicMachine(Leak(dist.normal));
                break;
            }

            yield return TimeYields.WaitOneFrameX;

        }
    }

    private IEnumerable<IEnumerable<Action>> Leak(Vector2 direction)
    {
        yield return TimeYields.WaitMilliseconds(GameTimer, 150);
        var leakDistance = Random.Range(0, 6);

        var latestOffset = Vector2.zero;

        for (var i = 0; i < leakDistance; i++)
        {
            if (_lightweightPaintPool.TryGetFromPool(out var paint))
            {
                var hDir = Mathf.Abs(direction.x) > 0 ? (Random.value > 0.5f ? Mathf.Sign(direction.x) : 0) : (Random.Range(-1, 2));
                var vDir = direction.y > 0 ? 0 : (Random.value > 0.5f ? 0 : -1);

                var offset = new Vector2(hDir, vDir) * 0.01f;
                latestOffset += offset;
                paint.transform.position = transform.position + (Vector3) latestOffset;
            }
            yield return TimeYields.WaitMilliseconds(GameTimer, 150);
        }
    }

    private IEnumerable<IEnumerable<Action>> Move()
    {
        yield return TimeYields.WaitOneFrameX;

        yield return PhysicsObject.GetSpeedAccessor(InitialSpeed)
            .ToSpeed(Vector2.zero)
            .Over(0.5f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .BreakIf(() => !PhysicsObject.enabled)
            .UsingTimer(GameTimer)
            .Build();
    }

    public override void OnActivation()
    {
        PhysicsObject.enabled = true;
        DefaultMachinery.AddBasicMachine(Move());
    }
}
