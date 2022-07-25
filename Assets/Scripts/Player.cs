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


    private void Update()
    {
        // physics fail-safe
        if (Vector2.Distance(transform.position, Camera.main.transform.position) > 3f)
        {
            transform.position = new Vector3(-0.0271565467f, -0.255657285f, 0);
        }
    }

    private IEnumerable<IEnumerable<Action>> EnablePhysicsObject()
    {
        yield return TimeYields.WaitSeconds(GameTimer, 1.5);
        PhysicsObject.enabled = true;
    }
}
