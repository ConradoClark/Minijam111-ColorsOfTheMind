using System;
using Licht.Unity.Objects;
using UnityEngine;

[Serializable]
public struct WeaponDefinition
{
    public float CooldownInMs;
    public ScriptPrefab BulletPrefab;
    public string Name;
    public Color Color;
}
