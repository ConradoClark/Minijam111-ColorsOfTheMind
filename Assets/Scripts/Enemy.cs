using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class Enemy : EffectPoolable
{
    public int HitPoints;
    private int _currentHitPoints;

    public ScriptPrefab KillEffect;
    public BulletHitDetector HitDetector;

    public override void OnActivation()
    {
        
    }

    private void HitDetector_OnHit(BulletContact obj)
    {
        _currentHitPoints -= obj.BulletDamage;

        if (_currentHitPoints <= 0)
        {
            if (KillEffect.Pool.TryGetFromPool(out var effect))
            {
                effect.Component.transform.position = transform.position;
            }
            EndEffect();
        }
    }

    private void OnEnable()
    {
        _currentHitPoints = HitPoints;
        HitDetector.OnHit += HitDetector_OnHit;
    }

    private void OnDisable()
    {
        HitDetector.OnHit -= HitDetector_OnHit;
    }
}
