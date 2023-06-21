using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class InkExternalFunctions 
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("EnterFight", () =>
        {
            DialogueManager.GetInstance().EnterFight();
        });
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("EnterFight");
    }

    
}