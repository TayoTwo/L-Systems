using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContain : MonoBehaviour{
    
    public Camera cam;
    public float smoothTime = 0.3f;
    public float lookHeight;
    public float heightMult;
    public Vector3 offset;
    Vector3 targetPos;
    Vector3 velocity;

    void Awake() {

        cam = GetComponentInChildren<Camera>();
        
    }

    void LateUpdate(){

        transform.position = Vector3.SmoothDamp(transform.position,targetPos + offset,ref velocity,smoothTime);
        cam.transform.LookAt(targetPos);

    }

    public void UpdateOffset(bool lines){

        float size =  targetPos.y * heightMult;

        if(lines){

            offset = new Vector3(0,0,-1) * size;

        } else {

            offset = new Vector3(-1,0,-1) * size;

        }
        

    }

    public void UpdatePosition(Vector3 pos){

        targetPos = pos;
        
        //transform.position = new Vector3(0,targetPos.y,0);

    }

    public void UpdatePositionAvg(Vector3 f,Vector3 l){

        targetPos = Vector3.Lerp(f,l,lookHeight);
        
        //transform.position = new Vector3(0,targetPos.y,0);

    }
}
