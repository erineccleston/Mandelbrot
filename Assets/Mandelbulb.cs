using System;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using MarchingCubesProject;

public class Mandelbulb
{
    //readonly RenderTexture Volume;
    public readonly Mesh Mesh = new Mesh();
    public float[] Data;

    public Mandelbulb(ComputeShader shader, Vector3Int voxels, Vector3d start, Vector3d end, double w, int iterations)
    {
        double step(int i) => (end[i] - start[i]) / voxels[i];

        //Volume = new RenderTexture(voxels.x, voxels.y, 0);
        ////Volume.format = alpha8
        //Volume.dimension = TextureDimension.Tex3D;
        //Volume.volumeDepth = voxels.z;
        //Volume.enableRandomWrite = true;
        //Volume.Create();
        ////Texture3D t3d = new Texture3D(voxels.x, voxels.y, voxels.z, TextureFormat.Alpha8, false);
        //shader.SetTexture(0, "Volume", Volume);

        shader.SetInt("Iterations", iterations);

        ComputeBuffer args = new ComputeBuffer(7, sizeof(double));
        args.SetData(new[] { start.x, step(0), start.y, step(1), start.z, step(2), w });
        shader.SetBuffer(0, "Params", args);

        ComputeBuffer data = new ComputeBuffer(voxels.x * voxels.y * voxels.z, sizeof(float), ComputeBufferType.Raw);
        shader.SetBuffer(0, "Data", data);

        shader.Dispatch(0, (voxels.x + 9) / 10, (voxels.y + 9) / 10, (voxels.z + 9) / 10);

        Data = new float[voxels.x * voxels.y * voxels.z];
        data.GetData(Data);
        March(Data, voxels);

        args.Release();
        args = null;

        data.Release();
        data = null;
    }

    //~Mandelbulb()
    //{
    //    Volume.Release();
    //}

    //public Mesh Generate()
    //{
    //    var old = RenderTexture.active;
    //    RenderTexture.active = Volume;

    //    var t3d = new Texture3D();
    //    t3d.SetPixels

    //    RenderTexture.active = old;
    //}

    void March(IList<float> voxels, Vector3Int dimensions)
    {
        MarchingTertrahedron marching = new MarchingTertrahedron();

        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();

        marching.Generate(voxels, dimensions.x, dimensions.y, dimensions.z, verts, indices);

        Mesh.indexFormat = IndexFormat.UInt32;

        Mesh.SetVertices(verts);
        Mesh.SetTriangles(indices, 0);
        Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();
    }
}

public readonly struct Vector3d
{
    public readonly double x, y, z;

    public Vector3d(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public double this[int i] => i == 0 ? x : (i == 1 ? y : (i == 2 ? z : throw new IndexOutOfRangeException()));

    public static implicit operator Vector3d(Vector3 v3)
    {
        return new Vector3d(v3.x, v3.y, v3.z);
    }
}