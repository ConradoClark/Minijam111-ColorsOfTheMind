using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Mixins;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndingText : BaseUIObject
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
        yield return ShowSentence("Thank you for playing my little game").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 3, breakCondition:()=> _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("No ghosts were harmed during production").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("We do not understand ourselves completely").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("Amidst the eternal turmoil inside our minds").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("May we find solace in our small goals").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 3, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("And a little peace in our breakthroughs").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("For we are but a fragment").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        yield return ShowSentence("Scattered across the universe").AsCoroutine();
        yield return TimeYields.WaitSeconds(UITimer, 2, breakCondition: () => _clickDetection.WasClickedThisFrame(out _));

        TextComponent.color = DangerColor;
        yield return ShowSentence("Infinitesimal, but beautiful.").AsCoroutine();
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
