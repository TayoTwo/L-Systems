using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIManager : MonoBehaviour
{

    public SequenceManager sequenceManager;
    public GameObject presetMenu;
    public GameObject treeMenu;

    [Header("UI Elements")]
    public TMP_Dropdown dropdown;
    public Toggle toggle;
    public Button backToPresetMenu;
    public Button next;
    public Button previous;
    public Button generate;
    public TMP_InputField length;
    public TMP_InputField width;
    public TMP_InputField angle;

    //Local variablies
    
    void Start() {

        length.text = sequenceManager.length.ToString();
        width.text = sequenceManager.width.ToString();
        angle.text = sequenceManager.angle.ToString();

        dropdown.onValueChanged.AddListener(OnPresetSelection);
        toggle.onValueChanged.AddListener(OnMeshModeSelction);

        backToPresetMenu.onClick.AddListener(OnBackToPresetMenu);
        next.onClick.AddListener(Next);
        previous.onClick.AddListener(Previous);
        generate.onClick.AddListener(OnGenButtonPress);

        length.onValueChanged.AddListener(OnLengthChanged);
        width.onValueChanged.AddListener(OnWidthChanged);
        angle.onValueChanged.AddListener(OnAngleChanged);

        OnPresetSelection(0);
        OnGenButtonPress();
        
    }

    public void Next(){

        sequenceManager.ChangeGeneration(1);
        treeMenu.GetComponentInChildren<TMP_Text>().text = "Generation: " + sequenceManager.currentGeneration.ToString() + "/" + sequenceManager.n.ToString();

    }

    public void Previous(){

        sequenceManager.ChangeGeneration(-1);
        treeMenu.GetComponentInChildren<TMP_Text>().text = "Generation: " + sequenceManager.currentGeneration.ToString() + "/" + sequenceManager.n.ToString();

    }

    public void OnBackToPresetMenu(){

        presetMenu.SetActive(true);
        treeMenu.SetActive(false);

    }

    public void OnGenButtonPress(){

        presetMenu.SetActive(false);
        treeMenu.SetActive(true);
        sequenceManager.NewTree();
        treeMenu.GetComponentInChildren<TMP_Text>().text = "Generation: 1/" + sequenceManager.presets[sequenceManager.presetIndex].n.ToString();
        
    }

    public void OnPresetSelection(int value){
        
        sequenceManager.presetIndex = value;
        angle.text = (sequenceManager.presets[value].angle.ToString());

    }

    public void OnMeshModeSelction(bool isOn){

        Debug.Log(isOn);
        sequenceManager.renderMesh = isOn;

    }

    public void OnLengthChanged(string l){

        Debug.Log(length);
        sequenceManager.length = float.Parse(l);

    }

    public void OnWidthChanged(string w){

        Debug.Log(width);
        sequenceManager.width = float.Parse(w);

    }

    public void OnAngleChanged(string a){

        Debug.Log(angle);
        sequenceManager.angle = float.Parse(a);

    }
}
