using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    
    public List<LSystem> presets = new List<LSystem>();

    [Header("Visuals")]
    public bool renderMesh;
    public float length = 1.25f;
    public float width = 0.3f;

    [Header("Realtime Values")]
    [SerializeField]  
    public string currentSequence;
    public int currentGeneration;

    [Header("Current Sequence")]
    public int presetIndex;
    public string presetName;
    public string axiom;
    public float angle;
    public int n;
    public List<Rule> productionRules = new List<Rule>();

    //Local Variables
    float time;
    string newSequence;
    LightingManager lightingManager;
    CameraManager cameraManager;
    Generator generator;

    //Assign the correct variables 
    void Awake(){

        lightingManager = GetComponent<LightingManager>();
        generator = GetComponent<Generator>();
        cameraManager = GetComponentInChildren<CameraManager>();

    }

    public void NewTree() {

        currentGeneration = 1;
        currentSequence = "";

        presetName = presets[presetIndex].name;
        axiom = presets[presetIndex].axiom;
        n = presets[presetIndex].n;
        productionRules = presets[presetIndex].rules;
        
        GenerateSequence();
        
    }

    //Increase or decrease the generation count
    public void ChangeGeneration(int i){

        if(currentGeneration + i < n + 1 && currentGeneration + i > 0){

            currentGeneration += i;
            GenerateSequence();

        }


    }

    //Look for the rule that matches the current character
    public string ApplyRule(char c){

        bool matchFound = false;
        int index = -1;

        foreach(Rule rule in productionRules){

            if(rule.Match(c)){

                matchFound = true;
                index = productionRules.IndexOf(rule);
                        
            }

        }

        if(matchFound){

            return productionRules[index].GetResult();
            
        } else {

            return c.ToString();

        }

    }

    //Gennerate an L-System string
    public void GenerateSequence(){

        currentSequence = axiom;

        //Based on the number 
        for(int i = 0;i < currentGeneration;i++){

            newSequence = "";

            //Loop through every character in the current tree and generate a new tree after apply production rules
            foreach(char c in currentSequence){

                newSequence += ApplyRule(c);

            }
            
            currentSequence = newSequence;

            Debug.Log(currentSequence);

        }

        //Tell the generator to generate the tree visually
        generator.VisualizeSequence(currentSequence,renderMesh,length,width,angle);
        //Update the camera's position as redo the scene's lighting
        cameraManager.UpdateOffset(renderMesh,generator.tallestPoint);
        cameraManager.UpdatePositionAvg(Vector3.zero, Vector3.up * generator.tallestPoint);

        lightingManager.RerenderScene();

    }


}
