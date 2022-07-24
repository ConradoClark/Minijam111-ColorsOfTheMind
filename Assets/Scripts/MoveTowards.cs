using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Objects;
using UnityEngine;

public class MoveTowards : BaseGameObject
{
    public Transform Target;

    public float Speed;

    // Update is called once per frame
    void Update()
    {
        Target = transform.parent;

        if (Target == null) return;

        transform.position = Vector2.MoveTowards(transform.position, Target.position,
            (float)GameTimer.UpdatedTimeInMilliseconds *
            Speed * 0.01f);
    }
}
