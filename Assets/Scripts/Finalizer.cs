using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine.SceneManagement;

public class Finalizer : BaseGameObject
{
    public ScriptBasicMachinery PostUpdate;
    public void LoadScene(string sceneName = null)
    {
        PostUpdate.Machinery.FinalizeWith(() => { });
        DefaultMachinery.FinalizeWith(() =>
        {
            if (sceneName == null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        });
    }
}
