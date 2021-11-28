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

public class MeshObj{

    public Vector3[] vertices;
    public int[] triangles;

    public MeshObj(){}
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
    GameObject newTreeObj;
    float time;


    Mesh treeMesh;
    MeshObj meshObj;
    List<Vector3> treeVertices = new List<Vector3>();
    List<int> treeTriangles = new List<int>();

    LightingManager lightingManager;

    // Start is called before the first frame update
    void Start(){

        treeMesh = new Mesh();
        meshObj = new MeshObj();

        lightingManager = GetComponent<LightingManager>();

        presetName = presets[preset].name;
        axiom = presets[preset].axiom;
        angle = presets[preset].angle;
        n = presets[preset].n;
        productionRules = presets[preset].rules;

        currentTree += axiom;
        GenerateTree();
        
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

    void GenerateTree(){

        newTree = "";

        for(int i = 0;i < currentTree.Length;i++){

            char c = currentTree[i];

            string result = "";

            for(int j = 0;j < productionRules.Length;j++){

                if(productionRules[j].Match(c)){

                    result = productionRules[j].GetResult();

                }

            }

            //Debug.Log("Result: " + result);
            newTree += result;

        }

        currentTree = newTree;
        Debug.Log("Current Tree: " + currentTree);
        GenerateTreeVisual(currentTree);
        currentGeneration++;


    }

    void SpawnLine(Vector3 start,Vector3 end){

        GameObject obj = Instantiate(line,transform.position,Quaternion.identity);
        LineRenderer lineRenderer = obj.GetComponent<LineRenderer>();

        obj.transform.parent = newTreeObj.transform;
        lineRenderer.SetPosition(0,start);
        lineRenderer.SetPosition(1,end);

    }

    void AddVertices(Vector3 pos){

        Vector3[] v = new Vector3[]{

            Vector3.up,
            (Vector3.up + Vector3.forward),
            Vector3.one,
            (Vector3.right + Vector3.up)

        };

        for(int i = 0; i < v.Length;i++){

            v[i].x -= 0.5f;
            v[i].z -= 0.5f;

            v[i].x *= width;
            v[i].z *= width;

            v[i] += pos;

        }

        treeVertices.AddRange(v);


    }

    void AddSides(int s){

        //Example
                    // 0,1,5,
                    // 5,4,0,

                    // 1,2,5,
                    // 5,2,6,

                    // 2,3,6,
                    // 6,3,7,

                    // 3,0,7,
                    // 7,0,4


        //Adds 24 individual points but THREE TIMES LESS triangles i.e 8 triangles
        treeTriangles.AddRange(

                new int[]{

                    s,s+1,s+5,
                    s+5,s+4,s,

                    s+1,s+2,s+5,
                    s+5,s+2,s+6,

                    s+2,s+3,s+6,
                    s+6,s+3,s+7,

                    s+3,s,s+7,
                    s+7,s,s+4


                }

            );
        

    }

    void AddTop(int s){

        treeTriangles.AddRange(
            
            new int[]{

                s,s+1,s+2,
                s+2,s+2,s

            } 

        );


    }

    // //Using line renderers
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

            //cameraContain.cam.orthographicSize = spawner.position.y * cameraContain.spawnerToSizeRatio;


        }



    }

    void GenerateTreeMesh(string treeString){

        int vertexIndex = 0;
        int start = 0;
        treeVertices.Clear();
        treeTriangles.Clear();

        Destroy(newTreeObj);
        spawner.position = Vector3.zero;
        spawner.rotation = Quaternion.identity;

        Vector3[] v = new Vector3[]{

            Vector3.zero,
            Vector3.forward,
            (Vector3.right + Vector3.forward),
            Vector3.right

        };

        for(int i = 0; i < v.Length;i++){

            v[i].x -= 0.5f;
            v[i].z -= 0.5f;

            v[i].x *= width;
            v[i].z *= width;

            v[i] += spawner.position;

        }

        treeVertices.AddRange(v);
        
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

                    AddTop(vertexIndex*4);

                    break;
                    
                default:

                    vertexIndex++;
                    Vector3 intialPos = spawner.position;
                    spawner.Translate(Vector3.up * length);
                    AddVertices(spawner.position);

                    break;

            }


        }

        start = 0;

        for(int j = 0;j < vertexIndex;j++){

            //Debug.Log(start);

            AddSides(start);

            start += 4;

        }

        //Debug.Log("G: " + currentGeneration + " V: " + treeVertices.Count + " T: " + (treeTriangles.Count/3));

        newTreeObj.GetComponent<MeshFilter>().mesh.vertices =  treeVertices.ToArray();
        newTreeObj.GetComponent<MeshFilter>().mesh.triangles = treeTriangles.ToArray();

        newTreeObj.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        lightingManager.RerenderScene();

        cameraContain.UpdatePosition(spawner.position);
        //cameraContain.UpdatePositionAvg(Vector3.zero,spawner.position);

        //cameraContain.camera.orthographicSize = spawner.position.y * cameraContain.spawnerToSizeRatio;

    }
}
