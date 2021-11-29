using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour{
    
    public Camera cam;
    public float smoothTime = 0.3f;
    public float lookHeight;
    public float lineModeMult;
    public float meshModeMult;
    public float camSizeMult;

    public Vector3 offset;
    public SequenceManager sequenceManager;
    Vector3 targetPos;
    Vector3 velocity;
    float x;

    void Awake() {

        cam = GetComponentInChildren<Camera>();
        
    }

    void LateUpdate(){

        transform.position = Vector3.SmoothDamp(transform.position,targetPos + offset,ref velocity,smoothTime);

        cam.transform.LookAt(targetPos);

    }

    public void UpdateOffset(bool renderMesh,float tallestPoint){

        if(!renderMesh){

            cam.orthographic = false;

            x = lineModeMult * (tallestPoint);
            offset = new Vector3(0,0,-1) * x;

        } else {

            cam.orthographic = true;

            cam.orthographicSize = camSizeMult * tallestPoint;
            x = meshModeMult * (tallestPoint);
            offset = new Vector3(-1,1,-1) * x;

            transform.position = targetPos + offset;

        }


    }

    public void UpdatePosition(Vector3 pos){

        targetPos = pos;
        
        //transform.position = new Vector3(0,targetPos.y,0);

    }

    public void UpdatePositionAvg(Vector3 f,Vector3 l){

        targetPos = Vector3.Lerp(f,l,lookHeight);
        transform.position = new Vector3(transform.position.x,targetPos.y + offset.y,transform.position.z);
        
        //transform.position = new Vector3(0,targetPos.y,0);

    }
}
