using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Mixins;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoryText : BaseUIObject
{
    public string NextScene;
    public TMP_Text LoadingText;
    public TMP_Text TextComponent;
    public AudioSource TextSound;
    public float CharactersFrequencyInMs;

    private Color _originalColor;
    public Color DangerColor;

    private ZoomInOnTransform _zoomInEffect;
    private ClickDetectionMixin _clickDetection;
    private Finalizer _finalizer;

    protected override void OnAwake()
    {
        base.OnAwake();
        _zoomInEffect = SceneObject<ZoomInOnTransform>.Instance();
        var standards = SceneObject<DefaultMouseInputs>.Instance();

        _clickDetection =
            new ClickDetectionMixinBuilder(this, standards.MousePosInput, standards.MouseClickInput).Build();

        _finalizer = SceneObject<Finalizer>.Instance();
    }


    private void OnEnable()
    {
        _originalColor = TextComponent.color;
        DefaultMachinery.AddBasicMachine(ShowStory());
    }

    private IEnumerable<IEnumerable<Action>> ShowStory()
    {
        yield return ShowSentence("The mind is a curious thing.").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 3, breakCondition:()=> _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("It allows humans to achieve greatness.").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("It's clever, sensitive, intuitive.").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("Conscious, able, perceptive.").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("And, more often than not...").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 3, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        TextComponent.color = DangerColor;
        yield return ShowSentence("SELF-SABOTAGING").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 4, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        TextComponent.color = _originalColor;
        yield return ShowSentence("Let me guide you into a journey").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("A quick, daunting adventure").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("Into my little broken mind").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("Click to continue").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        while (!_clickDetection.WasClickedThisFrame(out _))
        {
            yield return TimeYields.WaitOneFrameX;
        }

        yield return _zoomInEffect.ZoomIn(UITimer).AsCoroutine();

        LoadingText.enabled = true;

        _finalizer.LoadScene(NextScene);
    }

    private IEnumerable<IEnumerable<Action>> ShowSentence(string sentence)
    {
        if (_clickDetection.WasClickedThisFrame(out _))
        {
            yield return TimeYields.WaitOneFrameX;
        }

        TextComponent.text = "";

        var index = 0;
        while (TextComponent.text != sentence)
        {
            if (TextSound != null)
            {
                TextSound.pitch = 0.95f + Random.value * 0.25f;
                TextSound.Play();
            }
            index++;
            TextComponent.text = sentence.Substring(0, index);
            yield return TimeYields.WaitMilliseconds(UITimer, CharactersFrequencyInMs, breakCondition: ()=> _clickDetection.WasClickedThisFrame(out _));

            if (_clickDetection.WasClickedThisFrame(out _))
            {
                TextComponent.text = sentence;
                yield return TimeYields.WaitOneFrameX;
            }
        }
    }
}
