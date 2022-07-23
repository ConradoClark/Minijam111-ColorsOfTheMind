using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Mixins;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class Crosshair : BaseGameObject
{
    public ScriptInput MousePosInput;
    public ScriptInput MouseClickInput;
    public LichtPhysicsObject CharacterPhysicsObject;

    public ScriptPrefab Test;
    public int WeaponCooldownInMs;
    public float DirectionRelativeToCharacter => transform.position.x >= CharacterPhysicsObject.transform.position.x ? 1 : -1;
    private ClickDetectionMixin _clickDetection;
    private CharacterAnimator _testChar;
    private BulletPool _bulletPool;

    protected override void OnAwake()
    {
        base.OnAwake();
        _bulletPool = SceneObject<BulletPoolManager>.Instance().GetEffect(Test);
        _testChar = FindObjectOfType<CharacterAnimator>();
        _clickDetection = new ClickDetectionMixinBuilder(this, MousePosInput, MouseClickInput).Build();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        DefaultMachinery.AddBasicMachine(FollowMouse());
        DefaultMachinery.AddBasicMachine(TestShoot());
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private IEnumerable<IEnumerable<Action>> TestShoot()
    {
        while (isActiveAndEnabled)
        {
            if (_clickDetection.IsPressed(out var pos)&& _bulletPool.TryGetFromPool(out var bullet))
            {
                bullet.Component.transform.position = new Vector3(_testChar.transform.position.x, _testChar.transform.position.y, 0);
                bullet.InitialSpeed = ((Vector2)(pos - _testChar.transform.position)).normalized;

                yield return TimeYields.WaitMilliseconds(GameTimer, WeaponCooldownInMs);
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
