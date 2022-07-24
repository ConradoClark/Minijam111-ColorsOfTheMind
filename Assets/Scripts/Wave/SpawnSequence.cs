using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class SpawnSequence : BaseGameObject
{
    public SpawnParams[] Waves;
    private bool _enabled;

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(SpawnWaves());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> SpawnWaves()
    {
        while (_enabled)
        {
            var wave = 0;

            while (wave < Waves.Length)
            {
                var currentWave = Waves[wave];

                yield return TimeYields.WaitSeconds(GameTimer, currentWave.DelayInSeconds);

                foreach (var spawn in currentWave.Params)
                {
                    DefaultMachinery.AddBasicMachine(spawn.Spawn(GameTimer));
                }

                while (currentWave.Params.Any(c => !c.Expired))
                {
                    yield return TimeYields.WaitOneFrameX;
                }

                wave++;
            }

            // break for now;
            break;
        }
    }
}
