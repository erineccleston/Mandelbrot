using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    public ComputeShader Shader;
    public Vector3Int Voxels;
    public Vector3 Begin, End;
    public double W;
    public int Iterations;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshFilter>().mesh = new Mandelbulb(Shader, Voxels, Begin, End, W, Iterations).Mesh;
    }
}
