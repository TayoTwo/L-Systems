using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule{
    
    public char inital;
    public string result;
    public bool Match(char i){

        if(i == inital){

            return true;

        } else {

            return false;

        }

    }

    public string GetResult(){

        return result;

    }

}

public class TreeManager : MonoBehaviour
{
    
    public int presetIndex;
    public bool renderMode;
    public List<RulePreset> presets = new List<RulePreset>();

    [Header("Visual Variables")]
    public CameraContain cameraContain;
    public float timePerGen = 1f;
    public float length = 1f;
    public float width = 1f;

    [Header("Realtime Values")]
    [SerializeField]  
    public string currentTree;
    public int currentGeneration;
    public List<string> trees = new List<string>();

    [Header("Preset's Values")]
    public string presetName;
    public string axiom;
    public float angle = 25f;
    public int n;
    public List<Rule> productionRules = new List<Rule>();
    float time;
    string newTree;
    LightingManager lightingManager;
    Generator generator;

    // Start is called before the first frame update
    void Start(){

        lightingManager = GetComponent<LightingManager>();
        generator = GetComponent<Generator>();
        generator.cameraContain = cameraContain;
        presetName = presets[presetIndex].name;
        axiom = presets[presetIndex].axiom;
        angle = presets[presetIndex].angle;
        n = presets[presetIndex].n;
        productionRules = presets[presetIndex].rules;

        GenerateTreeString();

    }

    public void ChangeGeneration(int i){

        if(currentGeneration + i < n && currentGeneration + i > 0){

            currentGeneration += i;

        }


    }

    //Depending on if the 
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

    public void GenerateTreeString(){

        currentTree = axiom;

        //Increase the generation count and create a variable to store the new tree
        for(int i = 0;i < currentGeneration;i++){

            newTree = "";

            foreach(char c in currentTree){

                newTree += ApplyRule(c);

            }
            
            currentTree = newTree;

            Debug.Log(currentTree);

        }

        generator.GenerateTree(currentTree,renderMode,length,width,angle);
        lightingManager.RerenderScene();

    }


}
