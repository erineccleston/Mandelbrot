using System;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using MarchingCubesProject;

public class MandelbulbOld
{
    public readonly Mesh Mesh = new Mesh();

    public MandelbulbOld(ComputeShader shader, Vector3Int voxels, Vector3d start, Vector3d end, double w, int iterations)
    {
        double step(int i) => (end[i] - start[i]) / voxels[i];

        var t3d = new RenderTexture(voxels.x, voxels.y, 0);
        t3d.dimension = TextureDimension.Tex3D;
        t3d.volumeDepth = voxels.z;
        t3d.enableRandomWrite = true;
        t3d.Create();
        //Texture3D t3d = new Texture3D(voxels.x, voxels.y, voxels.z, TextureFormat.Alpha8, false);
        shader.SetTexture(0, "Volume", t3d);

        shader.SetInt("Iterations", iterations);

        ComputeBuffer args = new ComputeBuffer(7, sizeof(double));
        args.SetData(new[] { start.x, step(0), start.y, step(1), start.z, step(2), w });
        shader.SetBuffer(0, "Params", args);

        //ComputeBuffer data = new ComputeBuffer(voxels.x * voxels.y * voxels.z, sizeof(float));

        shader.Dispatch(0, (voxels.x + 9) / 10, (voxels.y + 9) / 10, (voxels.z + 9) / 10);

        //March(Array.ConvertAll(t3d.GetPixels(), c => c.a), voxels);

        args.Release();
        args = null;

        t3d.Release();
        t3d = null;
    }

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

//public readonly struct Vector3d
//{
//    public readonly double x, y, z;

//    public Vector3d(double x, double y, double z)
//    {
//        this.x = x;
//        this.y = y;
//        this.z = z;
//    }

//    public double this[int i] => i == 0 ? x : (i == 1 ? y : (i == 2 ? z : throw new IndexOutOfRangeException()));

//    public static implicit operator Vector3d(Vector3 v3)
//    {
//        return new Vector3d(v3.x, v3.y, v3.z);
//    }
//}