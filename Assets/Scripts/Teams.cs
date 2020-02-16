using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teams
{

    public static readonly Dictionary<string, Color> teams = new Dictionary<string, Color>()
    {
        {"Player", Color.blue },
        {"AlphaPack", Color.red },
        {"BetaPack", Color.green },
        {"OmegaPack", Color.yellow }
    };

}
