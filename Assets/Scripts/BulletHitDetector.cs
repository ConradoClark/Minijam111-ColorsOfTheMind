using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class BulletHitDetector : BaseGameObject
{
    public float HitCooldownInMs;
    public LichtPhysicsCollisionDetector CollisionDetector;
    public LichtPhysicsObject PhysicsObject;
    private LichtPhysics _physics;

    private bool _enabled;

    public event Action<BulletContact> OnHit;

    protected override void OnAwake()
    {
        base.OnAwake();
        _physics = this.GetLichtPhysics();
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(DetectHits());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> DetectHits()
    {
        BulletContact bullet = null;
        while (_enabled)
        {

            var trigger = CollisionDetector.Triggers.FirstOrDefault(t => t.TriggeredHit &&
                                                                         _physics.TryGetPhysicsObjectByCollider(
                                                                             t.Collider, out var target) &&
                                                                         target.TryGetCustomObject(out bullet));

            if (!default(CollisionResult).Equals(trigger))
            {
                OnHit?.Invoke(bullet);
                Debug.Log("Hit detected!");
                yield return TimeYields.WaitMilliseconds(GameTimer, HitCooldownInMs);
                // Hit detected!
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }
}
