using UnityEngine;
using System.Collections;

public class TilerEffects : MonoBehaviour
{
    public Shader shader;
    [Range(1, 16)]
    public int
        division = 3;
    [Range(0.0f, 1.0f)]
    public float
        displace = 0.2f;
    Material material;
    Vector2[] offsets;

    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            material = new Material (shader);
        }

        if (offsets == null || division * division != offsets.Length)
        {
            offsets = new Vector2[division * division];
            for (var i = 0; i < division * division; i++)
            {
                offsets [i] = new Vector2 (Random.value, Random.value);
            }
        }

        RenderTexture.active = destination;

        material.SetTexture ("_MainTex", source);
        
        GL.PushMatrix ();
        GL.LoadOrtho ();

        material.SetPass (0);
        GL.Color (Color.white);

        GL.Begin (GL.QUADS);

        var w = 1.0f / division;

        for (var r = 0; r < division; r++)
        {
            for (var c = 0; c < division; c++)
            {
                var o1 = offsets [r * division + c] * displace;
                var o2 = o1 + Vector2.one * (1.0f - displace);

                GL.TexCoord2 (o1.x, o1.y);
                GL.Vertex3 (w * c, w * r, 0.0f);
                
                GL.TexCoord2 (o2.x, o1.y);
                GL.Vertex3 (w * c + w, w * r, 0.0f);

                GL.TexCoord2 (o2.x, o2.y);
                GL.Vertex3 (w * c + w, w * r + w, 0.0f);

                GL.TexCoord2 (o1.x, o2.y);
                GL.Vertex3 (w * c, w * r + w, 0.0f);
            }
        }

        GL.End ();
        GL.PopMatrix ();
    }
}
