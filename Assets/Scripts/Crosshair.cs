using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Mixins;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Crosshair : BaseGameObject
{
    public ScriptInput MousePosInput;
    public ScriptInput MouseClickInput;
    public ScriptInput ChangeWeaponInput;
    public LichtPhysicsObject CharacterPhysicsObject;

    public WeaponDefinition[] Weapons;
    public WeaponDefinition CurrentWeapon { get; private set; }
    public float WeaponSwitchCooldownInMs;

    public event Action<WeaponDefinition> OnWeaponSwitch;
    public AudioSource AudioSource;

    public float DirectionRelativeToCharacter => transform.position.x >= CharacterPhysicsObject.transform.position.x ? 1 : -1;
    private ClickDetectionMixin _clickDetection;
    private CharacterAnimator _character;
    private Dictionary<WeaponDefinition, BulletPool> _bulletPool;

    private InputAction _switchWeaponAction;
    private int _currentWeaponIndex;

    protected override void OnAwake()
    {
        base.OnAwake();
        _bulletPool = new Dictionary<WeaponDefinition, BulletPool>();
        var bulletPoolManager = SceneObject<BulletPoolManager>.Instance();
        var playerInput = PlayerInput.GetPlayerByIndex(0);

        _switchWeaponAction = playerInput.actions[ChangeWeaponInput.ActionName];

        foreach (var def in Weapons)
        {
            _bulletPool[def] = bulletPoolManager.GetEffect(def.BulletPrefab);
        }

        _character = FindObjectOfType<CharacterAnimator>();
        _clickDetection = new ClickDetectionMixinBuilder(this, MousePosInput, MouseClickInput).Build();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        CurrentWeapon = Weapons.FirstOrDefault();
        DefaultMachinery.AddBasicMachine(FollowMouse());
        DefaultMachinery.AddBasicMachine(HandleShooting());
        DefaultMachinery.AddBasicMachine(HandleWeaponSwitch());
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private IEnumerable<IEnumerable<Action>> HandleShooting()
    {
        while (isActiveAndEnabled)
        {
            if (_clickDetection.IsPressed(out var pos) && _bulletPool[CurrentWeapon].TryGetFromPool(out var bullet))
            {
                if (AudioSource != null && CurrentWeapon.AudioClip != null)
                {
                    AudioSource.pitch = 0.95f + Random.value * 0.1f;
                    AudioSource.PlayOneShot(CurrentWeapon.AudioClip);
                }

                bullet.InitialSpeed = ((Vector2)(pos - _character.transform.position)).normalized;
                bullet.Component.transform.position = new Vector3(_character.transform.position.x, _character.transform.position.y, 0)
                                                      + (Vector3)bullet.InitialSpeed * 0.1f;


                yield return TimeYields.WaitMilliseconds(GameTimer, CurrentWeapon.CooldownInMs);
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }

    private IEnumerable<IEnumerable<Action>> HandleWeaponSwitch()
    {
        while (isActiveAndEnabled)
        {
            if (_switchWeaponAction.WasPerformedThisFrame())
            {
                _currentWeaponIndex++;
                if (_currentWeaponIndex >= Weapons.Length) _currentWeaponIndex = 0;
                CurrentWeapon = Weapons[_currentWeaponIndex];

                OnWeaponSwitch?.Invoke(CurrentWeapon);

                yield return TimeYields.WaitMilliseconds(GameTimer, WeaponSwitchCooldownInMs);

            }

            yield return TimeYields.WaitOneFrameX;
        }
    }

    private IEnumerable<IEnumerable<Action>> FollowMouse()
    {
        while (isActiveAndEnabled)
        {
            var mousePos = _clickDetection.GetMousePosition();
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            yield return TimeYields.WaitOneFrameX;
        }
    }


}
