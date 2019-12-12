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

    public double A = -0.75, B = 0, R = 1;
    private double Al, Bl, Rl;

    public bool Debug;

    private RenderTexture Temp;
    private ComputeBuffer CB;

    private AlgorithmSelection AS;

    void OnEnable()
    {
        Cursor.visible = false;
        Shader.SetInt("Iterations", Iterations);

        CB = new ComputeBuffer(4, sizeof(double));
        Shader.SetBuffer(0, "Params", CB);

        AS = FindObjectOfType<AlgorithmSelection>();
        Shader.SetInt("Fractal", (int)AS.GetCurrentFractal());
    }

    void Update()
    {
        int GetValue(KeyCode negative, KeyCode positive)
        {
            return (Input.GetKey(negative) ? -1 : 0) + (Input.GetKey(positive) ? 1 : 0);
        }

        double scale = Time.deltaTime * R;
        A += GetValue(KeyCode.A, KeyCode.D) * Speed.x * scale;
        B += GetValue(KeyCode.S, KeyCode.W) * Speed.y * scale;
        R += GetValue(KeyCode.E, KeyCode.Q) * Speed.z * scale;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Iterations *= 2;
            Rl = 0;
            Shader.SetInt("Iterations", Iterations);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Iterations >= 2)
                Iterations /= 2;
            Rl = 0;
            Shader.SetInt("Iterations", Iterations);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            A = -0.75;
            B = 0;
            R = 1.25;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
            AS.gameObject.SetActive(true);
            gameObject.SetActive(false);
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

    void OnDisable()
    {
        Rl = 0;

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
        double r1 = R;
        double r2 = R * (width / height);

        return new List<double>()
        {
            A - r2,
            r2 * 2 / width,
            -B + r1,
            -r1 * 2 / height
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

        if (A != Al || B != Bl || R != Rl || Debug)
        {
            CB.SetData(GetParams(width, height));

            Shader.Dispatch(0, (width + 31) / 32, (height + 31) / 32, 1);
            Al = A;
            Bl = B;
            Rl = R;
        }

        Graphics.Blit(Temp, null as RenderTexture); // test null vs dst
    }
}