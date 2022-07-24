using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class Player : BaseGameObject
{
    public LichtPhysicsObject PhysicsObject;
    private void OnEnable()
    {
        PhysicsObject.enabled = false;
        DefaultMachinery.AddBasicMachine(EnablePhysicsObject());
    }

    private IEnumerable<IEnumerable<Action>> EnablePhysicsObject()
    {
        yield return TimeYields.WaitSeconds(GameTimer, 1.5);
        PhysicsObject.enabled = true;
    }
}
