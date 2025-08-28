using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParticleSystem : MaskableGraphic
{
    [SerializeField] private ParticleSystemRenderer _particleSystemRenderer;
    [SerializeField] private Texture _texture;

    public override Texture mainTexture => _texture ?? base.mainTexture;

    private void Update()
    {
        SetVerticesDirty();
    }
    protected override void OnPopulateMesh(Mesh mesh)
    {
        mesh.Clear();
        if (_particleSystemRenderer != null && Camera.main != null)
            _particleSystemRenderer.BakeMesh(mesh, Camera.main);
    }

    //private Mesh reusableMesh;

    //protected override void OnPopulateMesh(VertexHelper vh)
    //{
    //    if (reusableMesh == null)
    //    {
    //        reusableMesh = new Mesh();
    //    }

    //    vh.Clear();

    //    if (_particleSystemRenderer != null && Camera.main != null)
    //    {
    //        _particleSystemRenderer.BakeMesh(reusableMesh, Camera.main);

    //        var vertices = reusableMesh.vertices;
    //        var colors = reusableMesh.colors32;
    //        var uv = reusableMesh.uv;
    //        var triangles = reusableMesh.triangles;

    //        for (int i = 0; i < vertices.Length; i++)
    //        {
    //            UIVertex vertex = new UIVertex
    //            {
    //                position = vertices[i],
    //                color = i < colors.Length ? colors[i] : Color.white,
    //                uv0 = i < uv.Length ? uv[i] : Vector2.zero
    //            };
    //            vh.AddVert(vertex);
    //        }

    //        for (int i = 0; i < triangles.Length; i += 3)
    //        {
    //            vh.AddTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
    //        }
    //    }
    //}
}