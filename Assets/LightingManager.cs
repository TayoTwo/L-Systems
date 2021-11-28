using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{

    ReflectionProbe reflectionProbe;
    // Start is called before the first frame update
    void Awake()
    {

        reflectionProbe = GetComponent<ReflectionProbe>();
        
    }

    public void RerenderScene(){

        reflectionProbe.RenderProbe();

    }
}
