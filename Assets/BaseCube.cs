using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCube : MonoBehaviour
{

    public static BaseCube by;

    void Awake()
    {
        by = this;
    }
    
}
