using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterAnimator : BaseGameObject
{
    public Crosshair Crosshair;
    public Animator Animator;
    public SpriteRenderer SpriteRenderer;
    public ScriptIdentifier Grounded;
    public LichtPhysicsObject PhysicsObject;
    private bool _hasJumpedRecently;

    public AudioSource JumpSound;
    public AudioSource StepSound;

    private void OnEnable()
    {
        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>( LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStartMoving, OnStartMoving);
        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStopMoving, OnFinishMoving);
        this.ObserveEvent<LichtPlatformerJumpController.LichtPlatformerJumpEvents, LichtPlatformerJumpController.LichtPlatformerJumpEventArgs>(LichtPlatformerJumpController.LichtPlatformerJumpEvents.OnJumpStart, OnJumpStart);
        DefaultMachinery.AddBasicMachine(HandleFalling());
        DefaultMachinery.AddBasicMachine(HandleTurning());
    }

    private void OnJumpStart(LichtPlatformerJumpController.LichtPlatformerJumpEventArgs obj)
    {
        if (JumpSound != null)
        {
            JumpSound.pitch = 0.95f + Random.value * 0.15f;
            JumpSound.Play();
        }
        Animator.SetBool("IsJumping", true);
        _hasJumpedRecently = true;
        DefaultMachinery.AddBasicMachine(HandleJumpDelay());
    }

    private IEnumerable<IEnumerable<Action>> HandleJumpDelay()
    {
        yield return TimeYields.WaitMilliseconds(GameTimer, 100);
        _hasJumpedRecently = false;
    }

    private IEnumerable<IEnumerable<Action>> HandleFalling()
    {
        while (isActiveAndEnabled)
        {
            var isJumping = Animator.GetBool("IsJumping");
            var isFalling = Animator.GetBool("IsFalling");

            if (!_hasJumpedRecently && !isJumping && !isFalling && !PhysicsObject.GetPhysicsTrigger(Grounded))
            {
                Animator.SetBool("IsJumping", true);
                Animator.SetBool("IsFalling", true);
                yield return TimeYields.WaitOneFrameX;
                continue;
            }

            if (!isJumping)
            {
                yield return TimeYields.WaitOneFrameX;
                continue;
            }

            if (!_hasJumpedRecently && PhysicsObject.GetPhysicsTrigger(Grounded))
            {
                Animator.SetBool("IsJumping",false);
                Animator.SetBool("IsFalling", false);

                if (StepSound != null)
                {
                    StepSound.pitch = 0.95f + Random.value * 0.05f;
                    StepSound.Play();
                }
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }

    private void OnDisable()
    {
        this.StopObservingEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStartMoving, OnStartMoving);
        this.StopObservingEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStopMoving, OnFinishMoving);
        this.StopObservingEvent<LichtPlatformerJumpController.LichtPlatformerJumpEvents, LichtPlatformerJumpController.LichtPlatformerJumpEventArgs>(LichtPlatformerJumpController.LichtPlatformerJumpEvents.OnJumpStart, OnJumpStart);
    }

    private void OnStartMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        Animator.SetBool("IsWalking", true);
    }

    private void OnFinishMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        Animator.SetBool("IsWalking", false);
    }

    private IEnumerable<IEnumerable<Action>> HandleTurning()
    {
        while (isActiveAndEnabled)
        {
            SpriteRenderer.flipX = Crosshair.DirectionRelativeToCharacter < 0;
            yield return TimeYields.WaitOneFrameX;
        }
    }

}
