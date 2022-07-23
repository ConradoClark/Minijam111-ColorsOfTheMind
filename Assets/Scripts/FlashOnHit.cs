using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class FlashOnHit : BaseGameObject
{
    public SpriteRenderer SpriteRenderer;
    public BulletHitDetector HitDetector;

    private string _uniqueId;
    private void OnEnable()
    {
        _uniqueId = $"{nameof(FlashOnHit)}_{GetInstanceID()}";
        HitDetector.OnHit += HitDetector_OnHit;
    }

    private void OnDisable()
    {
        HitDetector.OnHit -= HitDetector_OnHit;
    }

    private void HitDetector_OnHit(BulletContact obj)
    {
        DefaultMachinery.AddUniqueMachine(_uniqueId, UniqueMachine.UniqueMachineBehaviour.Replace, Flash(obj));
    }

    private IEnumerable<IEnumerable<Action>> Flash(BulletContact obj)
    {
        var originalColor = SpriteRenderer.material.GetColor("_Colorize");
        yield return SpriteRenderer.GetAccessor()
            .Material("_Colorize")
            .AsColor()
            .ToColor(obj.BulletColor)
            .Easing(EasingYields.EasingFunction.CubicEaseOut)
            .Over(0.05f)
            .UsingTimer(GameTimer)
            .Build();

        yield return SpriteRenderer.GetAccessor()
            .Material("_Colorize")
            .AsColor()
            .ToColor(originalColor)
            .Easing(EasingYields.EasingFunction.CubicEaseIn)
            .Over(0.1f)
            .UsingTimer(GameTimer)
            .Build();
    }

}
