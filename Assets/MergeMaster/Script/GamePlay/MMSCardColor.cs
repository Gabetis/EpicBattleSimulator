using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSCardColor : MonoBehaviour
{
    public Material BlueTexture;
    public Material RedTexture;
    Material usedMaterial;
    public Renderer[] Renderers;
    public ParticleSystem[] ParticleSystemsAttack;
    Color flashColor = new Color(0.5f, 0.5f, 0.5f);

    public void SetTexture(bool isUser)
    {
        Material material = (isUser ? BlueTexture : RedTexture);
        usedMaterial = new Material(material.shader);
        usedMaterial.mainTexture = material.mainTexture;
        usedMaterial.color = material.color;
        material.CopyPropertiesFromMaterial(usedMaterial);
        usedMaterial.EnableKeyword("_EMISSION");
        for (int i = 0; i < Renderers.Length; i++)
        {
            Renderers[i].material = usedMaterial;
        }
        if (ParticleSystemsAttack.Length != 0 || ParticleSystemsAttack != null)
        {
            for (int i = 0; i < ParticleSystemsAttack.Length; i++)
            {
                if (ParticleSystemsAttack[i] != null)
                    ParticleSystemsAttack[i].startColor = (isUser ? Color.blue : Color.red);
            }
        }
    }

    public void SetFlash()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        usedMaterial.SetColor("_EmissionColor", flashColor);

        yield return new WaitForSeconds(0.35f);
        usedMaterial.SetColor("_EmissionColor", Color.black);
    }
}
