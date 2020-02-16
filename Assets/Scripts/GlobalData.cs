using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{

    public int seed = 0;
    public int level = 0;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
