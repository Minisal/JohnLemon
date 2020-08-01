using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeModel
{
    private GameObject model;
    private float fadeTime = 2f;
    private List<Material> materials = new List<Material>();

    public FadeModel(GameObject model, float fadeTime = 2f)
    {
        this.model = model;
        this.fadeTime = fadeTime;
        SkinnedMeshRenderer[] meshRenderers = model.GetComponents<SkinnedMeshRenderer>();
        //MeshRenderer[] meshRenderers = model.GetComponents<MeshRenderer>();
        foreach (SkinnedMeshRenderer mr in meshRenderers)
        {
            Material[] materals = mr.materials;
            foreach (Material m in materals)
            {
                if (!materials.Contains(m))
                {
                    materials.Add(m);
                }
            }
        }
    }

    public void HideModel()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            Material m = materials[i];
            Color color = m.color;
            m.color = new Color(color.r, color.g, color.b, 1);
            setMaterialRenderingMode(m, RenderingMode.Fade);
            m.DOColor(new Color(color.r, color.g, color.b, 0), fadeTime);
        }
    }

    public void ShowModel()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            Material m = materials[i];
            Color color = m.color;
            setMaterialRenderingMode(m, RenderingMode.Opaque);
        }
    }

    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    private void setMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}