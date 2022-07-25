using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterSmokeOnWalk : BaseGameObject
{
    public Vector3 Offset;
    public ScriptPrefab SmokeEffect;
    public ScriptBasicMachinery LateUpdate;
    public AudioSource StepSound;

    public void CreateSmoke()
    {
        LateUpdate.Machinery.AddBasicMachine(SpawnSmokeOnLateUpdate());
    }

    private IEnumerable<IEnumerable<Action>> SpawnSmokeOnLateUpdate()
    {
        yield return TimeYields.WaitOneFrameX;
        if (SmokeEffect.Pool.TryGetFromPool(out var smoke))
        {
            smoke.Component.transform.position = transform.position + Offset;
        }

        yield return TimeYields.WaitMilliseconds(GameTimer, 150);
        if (SmokeEffect.Pool.TryGetFromPool(out var smoke2))
        {
            smoke2.Component.transform.position = transform.position + Offset;
        }
    }
}
