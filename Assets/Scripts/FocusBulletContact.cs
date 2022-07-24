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

public class FocusBulletContact : BulletContact
{
    public ScriptPrefab Self;
    public LichtPhysicsCollisionDetector CollisionDetector;
    private BulletPool _bulletPool;
    protected override void OnAwake()
    {
        base.OnAwake();
        if (Self != null) _bulletPool = SceneObject<BulletPoolManager>.Instance().GetEffect(Self);
    }

    protected override IEnumerable<IEnumerable<Action>> Move()
    {
        yield return TimeYields.WaitOneFrameX;
        if (InitialSpeed.magnitude == 0) yield break;

        //yield return PhysicsObject.GetSpeedAccessor()
        //    .ToSpeed(InitialSpeed * BulletSpeed)
        //    .Over(1f)
        //    .UsingTimer(GameTimer)
        //    .BreakIf(()=> IsEffectOver)
        //    .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
        //    .Build();

        while (!IsEffectOver)
        {
            PhysicsObject.ApplySpeed(InitialSpeed * BulletSpeed);
            yield return TimeYields.WaitOneFrameX;
        }
    }

    protected override IEnumerable<IEnumerable<Action>> HandleContact()
    {
        // resets gravity accel for object
        Physics.BlockCustomPhysicsForceForObject(this, PhysicsObject, Gravity.Name);
        yield return TimeYields.WaitOneFrameX;
        Physics.UnblockCustomPhysicsForceForObject(this, PhysicsObject, Gravity.Name);

        while (isActiveAndEnabled)
        {
            if (PhysicsObject.GetPhysicsTrigger(Contact))
            {
                if (!IsSubSpawn)
                {
                    if (_bulletPool != null && _bulletPool.TryGetFromPool(out var bullet))
                    {
                        bullet.IsSubSpawn = true;

                        var colliderB = CollisionDetector.Triggers.FirstOrDefault().Collider;

                        if (colliderB != null)
                        {
                            var dist = Physics2D.Distance(CollisionDetector.Collider, colliderB);

                            bullet.InitialSpeed = InitialSpeed * new Vector2(
                                Math.Sign(dist.normal.x) == Math.Sign(InitialSpeed.x) ? -1 : 1,
                                Math.Sign(dist.normal.y) == Math.Sign(InitialSpeed.y) ? -1 : 1
                            );
                            bullet.transform.position = transform.position + (Vector3)bullet.InitialSpeed * 0.1f;
                        }
                    }
                }

                if (SplatterEffect.Pool.TryGetFromPool(out var splatter))
                {
                    splatter.Component.transform.position = transform.position;
                }

                if (PaintPool.TryGetManyFromPool(2, out var paints))
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

                EndEffect();
                break;
            }
            yield return TimeYields.WaitMilliseconds(GameTimer, 10);
        }
    }
}
