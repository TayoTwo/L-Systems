using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoad : MonoBehaviour
{

    public GameObject treeManagerPrefab;
    public VariableManager variableManager;
    TreeManager treeManager;

    void Start(){

        Debug.Log("OnEnable called");
        variableManager = FindObjectOfType<VariableManager>();
        treeManager = Instantiate(treeManagerPrefab,Vector3.zero,Quaternion.identity).GetComponent<TreeManager>();
        variableManager.SetValues(treeManager);

    }

}
