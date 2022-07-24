using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Objects;
using UnityEngine;

public class SongAudioSource : BaseUIObject
{
    public AudioSource AudioSource;

    public void ChangeClip(AudioClip clip)
    {
        DefaultMachinery.AddBasicMachine(CrossFade(clip));
    }

    private IEnumerable<IEnumerable<Action>> CrossFade(AudioClip clip)
    {
        yield return new LerpBuilder(f => AudioSource.volume = f, () => AudioSource.volume)
            .SetTarget(0)
            .Over(1)
            .UsingTimer(UITimer)
            .Build();

        AudioSource.clip = clip;

        yield return TimeYields.WaitSeconds(UITimer, 0.5f);
        AudioSource.Play();

        yield return new LerpBuilder(f => AudioSource.volume = f, () => AudioSource.volume)
            .SetTarget(1)
            .Over(0.15f)
            .UsingTimer(UITimer)
            .Build();
    }
}
