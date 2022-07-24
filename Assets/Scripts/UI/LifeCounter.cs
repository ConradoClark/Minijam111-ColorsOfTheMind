using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Objects;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class LifeCounter : MonoBehaviour
{
    public SpriteRenderer LifeSprite;
    public float MinSize;
    public float SizePerLife;

    private CharacterGettingHit _hit;

    private void Awake()
    {
        _hit = SceneObject<CharacterGettingHit>.Instance();
    }

    private void OnEnable()
    {
        _hit.OnHit += Hit_OnHit;
        AdjustSize();
    }

    private void OnDisable()
    {
        _hit.OnHit -= Hit_OnHit;
    }

    private void Hit_OnHit()
    {
        AdjustSize();
    }

    private void AdjustSize()
    {
        LifeSprite.size = new Vector2(MinSize + SizePerLife * _hit.CurrentHitPoints, LifeSprite.size.y);
    }
}
