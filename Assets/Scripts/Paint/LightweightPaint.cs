using Licht.Unity.Pooling;
using UnityEngine;

namespace Assets.Scripts.Paint
{
    public class LightweightPaint : EffectPoolable
    {
        public SpriteRenderer SpriteRenderer;
        public override void OnActivation()
        {
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 1);
            _checkTime = true;
            _elapsedTime = 0;

            //DefaultMachinery.AddBasicMachine(Fade());
        }

        private bool _checkTime;
        private double _elapsedTime;

        private void Update()
        {
            if (!_checkTime) return;

            _elapsedTime+= GameTimer.UpdatedTimeInMilliseconds;

            if (_elapsedTime > 30 * 1000)
            {
                _checkTime = false;
                EndEffect();
            }
        }

        //private IEnumerable<IEnumerable<Action>> Fade()
        //{
        //    yield return TimeYields.WaitSeconds(GameTimer, 30);

        //    yield return SpriteRenderer.GetAccessor()
        //        .Color
        //        .A
        //        .SetTarget(0)
        //        .WithStep(0.1f)
        //        .Over(1f)
        //        .UsingTimer(GameTimer)
        //        .Build();

        //}
    }
}
