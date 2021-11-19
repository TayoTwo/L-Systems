using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContain : MonoBehaviour{
    
    public Camera camera;
    public float spawnerToSizeRatio;

    void Awake() {

        camera = GetComponent<Camera>();
        
    }

    public void UpdatePosition(Vector3 pos){

        Vector3 position = transform.position;
        pos.z = -1f;
        
        transform.position = pos;

    }

    public void UpdatePositionAvg(Vector3 f,Vector3 l){

        //Debug.Log(f + ":" + l);
        Vector3 pos = Vector3.Lerp(f,l,0.5f);
        pos.z = -1f;

        transform.position = pos;

    }
}
