using UnityEngine;

public class RenderToScreen : MonoBehaviour
{
    public RenderTexture RendTex;
    public ComputeShader Shader;

    [SerializeField]
    // left, dx, top, dy
    float[] Params = new float[4];

    public Vector3 Speed = new Vector3(1, 1, 1);

    [SerializeField]
    // a, b, r
    Vector3 Position = new Vector3(0, 0, 2);
    Vector3 LastPosition;

    void Update()
    {
        int GetValue(KeyCode negative, KeyCode positive)
        {
            return (Input.GetKey(negative) ? -1 : 0) + (Input.GetKey(positive) ? 1 : 0);
        }

        float a, b, r;
        a = GetValue(KeyCode.A, KeyCode.D) * Time.deltaTime * Speed.x;
        b = GetValue(KeyCode.S, KeyCode.W) * Time.deltaTime * Speed.y;
        r = GetValue(KeyCode.E, KeyCode.Q) * Time.deltaTime * Speed.z * Position.z;
        Position += new Vector3(a, b, r);
    }

    void UpdateParams()
    {
        float r1 = Position.z;
        float r2 = Position.z * (Screen.width / Screen.height);

        Params[0] = Position.x - r2;
        Params[1] = r2 * 2 / Screen.width;
        Params[2] = Position.y + r1;
        Params[3] = r1 * 2 / Screen.height; ;
    }

    void Awake()
    {
        //RendTex = new RenderTexture(Screen.width, Screen.height, 0);
        //RendTex.enableRandomWrite = true;
        Shader.SetTexture(0, nameof(RendTex), RendTex);
    }

    //void OnRenderImage(RenderTexture src, RenderTexture dest)
    //{
    //    Graphics.Blit(RendTex, null as RenderTexture);
    //    if (Position != LastPosition)
    //    {
    //        UpdateParams();
    //        Shader.SetFloats(nameof(Params), Params);
    //        Shader.Dispatch(Shader.FindKernel("CSMain"), Screen.width / 16, Screen.height / 16, 1);
    //        LastPosition = Position;
    //    }
    //}

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //if (Position != LastPosition)
        //{
            var rt = RenderTexture.GetTemporary(src.width, src.height, 0);
        rt.enableRandomWrite = true;
            UpdateParams();
            Shader.SetFloats(nameof(Params), Params);
            Shader.Dispatch(0, Screen.width / 16, Screen.height / 16, 1);
            LastPosition = Position;
        //}
        Graphics.Blit(RendTex, null as RenderTexture);
            RenderTexture.ReleaseTemporary(rt);
    }

    public void Dispatch(RenderTexture texture)
    {
        var tmpSource = RenderTexture.GetTemporary(texture.width, texture.height, 0, texture.format);
        Graphics.Blit(texture, tmpSource);

        texture.enableRandomWrite = true;

        Shader.SetTexture(0, "_Texture", tmpSource);
        Shader.SetTexture(0, "_Result", texture);
        Shader.Dispatch(0, texture.width / 32, texture.height / 32, 1);
        RenderTexture.ReleaseTemporary(tmpSource);

    }
}
