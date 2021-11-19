using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{

    TreeManager treeManager;
    public MeshRenderer renderer;

    void Awake() {

        treeManager = FindObjectOfType<TreeManager>();
        
    }

    public void Spawn (){

        

    }

}
