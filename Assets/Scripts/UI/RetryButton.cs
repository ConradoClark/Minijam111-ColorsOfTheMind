using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Mixins;
using Licht.Unity.Objects;
using UnityEngine;

public class RetryButton : BaseUIObject
{
    public Color HoverColorize;
    public SpriteRenderer ButtonSprite;
    private ClickableObjectMixin _clickable;
    private Finalizer _finalizer;
    private Color _originalColorize;

    protected override void OnAwake()
    {
        base.OnAwake();
        var defaults = SceneObject<DefaultMouseInputs>.Instance();
        var uiCamera = SceneObject<UICamera>.Instance().Camera;
        _finalizer = SceneObject<Finalizer>.Instance();

        _originalColorize = ButtonSprite.material.GetColor("_Colorize");

        _clickable = new ClickableObjectMixinBuilder(this, defaults.MousePosInput, defaults.MouseClickInput)
            .WithCamera(uiCamera)
            .Build();
    }

    private void OnEnable()
    {
        DefaultMachinery.AddBasicMachine(_clickable.HandleHover(() => ButtonSprite.material.SetColor("_Colorize", HoverColorize),
            () => ButtonSprite.material.SetColor("_Colorize", _originalColorize)));
        DefaultMachinery.AddBasicMachine(HandleClick());
    }

    private IEnumerable<IEnumerable<Action>> HandleClick()
    {
        while (isActiveAndEnabled)
        {
            if (_clickable.WasClickedThisFrame())
            {
                _finalizer.LoadScene(); // reloads same scene
                break;
            }
            yield return TimeYields.WaitOneFrameX;
        }
    }

}
