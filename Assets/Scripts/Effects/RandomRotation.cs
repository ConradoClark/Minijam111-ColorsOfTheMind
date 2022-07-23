using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private void OnEnable()
    {
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.forward);
    }
}
