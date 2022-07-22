using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Unity.CharacterControllers;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator Animator;
    public SpriteRenderer SpriteRenderer;

    private void OnEnable()
    {
        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>( LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStartMoving, OnStartMoving);
        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStopMoving, OnFinishMoving);
    }
    private void OnDisable()
    {
        this.StopObservingEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStartMoving, OnStartMoving);
        this.StopObservingEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents, LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStopMoving, OnFinishMoving);
    }

    private void OnStartMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        Animator.SetBool("IsWalking", true);
        AdjustTurn(obj);
    }

    private void OnFinishMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        Animator.SetBool("IsWalking", false);
        AdjustTurn(obj);
    }

    private void AdjustTurn(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        SpriteRenderer.flipX = obj.Direction < 0;
    }

}
