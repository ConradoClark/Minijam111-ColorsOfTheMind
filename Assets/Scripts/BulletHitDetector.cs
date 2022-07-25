using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemies;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletHitDetector : BaseGameObject
{
    public bool HasColorRestriction;
    public Color HitByColor;
    public float HitCooldownInMs;
    public LichtPhysicsCollisionDetector CollisionDetector;
    public LichtPhysicsObject PhysicsObject;
    private LichtPhysics _physics;

    private bool _enabled;

    public event Action<BulletContact> OnHit;

    private EnemyHitSoundEffect _enemyHitSoundEffect;

    protected override void OnAwake()
    {
        base.OnAwake();
        _physics = this.GetLichtPhysics();
        _enemyHitSoundEffect = SceneObject<EnemyHitSoundEffect>.Instance();
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

            if (!default(CollisionResult).Equals(trigger) && (!HasColorRestriction || HitByColor == bullet.BulletColor))
            {
                _enemyHitSoundEffect.AudioSource.pitch = 0.9f + Random.value * 0.2f;
                _enemyHitSoundEffect.AudioSource.Play();
                OnHit?.Invoke(bullet);
                yield return TimeYields.WaitMilliseconds(GameTimer, HitCooldownInMs);
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }
}
