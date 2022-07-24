using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextFlicker : BaseUIObject
{
    public float RotationVariation;
    public float SizeVariation;
    public float FrequencyInMs;
    public TMP_Text TextComponent;
    private float _originalTextSize;

    private void OnEnable()
    {
        _originalTextSize = TextComponent.fontSize;
        DefaultMachinery.AddBasicMachine(Flicker());
    }

    private IEnumerable<IEnumerable<Action>> Flicker()
    {
        while (isActiveAndEnabled)
        {
            TextComponent.fontSize = _originalTextSize + Random.insideUnitCircle.x * SizeVariation;
            transform.rotation = Quaternion.Euler(0, 0, Random.insideUnitCircle.x * RotationVariation);
            yield return TimeYields.WaitMilliseconds(UITimer, FrequencyInMs);
        }
    }
}
