using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Time;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

[Serializable]
public class SpawnDefinition
{
    public ScriptPrefab SpawnSpiral;
    public ScriptPrefab ObjectToSpawn;
    public Vector3 Position;
    public float OffsetInSeconds;
    public IPoolableComponent SpawnedObject;

    public bool Expired { get; private set; }

    public IEnumerable<IEnumerable<Action>> Spawn(ITimer timer)
    {
        Expired = false;
        yield return TimeYields.WaitSeconds(timer, OffsetInSeconds);

        if (SpawnSpiral != null && SpawnSpiral.Pool.TryGetFromPool(out var spiral))
        {
            spiral.Component.transform.position = Position;
            while (spiral.IsActive)
            {
                yield return TimeYields.WaitOneFrameX;
            }
        }

        if (ObjectToSpawn.Pool.TryGetFromPool(out var obj))
        {
            SpawnedObject = obj;
            SpawnedObject.Component.transform.position = Position;

            yield return HandleExpiration().AsCoroutine();
        }
    }

    private IEnumerable<IEnumerable<Action>> HandleExpiration()
    {
        while (SpawnedObject.IsActive)
        {
            yield return TimeYields.WaitOneFrameX;
        }

        Expired = true;
    }
}
