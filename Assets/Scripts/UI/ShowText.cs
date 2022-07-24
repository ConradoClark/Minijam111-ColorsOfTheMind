using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using TMPro;
using UnityEngine;

public class ShowText : BaseGameObject
{
    public TMP_Text TextComponent;
    public DurationPoolable DurationPoolable;

    private void OnEnable()
    {
        transform.localScale = new Vector3(1f, 0.1f, 1f);
        DefaultMachinery.AddBasicMachine(TextEffect());
    }

    private IEnumerable<IEnumerable<Action>> TextEffect()
    {
        yield return transform.GetAccessor()
            .LocalScale
            .Y
            .SetTarget(1f)
            .Over(0.25f)
            .WithStep(0.05f)
            .Easing(EasingYields.EasingFunction.Linear)
            .UsingTimer(GameTimer)
            .Build();

        var remaining = DurationPoolable.DurationInSeconds - .5f;

        yield return TimeYields.WaitSeconds(GameTimer, remaining);

        yield return transform.GetAccessor()
            .LocalScale
            .Y
            .SetTarget(0.1f)
            .Over(0.25f)
            .WithStep(0.05f)
            .Easing(EasingYields.EasingFunction.Linear)
            .UsingTimer(GameTimer)
            .Build();
    }
}
