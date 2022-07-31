using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Pooling;
using UnityEngine;

namespace Assets.Scripts.Wave
{
    public abstract class ObjectReplacer : EffectPoolable
    {
        public Transform ObjectFrom;
        public Transform ObjectTo;

        protected abstract IEnumerable<IEnumerable<Action>> PerformReplaceEffect();

        private IEnumerable<IEnumerable<Action>> Replace()
        {
            yield return PerformReplaceEffect().AsCoroutine();
            ObjectFrom.gameObject.SetActive(false);
            ObjectTo.gameObject.SetActive(true);
            EndEffect();
        }

        public override void OnActivation()
        {
            DefaultMachinery.AddBasicMachine(Replace());
        }
    }
}
