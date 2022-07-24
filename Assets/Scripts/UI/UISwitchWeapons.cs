using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UISwitchWeapons : BaseUIObject
{
    public Transform HopeUI;
    public Transform PeaceUI;
    public Transform FocusUI;
    public TMP_Text WeaponName;

    private Vector3 _hopePosition;
    private Vector3 _peacePosition;
    private Vector3 _focusPosition;

    private Crosshair _crosshair;

    protected override void OnAwake()
    {
        base.OnAwake();
        _hopePosition = HopeUI.position;
        _peacePosition = PeaceUI.position;
        _focusPosition = FocusUI.position;
        _crosshair = SceneObject<Crosshair>.Instance();
    }

    private void OnEnable()
    {
        _crosshair.OnWeaponSwitch += OnWeaponSwitch;
    }

    private void OnDisable()
    {
        _crosshair.OnWeaponSwitch -= OnWeaponSwitch;
    }
    private void OnWeaponSwitch(WeaponDefinition obj)
    {
        WeaponName.text = $"<color=#{ColorUtility.ToHtmlStringRGB(obj.Color)}>{obj.Name}</color>";
        DefaultMachinery.AddBasicMachine(SwitchWeapon(obj.Name));
    }

    private IEnumerable<IEnumerable<Action>> SwitchWeapon(string weapon)
    {
        Vector3 hopePos = _hopePosition, peacePos = _peacePosition, focusPos = _focusPosition;
        switch (weapon)
        {
            case "Hope":
                hopePos = _hopePosition;
                peacePos = _peacePosition;
                focusPos = _focusPosition;
                break;
            case "Peace":
                hopePos = _focusPosition;
                peacePos = _hopePosition;
                focusPos = _peacePosition;
                break;
            case "Focus":
                hopePos = _peacePosition;
                peacePos = _focusPosition;
                focusPos = _hopePosition;
                break;
        }

        var hopeMove = HopeUI.GetAccessor()
            .Position
            .ToPosition(hopePos)
            .Over(0.15f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .UsingTimer(UITimer)
            .Build();

        var peaceMove = PeaceUI.GetAccessor()
            .Position
            .ToPosition(peacePos)
            .Over(0.15f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .UsingTimer(UITimer)
            .Build();

        var focusMove = FocusUI.GetAccessor()
            .Position
            .ToPosition(focusPos)
            .Over(0.15f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .UsingTimer(UITimer)
            .Build();

        yield return hopeMove.Combine(peaceMove).Combine(focusMove);
    }
}
