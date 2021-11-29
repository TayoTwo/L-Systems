using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VariableManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Toggle toggle;
    public GameObject treeManagerPrefab;
    public GameObject menu;
    public GameObject treeInfo;
    public GameObject next;
    public GameObject previous;
    public int presetIndex;
    public bool renderMode;
    public TreeManager treeManager;
    

    void Start() {

        if(FindObjectsOfType<VariableManager>().Length > 1){

            Destroy(gameObject);

        }

        DontDestroyOnLoad(gameObject);

        OnPresetSelection();
        RenderModeSelction();
        
    }

    public void SetValues(TreeManager tm){

        treeManager = tm;
        treeManager.presetIndex = presetIndex;
        treeManager.renderMode = renderMode;

    }

    public void Next(){

        treeManager.ChangeGeneration(1);
        treeInfo.GetComponentInChildren<TMP_Text>().text = "Generation: " + treeManager.currentGeneration.ToString();
        treeManager.GenerateTreeString();

    }

    public void Previous(){

        treeManager.ChangeGeneration(-1);
        treeInfo.GetComponentInChildren<TMP_Text>().text = "Generation: " + treeManager.currentGeneration.ToString();
        treeManager.GenerateTreeString();

    }

    public void OnBackToMenuPress(){

        menu.SetActive(true);
        treeInfo.SetActive(false);
        SceneManager.LoadScene(0);

    }

    public void OnGenButtonPress(){

        menu.SetActive(false);
        treeInfo.SetActive(true);
        treeInfo.GetComponentInChildren<TMP_Text>().text = "Generation: 1";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void OnPresetSelection(){
        
        presetIndex = dropdown.value;
        Debug.Log(presetIndex);

    }

    public void RenderModeSelction(){

        renderMode = toggle.isOn;
        Debug.Log(renderMode);

    }
}
