// https://en.wikibooks.org/wiki/Cg_Programming/Unity/Computing_Image_Effects

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Test : MonoBehaviour
{
    // todo: switch between float and double as needed
    public ComputeShader Shader;

    public int Iterations = 256;

    public Vector3 Speed = Vector3.one;

    static readonly Vector3 Default = new Vector3(-0.75f, 0, 1);

    public Vector3 ABR = Default;
    private Vector3 Last;

    private RenderTexture Temp;

    void Awake()
    {
        Cursor.visible = false;
        Shader.SetInt("Iterations", Iterations);
    }

    void Update()
    {
        int GetValue(KeyCode negative, KeyCode positive)
        {
            return (Input.GetKey(negative) ? -1 : 0) + (Input.GetKey(positive) ? 1 : 0);
        }

        float a, b, r;
        float scale = Time.deltaTime * ABR.z;
        a = GetValue(KeyCode.A, KeyCode.D) * Speed.x * scale;
        b = GetValue(KeyCode.S, KeyCode.W) * Speed.y * scale;
        r = GetValue(KeyCode.E, KeyCode.Q) * Speed.z * scale;
        ABR += new Vector3(a, b, r);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Iterations *= 2;
            Last = Vector3.zero;
            Shader.SetInt("Iterations", Iterations);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Iterations /= 2;
            Last = Vector3.zero;
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
    }

    void OnDestroy()
    {
        if (Temp != null)
        {
            Temp.Release();
            Temp = null;
        }
    }

    Vector4 CalcParams(float width, float height)
    {
        float r1 = ABR.z;
        float r2 = ABR.z * (width / height);

        return new Vector4
        (
            ABR.x - r2,
            r2 * 2 / width,
            ABR.y - r1,
            r1 * 2 / height
        );
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

        if (ABR.x != Last.x || ABR.y != Last.y || ABR.z != Last.z)
        {
            Shader.SetVector("Parameters", CalcParams(width, height));
            Shader.Dispatch(0, width / 32, height / 32, 1);
            Last = ABR;
        }

        Graphics.Blit(Temp, null as RenderTexture); // test null vs dst
    }
}