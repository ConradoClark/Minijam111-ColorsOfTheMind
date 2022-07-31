using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.Forces;
using UnityEngine;

namespace Assets.Scripts.Wave
{
    public class DepressionGravityChange : TilemapReplacer
    {
        private Gravity _gravity;
        private Player _player;
        private LichtPhysics _physics;

        protected override void OnAwake()
        {
            base.OnAwake();
            _gravity = SceneObject<Gravity>.Instance();
            _player = SceneObject<Player>.Instance();
            _physics = this.GetLichtPhysics();
        }

        protected override IEnumerable<IEnumerable<Action>> PerformReplaceEffect()
        {
            yield return base.PerformReplaceEffect().AsCoroutine();
            _gravity.Direction = Vector2.up;

            yield return TimeYields.WaitMilliseconds(GameTimer, 400);

            _physics.BlockCustomPhysicsForceForObject(this, _player.PhysicsObject, _gravity.Key);
            _player.ChangeControlToTopDown();
        }
    }
}
