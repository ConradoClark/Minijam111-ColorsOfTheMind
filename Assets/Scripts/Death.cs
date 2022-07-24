using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class Death : BaseGameObject
{
    public LayerMask DeathLayerMask;
    private CharacterGettingHit _hit;
    private DeathScreen _deathScreen;
    protected override void OnAwake()
    {
        base.OnAwake();
        _hit = SceneObject<CharacterGettingHit>.Instance();
        _deathScreen = SceneObject<DeathScreen>.Instance(true);
    }

    private void OnEnable()
    {
        _hit.OnHit += OnHit;
    }

    private void OnDisable()
    {
        _hit.OnHit -= OnHit;
    }
    private void OnHit()
    {
        if (_hit.CurrentHitPoints > 0) return;
        _hit.PhysicsObject.enabled = false;
        DefaultMachinery.AddBasicMachine(DeathEffect());
    }

    private IEnumerable<IEnumerable<Action>> DeathEffect()
    {
        Camera.main.cullingMask = DeathLayerMask;

        yield return _hit.transform.GetAccessor()
            .Position
            .ToPosition(Vector3.zero)
            .Over(2f)
            .UsingTimer(GameTimer)
            .Build();

        _deathScreen.Activate();
    }
}
