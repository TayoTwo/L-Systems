using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VariableManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject treeManagerPrefab;
    public GameObject menu;
    public GameObject genInfo;
    public int presetIndex;
    GameObject treeManager;
    
    void Start() {

        DontDestroyOnLoad(gameObject);
        
    }

    public void OnButtonPress(){

        menu.SetActive(false);
        genInfo.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        treeManager = (GameObject)Instantiate(treeManager,Vector3.zero,Quaternion.identity);

        treeManager.GetComponent<TreeManager>().presetIndex = presetIndex;
        
    }

    public void OnPresetSelection(){
        
        presetIndex = dropdown.value;
        Debug.Log(presetIndex);

    }
}
