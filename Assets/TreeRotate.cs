using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRotate : MonoBehaviour
{

    public float rotSpeed;

    void LateUpdate() {

        if(transform.childCount == 0){

            Rotate();

        }
        
    }

    void Rotate(){

        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);

    }
}
