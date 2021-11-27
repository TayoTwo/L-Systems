using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMesh : MonoBehaviour
{

    Mesh mesh;

    public Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
