using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Naninovel;

public class NaniNovelTest : MonoBehaviour
{
    public static NaniNovelTest Instance;
    public IScriptPlayer scriptPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    [Button]
    public void StartNaniNovelSequence()
    {
        if(scriptPlayer == null)
        {
            scriptPlayer = Engine.GetService<IScriptPlayer>();
        }
        scriptPlayer.PreloadAndPlayAsync("TestScript");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
