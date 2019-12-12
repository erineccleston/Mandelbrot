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
        //GetComponent<MeshFilter>().mesh = new Mandelbulb(Shader, Voxels, Begin, End, W, Iterations).Mesh;
        var bulb = new Mandelbulb(Shader, new Vector3Int(128, 128, 1), new Vector3d(-2, -2, 0), new Vector3d(2, 2, 0), 0, 128);
        var lines = new List<string>();
        for (int i = 0; i < 128; i++)
        {
            lines.Add(string.Join("", new System.ArraySegment<float>(bulb.Data, i * 128, 128)));
        }
        System.IO.File.WriteAllLines("dump.txt", lines);
        //System.IO.File.WriteAllText("dump.txt", string.Join("", bulb.Data));
    }
}
