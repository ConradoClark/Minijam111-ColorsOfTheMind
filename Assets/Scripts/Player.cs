using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class Player : BaseGameObject
{
    public LichtPhysicsObject PhysicsObject;
    private LichtPlatformerJumpController _jumpController;
    private LichtPlatformerMoveController _platformerController;
    private LichtTopDownMoveController _topDownController;

    protected override void OnAwake()
    {
        base.OnAwake();
        _platformerController = GetComponent<LichtPlatformerMoveController>();
        _topDownController = GetComponent<LichtTopDownMoveController>();
        _jumpController = GetComponent<LichtPlatformerJumpController>();
    }

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

    private IEnumerable<IEnumerable<Action>> JumpAndDisableJump()
    {
        yield return _jumpController.ExecuteJump().AsCoroutine();
        _jumpController.enabled = false;
    }

    public void ChangeControlToTopDown()
    {
        _platformerController.enabled = false;
        _topDownController.enabled = true;
        DefaultMachinery.AddBasicMachine(JumpAndDisableJump());
    }
}
