using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class CharacterGettingHit : BaseGameObject
{
    public int HitPoints = 3;
    public int CurrentHitPoints { get; private set; }
    public event Action OnHit;

    public LichtPhysicsObject PhysicsObject;
    public ScriptIdentifier HitTrigger;
    public float HitCooldownInSeconds;

    private bool _enabled;

    private void OnEnable()
    {
        _enabled = true;
        CurrentHitPoints = HitPoints;
        DefaultMachinery.AddBasicMachine(HandleHit());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> HandleHit()
    {
        while (_enabled)
        {
            if (PhysicsObject.GetPhysicsTrigger(HitTrigger))
            {
                CurrentHitPoints--;
                OnHit?.Invoke();
                yield return TimeYields.WaitSeconds(GameTimer, HitCooldownInSeconds);
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }
}
