﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

namespace Assets.Scripts
{
    public class DeathScreen : MonoBehaviour
    {
        public AudioSource AudioSource;
        private SongAudioSource _song;

        public void Activate()
        {
            _song = SceneObject<SongAudioSource>.Instance();
            _song.AudioSource.Stop();
            gameObject.SetActive(true);
            AudioSource.Play();
        }
    }
}
