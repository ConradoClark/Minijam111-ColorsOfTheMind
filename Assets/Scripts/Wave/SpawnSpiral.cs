using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSpiral : EffectPoolable
{
    public float DurationInSeconds;
    public ScriptPrefab SpawnParticle;
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        DefaultMachinery.AddBasicMachine(SpawnParticles());
        DefaultMachinery.AddBasicMachine(HandleTimer());
    }

    private IEnumerable<IEnumerable<Action>> SpawnParticles()
    {
        while (!IsEffectOver)
        {
            if (transform.gameObject != null && transform.gameObject.activeSelf && SpawnParticle.Pool.TryGetFromPool(out var effect))
            {
                effect.Component.transform.localScale = Vector3.one;
                effect.Component.transform.SetParent(transform);
                effect.Component.transform.localPosition = Random.insideUnitCircle * 0.15f + Random.insideUnitCircle * 0.15f;
            }
            yield return TimeYields.WaitMilliseconds(GameTimer, 50);
        }
    }

    private IEnumerable<IEnumerable<Action>> HandleTimer()
    {
        yield return TimeYields.WaitSeconds(GameTimer, DurationInSeconds);

        yield return transform.GetAccessor()
            .UniformScale()
            .SetTarget(0)
            .Over(1f)
            .UsingTimer(GameTimer)
            .Easing(EasingYields.EasingFunction.BackEaseIn)
            .Build();

        EndEffect();
    }

    public override void OnActivation()
    {
    }
}
