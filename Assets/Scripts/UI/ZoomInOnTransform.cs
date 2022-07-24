using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Time;
using Licht.Unity.Builders;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ZoomInOnTransform : MonoBehaviour
{
    public float TimeInSeconds;
    public Transform Transform;
    private PixelPerfectCamera _pixelPerfectCamera;
    private void Awake()
    {
        _pixelPerfectCamera = SceneObject<PixelPerfectCamera>.Instance();
    }

    public IEnumerable<IEnumerable<Action>> ZoomIn(ITimer timer)
    {
        var cam = Camera.main;
        _pixelPerfectCamera.enabled = false;

        var zoomIn = new LerpBuilder(f => cam.orthographicSize = f, () => cam.orthographicSize)
            .SetTarget(0.1f)
            .Over(TimeInSeconds)
            .Easing(EasingYields.EasingFunction.BounceEaseOut)
            .UsingTimer(timer)
            .Build();
        
        var pos = cam.transform.GetAccessor()
            .Position
            .ToPosition(new Vector3(Transform.position.x, Transform.position.y, cam.transform.position.z))
            .Over(TimeInSeconds)
            .Easing(EasingYields.EasingFunction.BounceEaseOut)
            .UsingTimer(timer)
            .Build();

        yield return zoomIn.Combine(pos);
    }
}
