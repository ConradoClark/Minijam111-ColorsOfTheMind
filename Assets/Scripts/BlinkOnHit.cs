using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class BlinkOnHit : BaseGameObject
{
    public CharacterGettingHit GettingHit;
    public SpriteRenderer SpriteRenderer;
    public AudioSource GettingHitSFX;

    private void OnEnable()
    {
        GettingHit.OnHit += GettingHit_OnHit;
    }

    private void GettingHit_OnHit()
    {
        if (GettingHitSFX != null)
        {
            GettingHitSFX.Play();
        }
        DefaultMachinery.AddBasicMachine(Blink());
    }

    private void OnDisable()
    {
        GettingHit.OnHit -= GettingHit_OnHit;
    }

    private IEnumerable<IEnumerable<Action>> Blink()
    {
        for (var i = 0; i < 20; i++)
        {
            SpriteRenderer.enabled = !SpriteRenderer.enabled;
            yield return TimeYields.WaitSeconds(GameTimer, GettingHit.HitCooldownInSeconds * 0.05);
        }

        SpriteRenderer.enabled = true;
    }
}
