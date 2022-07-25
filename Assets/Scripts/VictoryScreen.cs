using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class VictoryScreen : MonoBehaviour
    {
        public AudioSource AudioSource;
        public void Activate()
        {
            gameObject.SetActive(true);
            AudioSource.Play();
        }
    }
}
