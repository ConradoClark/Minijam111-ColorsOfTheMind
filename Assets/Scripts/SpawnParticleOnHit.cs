using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnParticleOnHit : BaseGameObject
{
    public Vector2 Range;
    public BulletHitDetector HitDetector;
    public ScriptPrefab Effect;

    private void OnEnable()
    {
        HitDetector.OnHit += HitDetector_OnHit;
    }

    private void OnDisable()
    {
        HitDetector.OnHit -= HitDetector_OnHit;
    }

    private void HitDetector_OnHit(BulletContact obj)
    {
        if (Effect.Pool.TryGetFromPool(out var effect))
        {
            effect.Component.transform.position = transform.position + (Vector3) (Random.insideUnitCircle * Range);
        }
    }
}
