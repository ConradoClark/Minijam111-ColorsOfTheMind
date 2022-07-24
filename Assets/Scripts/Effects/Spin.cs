using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class Spin : BaseGameObject
{
    public float Speed;
    private void Update()
    {
        transform.Rotate(Vector3.forward, (float) GameTimer.UpdatedTimeInMilliseconds * 0.1f * Speed, Space.Self);
    }
}
