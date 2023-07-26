using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for Dialogue */

[System.Serializable]
public class Dialogue
{
    public string name;         // get name from inspector window

    [TextArea(3,10)]
    public string[] sentences;  // get sentences array from inspector window
}
