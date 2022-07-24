using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;

public class PeaceBulletContact : BulletContact
{
    protected override IEnumerable<IEnumerable<Action>> Move()
    {
        yield return TimeYields.WaitOneFrameX;
        if (InitialSpeed.magnitude == 0) yield break;

        yield return PhysicsObject.GetSpeedAccessor(InitialSpeed * BulletSpeed * 0.5f)
            .ToSpeed(Vector2.zero)
            .Over(0.5f)
            .UsingTimer(GameTimer)
            .BreakIf(()=> IsEffectOver)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Build();

        yield return PhysicsObject.GetSpeedAccessor()
            .ToSpeed(InitialSpeed * BulletSpeed * -1)
            .Over(1f)
            .UsingTimer(GameTimer)
            .BreakIf(() => IsEffectOver)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Build();

        while (!IsEffectOver)
        {
            PhysicsObject.ApplySpeed(InitialSpeed * BulletSpeed * -1);
            yield return TimeYields.WaitOneFrameX;
        }
    }
}
