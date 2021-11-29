using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BranchPos {
    public Vector3 position;
    public Quaternion rotation;

}

public class Generator : MonoBehaviour{

    [Header("Objects")]
    public Transform spawner;
    public GameObject treeObj;
    public GameObject line;
    public Stack<BranchPos> branchStack = new Stack<BranchPos>();
    
    GameObject newTreeObj;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    public float tallestPoint = 0f;

    public void VisualizeSequence(string tree,bool mesh,float len ,float wid ,float ang){

        tallestPoint = 0;

        switch(mesh){

            case false:

                GenerateSequenceLines(tree,len,wid,ang);
                break;

            case true:

                GenerateSequenceMesh(tree,len,wid,ang);
                break;

        }


    }

    //Generate the tree as a mesh or as lines depending on the mode
    //0 is for lines
    //1 is for meshes
    void GenerateSequenceLines(string sequenceString,float length,float width,float angle){

        BranchPos branchPos = new BranchPos();
        Destroy(newTreeObj);
        spawner.position = Vector3.zero;
        spawner.rotation = Quaternion.identity;

        newTreeObj = (GameObject)Instantiate(treeObj,Vector3.zero,Quaternion.identity);

        for(int i=0;i<sequenceString.Length;i++){

            switch(sequenceString[i]){

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
                    SpawnLine(intialPos,spawner.position,width);
                    break;

            }

             //Camera stuff
            if(tallestPoint < spawner.position.y){

                tallestPoint = spawner.position.y;

            }


        }


    }
    void SpawnLine(Vector3 start,Vector3 end, float width){

        GameObject obj = Instantiate(line,transform.position,Quaternion.identity);
        LineRenderer lineRenderer = obj.GetComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        obj.transform.parent = newTreeObj.transform;
        lineRenderer.SetPosition(0,start);
        lineRenderer.SetPosition(1,end);

    }

    void GenerateSequenceMesh(string sequenceString,float length,float width,float angle){

        int vertexIndex = 0;
        int start = 0;
        BranchPos branchPos = new BranchPos();
        vertices.Clear();
        triangles.Clear();
        
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

        vertices.AddRange(v);
        
        newTreeObj = (GameObject)Instantiate(treeObj,Vector3.zero,Quaternion.identity);

        for(int i=0;i<sequenceString.Length;i++){

            start = vertexIndex * 4;

           switch(sequenceString[i]){

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

        AddTop(start);

        Mesh mesh =  newTreeObj.GetComponent<MeshFilter>().mesh;

        //Generate the tree Mesh
        mesh.Clear();
        mesh.vertices =  vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();


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

        vertices.AddRange(v);


    }

    void AddSides(int s){

        //Adds 24 individual points but THREE TIMES LESS triangles i.e 8 triangles
        triangles.AddRange(

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

        triangles.AddRange(
            
            new int[]{

                s,s+1,s+2,
                s+2,s+3,s

            } 

        );

    }


}
