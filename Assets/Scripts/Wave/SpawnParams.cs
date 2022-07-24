using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct SpawnParams
{
    public SpawnDefinition[] Params;
    public float DelayInSeconds;
}
