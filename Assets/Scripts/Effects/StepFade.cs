using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class StepFade : BaseGameObject
{
    public SpriteRenderer SpriteRenderer;
    public float DurationInSeconds;
    public float Step;

    private void OnEnable()
    {
        SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 1);
        DefaultMachinery.AddBasicMachine(Fade());
    }

    private IEnumerable<IEnumerable<Action>> Fade()
    {
        yield return SpriteRenderer.GetAccessor()
            .Color
            .A
            .SetTarget(0f)
            .WithStep(Step)
            .Over(DurationInSeconds)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .UsingTimer(GameTimer)
            .Build();
    }

}
