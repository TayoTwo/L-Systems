using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule{
    
    public char inital;
    public string result;

    public bool Match(char c){

        if(c == inital){

            return true;

        } else {

            return false;

        }

    }

    public string GetResult(){

        return result;

    }

}

[System.Serializable]
public struct NodePos {
    public Vector3 position;
    public Quaternion rotation;

}

public class TreeManager : MonoBehaviour
{
    
    public int preset;
    public List<RulePreset> presets = new List<RulePreset>();

    [Header("Visual Variables")]
    public CameraContain cameraContain;
    [SerializeField]   
    public float timePerGen = 1f;
    public float length = 1f;


    [Header("Realtime Values")]
    [SerializeField]  
    public Stack<NodePos> nodeStack = new Stack<NodePos>();
    public string currentTree;
    public int currentGeneration = 0;

    [Header("Objects")]
    public Transform spawner;
    public GameObject nodeObj;
    public GameObject tree;
    public GameObject line;
    NodePos nodePos = new NodePos();

    [Header("Preset's Values")]
    public string presetName;
    public string axiom;
    public float angle = 25f;
    public int n;
    public Rule[] productionRules = new Rule[1];

    string newTree;
    GameObject newTreeObj;
    float time;

    // Start is called before the first frame update
    void Start(){

        presetName = presets[preset].name;
        axiom = presets[preset].axiom;
        angle = presets[preset].angle;
        n = presets[preset].n;
        productionRules = presets[preset].rules;

        currentTree += axiom;
        Debug.Log(currentTree);
        
    }

    void Update() {

        if(time >= timePerGen){

            if(currentGeneration < n){

                GenerateTree();

            }

            time = 0;

        }
        
        time += Time.deltaTime;
    }

    public string ApplyRule(char c){

        bool matchFound = false;
        int index = -1;

        for(int i = 0;i < productionRules.Length;i++){

            if(productionRules[i].Match(c)){

                matchFound = true;
                index = i;
                        
            }

        }

        if(matchFound){

            return productionRules[index].GetResult();
            
        } else {

            return c.ToString();

        }

    }

    void GenerateTree(){

        currentGeneration++;
        newTree = "";

        for(int i = 0;i < currentTree.Length;i++){

            char c = currentTree[i];

            newTree += ApplyRule(c);

        }

        currentTree = newTree;
        Debug.Log(currentTree);
        GenerateTreeVisual(currentTree);

    }

    void SpawnLine(Vector3 start,Vector3 end){

        GameObject obj = Instantiate(line,transform.position,Quaternion.identity);
        LineRenderer lineRenderer = obj.GetComponent<LineRenderer>();

        obj.transform.parent = newTreeObj.transform;
        lineRenderer.SetPosition(0,start);
        lineRenderer.SetPosition(1,end);

    }

    void GenerateTreeVisual(string treeString){

        Destroy(newTreeObj);
        spawner.position = Vector3.zero;
        spawner.rotation = Quaternion.identity;

        newTreeObj = (GameObject)Instantiate(tree,Vector3.zero,Quaternion.identity);

        for(int i=0;i<treeString.Length;i++){

            switch(treeString[i]){

                case '+':

                    spawner.Rotate(Vector3.forward * angle,Space.Self);
                    break;

                case '-':
                    
                    spawner.Rotate(Vector3.forward * -angle,Space.Self);
                    break;

                case '[':

                    nodePos = new NodePos();
                    nodePos.position = spawner.position;
                    nodePos.rotation = spawner.rotation;

                    nodeStack.Push(nodePos);

                    break;

                case ']':

                    nodePos = nodeStack.Pop();
                    spawner.position = nodePos.position;
                    spawner.rotation = nodePos.rotation;

                    break;
                    
                default:

                    Vector3 intialPos = spawner.position;

                    spawner.Translate(Vector3.up * length);
                    SpawnLine(intialPos,spawner.position);
                    break;


            }

            //cameraContain.UpdatePosition(spawner.position);

            cameraContain.UpdatePositionAvg(Vector3.zero,spawner.position);

            cameraContain.camera.orthographicSize = spawner.position.y * cameraContain.spawnerToSizeRatio;


        }



    }

}
