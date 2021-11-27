using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule{
    
    public char inital;
    public string result;

    public string Apply(char c){

        if(c == inital){

            return result;

        } else {

            return c.ToString();

        }

    }

    public string GetResult(){

        return result;

    }

}

public class MeshObj{

    Vector3[] vertices;
    int[] triangles;

    public MeshObj(Vector3[] v,int[] t){

       vertices = v;
       triangles = t;

    }

    public Mesh Generate(){

        Mesh m = new Mesh();

        m.vertices = vertices;
        m.triangles = triangles;

        m.RecalculateNormals();

        return m;

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
    public float width = 1f;

    [Header("Realtime Values")]
    [SerializeField]  
    public Stack<NodePos> nodeStack = new Stack<NodePos>();
    public string currentTree;
    public int currentGeneration = 0;

    [Header("Objects")]
    public Transform spawner;
    public GameObject nodeObj;
    public GameObject treeObj;
    public GameObject line;
    NodePos nodePos = new NodePos();

    [Header("Preset's Values")]
    public string presetName;
    public string axiom;
    public float angle = 25f;
    public int n;
    public Rule[] productionRules = new Rule[1];

    string newTree;
    Mesh treeMesh;
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

        SpawnMesh();
        
    }

    void Update() {

        // if(time >= timePerGen){

        //     if(currentGeneration < n){

        //         GenerateTree();

        //     }

        //     time = 0;

        // }
        
        // time += Time.deltaTime;
    }

    void GenerateTree(){

        currentGeneration++;
        newTree = "";

        for(int i = 0;i < currentTree.Length;i++){

            char c = currentTree[i];

            string result = "";

            for(int j = 0;j < productionRules.Length;j++){

                result = productionRules[j].Apply(c);

            }

            newTree += result;

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

    void SpawnMesh(Vector3 pos){

        GameObject obj = Instantiate(treeObj,transform.position,Quaternion.identity);

        Vector3[] vertices = new Vector3[]{

                Vector3.zero,
                Vector3.right,
                (Vector3.right + Vector3.forward),
                Vector3.forward,
                Vector3.up,
                (Vector3.up + Vector3.forward),
                Vector3.one,
                (Vector3.right + Vector3.up)

            };

        for(int i = 0;i < vertices.Length;i++){

            vertices[i].x *= width;
            vertices[i].z *= length;

        }

        MeshObj meshObj = new MeshObj(
            vertices,
            new int[]{

                //SideX+
                1,7,6,
                6,2,1,
                
                //SideX-
                0,3,5,
                5,4,0,

                //SideZ+
                6,5,3,
                3,2,6,

                //SideZ-
                7,1,0,
                0,4,7

            }

        );

        obj.GetComponent<MeshFilter>().mesh = meshObj.Generate();

        obj.transform.position = 

    }

    //Using line renderers
    void GenerateTreeVisual(string treeString){

        Destroy(newTreeObj);
        spawner.position = Vector3.zero;
        spawner.rotation = Quaternion.identity;

        newTreeObj = (GameObject)Instantiate(treeObj,Vector3.zero,Quaternion.identity);

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

    void GenerateTreeMesh(string treeString){

        Destroy(newTreeObj);
        spawner.position = Vector3.zero;
        spawner.rotation = Quaternion.identity;

        newTreeObj = (GameObject)Instantiate(treeObj,Vector3.zero,Quaternion.identity);

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
