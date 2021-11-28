using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BranchPos {
    public Vector3 position;
    public Quaternion rotation;

}

public class Generator : MonoBehaviour
{


    [Header("Objects")]
    public CameraContain cameraContain;
    public Transform spawner;
    public GameObject treeObj;
    public GameObject line;
    public Stack<BranchPos> branchStack = new Stack<BranchPos>();
    
    RulePreset preset;
    GameObject newTreeObj;
    List<Vector3> treeVertices = new List<Vector3>();
    List<int> treeTriangles = new List<int>();

    float tallestPoint = 0f;

    public void GenerateTree(string tree,int mode ,float len ,float wid ,float ang){


        switch(mode){

            case 0:

                GenerateTreeLines(tree,len,wid,ang);
                cameraContain.UpdateOffset(true);
                break;

            case 1:

                GenerateTreeMesh(tree,len,wid,ang);
                cameraContain.UpdateOffset(false);
                break;

        }


    }

    //Generate the tree as a mesh or as lines depending on the mode
    //0 is for lines
    //1 is for meshes
    void GenerateTreeLines(string treeString,float length,float width,float angle){

        BranchPos branchPos = new BranchPos();
        Destroy(newTreeObj);
        spawner.position = Vector3.zero;
        spawner.rotation = Quaternion.identity;

        newTreeObj = (GameObject)Instantiate(treeObj,Vector3.zero,Quaternion.identity);

        for(int i=0;i<treeString.Length;i++){

            //Debug.Log(i);

            switch(treeString[i]){

                case '+':

                    spawner.Rotate(Vector3.forward * angle,Space.Self);
                    break;

                case '-':
                    
                    spawner.Rotate(Vector3.forward * -angle,Space.Self);
                    break;

                case '[':

                    branchPos = new BranchPos();
                    branchPos.position = spawner.position;
                    branchPos.rotation = spawner.rotation;

                    branchStack.Push(branchPos);

                    break;

                case ']':

                    branchPos = branchStack.Pop();
                    spawner.position = branchPos.position;
                    spawner.rotation = branchPos.rotation;

                    break;
                    
                default:

                    Vector3 intialPos = spawner.position;

                    spawner.Translate(Vector3.up * length);
                    SpawnLine(intialPos,spawner.position);
                    break;


            }

            //Camera stuff
            if(tallestPoint < spawner.position.y){

                tallestPoint = spawner.position.y;

            }

            cameraContain.UpdatePositionAvg(Vector3.zero, Vector3.up * tallestPoint);


        }



    }
    void SpawnLine(Vector3 start,Vector3 end){

        GameObject obj = Instantiate(line,transform.position,Quaternion.identity);
        LineRenderer lineRenderer = obj.GetComponent<LineRenderer>();

        obj.transform.parent = newTreeObj.transform;
        lineRenderer.SetPosition(0,start);
        lineRenderer.SetPosition(1,end);

    }

    void GenerateTreeMesh(string treeString,float length,float width,float angle){

        int vertexIndex = 0;
        int start = 0;
        BranchPos branchPos = new BranchPos();
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

                    branchPos = new BranchPos();
                    branchPos.position = spawner.position;
                    branchPos.rotation = spawner.rotation;

                    branchStack.Push(branchPos);

                    break;

                case ']':

                    branchPos = branchStack.Pop();
                    spawner.position = branchPos.position;
                    spawner.rotation = branchPos.rotation;

                    start = vertexIndex * 4;
                    AddTop(start);
                    break;
                    
                default:

                    Vector3 intialPos = spawner.position;
                    spawner.Translate(Vector3.up * length);

                    vertexIndex++;
                    AddVertices(spawner.position,width);

                    break;

            }

            //Camera stuff
            if(tallestPoint < spawner.position.y){

                tallestPoint = spawner.position.y;

            }


        }

        start = 0;

        for(int j = 0;j < vertexIndex;j++){

            //Debug.Log(start);

            AddSides(start);

            start += 4;

        }

        //Generate the tree Mesh
        newTreeObj.GetComponent<MeshFilter>().mesh.vertices =  treeVertices.ToArray();
        newTreeObj.GetComponent<MeshFilter>().mesh.triangles = treeTriangles.ToArray();
        newTreeObj.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        cameraContain.UpdatePositionAvg(Vector3.zero, Vector3.up * tallestPoint);

    }

    void AddVertices(Vector3 pos,float width){

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


}
