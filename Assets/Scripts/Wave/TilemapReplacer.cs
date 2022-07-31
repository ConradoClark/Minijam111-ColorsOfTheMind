using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics.Forces;

namespace Assets.Scripts.Wave
{
    public class TilemapReplacer : ObjectReplacer
    {
        private CinemachineImpulseSource _impulseSource;

        protected override void OnAwake()
        {
            base.OnAwake();
            var @params = SceneObject<TilemapReplacerParams>.Instance();
            ObjectFrom = @params.From;
            ObjectTo = @params.To;
            _impulseSource = @params.CameraImpulseSource;
        }

        protected override IEnumerable<IEnumerable<Action>> PerformReplaceEffect()
        {
            _impulseSource.GenerateImpulse(1f);
            yield return TimeYields.WaitOneFrameX;
        }
    }
}
