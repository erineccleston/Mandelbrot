// https://en.wikibooks.org/wiki/Cg_Programming/Unity/Computing_Image_Effects

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class Test : MonoBehaviour
{
    // todo: switch between float and double as needed
    public ComputeShader Shader;

    public int Iterations = 256;

    public Vector3 Speed = Vector3.one;

    static readonly Double3 Default = new Double3(-0.75, 0, 1);

    private Double3 ABR = Default;
    private Double3 Last;

    private RenderTexture Temp;
    private ComputeBuffer CB;

    void Awake()
    {
        Cursor.visible = false;
        Shader.SetInt("Iterations", Iterations);

        CB = new ComputeBuffer(4, 8);
        Shader.SetBuffer(0, "Params", CB);
    }

    void Update()
    {
        int GetValue(KeyCode negative, KeyCode positive)
        {
            return (Input.GetKey(negative) ? -1 : 0) + (Input.GetKey(positive) ? 1 : 0);
        }

        double a, b, r;
        double scale = Time.deltaTime * ABR.z;
        a = GetValue(KeyCode.A, KeyCode.D) * Speed.x * scale;
        b = GetValue(KeyCode.S, KeyCode.W) * Speed.y * scale;
        r = GetValue(KeyCode.E, KeyCode.Q) * Speed.z * scale;
        ABR += new Double3(a, b, r);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Iterations *= 2;
            Last = new Double3();
            Shader.SetInt("Iterations", Iterations);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Iterations >= 2)
                Iterations /= 2;
            Last = new Double3();
            Shader.SetInt("Iterations", Iterations);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ABR = Default;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //if (Input.GetKeyDown(KeyCode.SysReq))
        //{
        //    Texture2D tex = new Texture2D(Temp.width, Temp.height);
        //    //RenderTexture.active = rTex;
        //    tex.ReadPixels(new Rect(0, 0, Temp.width, Temp.height), 0, 0);
        //    tex.Apply();
        //    System.IO.File.WriteAllBytes(System.DateTime.Now + ".png", tex.EncodeToPNG());
        //}
    }

    void OnDestroy()
    {
        if (Temp != null)
        {
            Temp.Release();
            Temp = null;
        }

        CB.Release();
        CB = null;
    }

    List<double> GetParams(double width, double height)
    {
        double r1 = ABR.z;
        double r2 = ABR.z * (width / height);

        return new List<double>()
        {
            ABR.x - r2,
            r2 * 2 / width,
            ABR.y - r1,
            r1 * 2 / height
        };
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        int width = src.width;
        int height = src.height;

        if (Temp == null || Temp.width != width || Temp.height != height)
        {
            if (Temp != null)
                Temp.Release();

            Temp = new RenderTexture(width, height, src.depth);
            Temp.enableRandomWrite = true;
            Temp.Create();
            Shader.SetTexture(0, "RendTex", Temp);
        }

        if (ABR.x != Last.x || ABR.y != Last.y || ABR.z != Last.z || false)
        {
            //Shader.SetVector("Parameters", SetParams(width, height));
            CB.SetData(GetParams(width, height));

            Shader.Dispatch(0, (width + 31) / 32, (height + 31) / 32, 1);
            Last = ABR;
        }

        Graphics.Blit(Temp, null as RenderTexture); // test null vs dst
    }
}