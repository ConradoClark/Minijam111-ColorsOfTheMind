using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Pooling;
using UnityEngine;

namespace Assets.Scripts.Paint
{
    [DefaultExecutionOrder(-2000)]
    public class LightweightPaintPoolManager : CustomPrefabManager<LightweightPaintPool, LightweightPaint>
    {
    }
}
