using UnityEngine;

public class XRayController : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public float radius = 1.5f;
    public float softness = 0.5f;

    void Update()
    {
        // Find all renderers in the scene (or optimize by using a radius check)
        // For performance in large games, use a list of nearby obstacles instead
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            // Only apply to materials using our custom shader
            if (sr.sharedMaterial.HasProperty("_CutoutPos"))
            {
                sr.material.SetVector("_CutoutPos", transform.position);
                sr.material.SetFloat("_Radius", radius);
                sr.material.SetFloat("_Softness", softness);
            }
        }
    }
}