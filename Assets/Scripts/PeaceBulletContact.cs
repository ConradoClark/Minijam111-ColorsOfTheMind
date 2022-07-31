using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using Licht.Unity.Pooling;
using UnityEngine;

public class PeaceBulletContact : BulletContact
{
    public float HitCooldownInMs;
    public LichtPhysicsCollisionDetector CollisionDetector;
    protected override IEnumerable<IEnumerable<Action>> Move()
    {
        yield return TimeYields.WaitOneFrameX;
        if (InitialSpeed.magnitude == 0) yield break;

        yield return PhysicsObject.GetSpeedAccessor(InitialSpeed * BulletSpeed * 0.5f)
            .ToSpeed(Vector2.zero)
            .Over(0.5f)
            .UsingTimer(GameTimer)
            .BreakIf(()=> IsEffectOver)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Build();

        yield return PhysicsObject.GetSpeedAccessor()
            .ToSpeed(InitialSpeed * BulletSpeed * -1)
            .Over(1f)
            .UsingTimer(GameTimer)
            .BreakIf(() => IsEffectOver)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Build();

        while (!IsEffectOver)
        {
            PhysicsObject.ApplySpeed(InitialSpeed * BulletSpeed * -1);
            yield return TimeYields.WaitOneFrameX;
        }
    }

    protected override IEnumerable<IEnumerable<Action>> HandleContact()
    {
        // resets gravity accel for object
        Physics.BlockCustomPhysicsForceForObject(this, PhysicsObject, Gravity.Name);
        yield return TimeYields.WaitMilliseconds(GameTimer, 50);

        Physics.UnblockCustomPhysicsForceForObject(this, PhysicsObject, Gravity.Name);

        while (isActiveAndEnabled)
        {
            if (PhysicsObject.GetPhysicsTrigger(Contact))
            {
                if (SplatterEffect.Pool.TryGetFromPool(out var splatter))
                {
                    splatter.Component.transform.position = transform.position;
                }

                if (PaintPool.TryGetManyFromPool(5, out var paints))
                {
                    for (var index = 0; index < paints.Length; index++)
                    {
                        var paint = paints[index];
                        paint.Component.transform.position =
                            transform.position;

                        paint.SpriteRenderer.material = PaintMaterial;

                        paint.InitialSpeed = new Vector2(0, 0.2f) + DegreeToVector2(index * (360f / paints.Length)) * 0.25f;
                    }
                }

                var trigger = CollisionDetector.Triggers.FirstOrDefault().Collider;
                if (trigger != null && trigger.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                {
                    EndEffect();
                    break;
                }

                CollisionDetector.Collider.enabled = false;
                yield return TimeYields.WaitMilliseconds(GameTimer, HitCooldownInMs);
                CollisionDetector.Collider.enabled = true;

            }
            yield return TimeYields.WaitMilliseconds(GameTimer, 10);
        }
    }
}
