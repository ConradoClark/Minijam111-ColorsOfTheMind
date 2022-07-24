using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class ChangeSong : MonoBehaviour
{
    public AudioClip Clip;

    private SongAudioSource _songAudioSource;

    private void Awake()
    {
        _songAudioSource = SceneObject<SongAudioSource>.Instance();
    }

    private void OnEnable()
    {
        _songAudioSource.ChangeClip(Clip);
    }
}
