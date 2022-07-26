using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;

public class BulletContact : EffectPoolable
{
    public bool IsSubSpawn { get; set; }
    public int BulletDamage;
    public Color BulletColor;
    public Material PaintMaterial;
    public ScriptPrefab SplatterEffect;
    public ScriptPrefab PaintEffect;
    public LichtPhysicsObject PhysicsObject;
    public ScriptIdentifier Contact;
    public ScriptIdentifier Gravity;
    public Vector2 InitialSpeed;
    public Vector2 BulletSpeed;
    protected PaintPool PaintPool;
    protected LichtPhysics Physics;

    protected override void OnAwake()
    {
        base.OnAwake();
        PhysicsObject.AddCustomObject(this);
        PaintPool = SceneObject<PaintPoolManager>.Instance().GetEffect(PaintEffect);
        Physics = this.GetLichtPhysics();
    }

    protected virtual IEnumerable<IEnumerable<Action>> Move()
    {
        yield return TimeYields.WaitOneFrameX;
        if (InitialSpeed.magnitude == 0) yield break;

        yield return PhysicsObject.GetSpeedAccessor(InitialSpeed * BulletSpeed)
            .ToSpeed(Vector2.zero)
            .Over(1f)
            .UsingTimer(GameTimer)
            .BreakIf(()=> IsEffectOver)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();
    }

    protected virtual IEnumerable<IEnumerable<Action>> HandleContact()
    {
        // resets gravity accel for object
        Physics.BlockCustomPhysicsForceForObject(this, PhysicsObject, Gravity.Name);
        yield return TimeYields.WaitMilliseconds(GameTimer, 50);

        Physics.UnblockCustomPhysicsForceForObject(this, PhysicsObject, Gravity.Name);

        while (isActiveAndEnabled)
        {
            if (PhysicsObject.GetPhysicsTrigger(Contact))
            {
                if (SplatterEffect.Pool.TryGetFromPool(out var splatter))
                {
                    splatter.Component.transform.position = transform.position;
                }

                if (PaintPool.TryGetManyFromPool(5, out var paints))
                {
                    for (var index = 0; index < paints.Length; index++)
                    {
                        var paint = paints[index];
                        paint.Component.transform.position =
                            transform.position;

                        paint.SpriteRenderer.material = PaintMaterial;

                        paint.InitialSpeed = new Vector2(0,0.2f) + DegreeToVector2(index * (360f / paints.Length)) * 0.25f;
                    }
                }

                EndEffect();
                break;
            }
            yield return TimeYields.WaitMilliseconds(GameTimer, 10);
        }
    }

    public override void OnActivation()
    {
        DefaultMachinery.AddBasicMachine(HandleContact());
        DefaultMachinery.AddBasicMachine(Move());
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));   
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
