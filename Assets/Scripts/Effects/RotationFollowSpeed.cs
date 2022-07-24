using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class RotationFollowSpeed : BaseGameObject
{
    public LichtPhysicsObject PhysicsObject;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.AngleAxis(Angle(PhysicsObject.LatestSpeed), Vector3.forward);
    }

   public static float Angle(Vector2 vector2)
 {
     if (vector2.x < 0)
     {
         return 360 - (Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg * -1) - 180;
     }
     else
     {
         return Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg - 180;
     }
 }
}
