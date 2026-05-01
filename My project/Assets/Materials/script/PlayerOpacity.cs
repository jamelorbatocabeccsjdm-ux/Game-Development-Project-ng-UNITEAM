using UnityEngine;
using System.Collections.Generic;

public class SpriteObstructionHandler : MonoBehaviour
{
    public Transform player;
    public LayerMask obstructionLayer;
    [Range(0, 1)] public float translucentAlpha = 0.4f;

    private List<SpriteRenderer> obscuredSprites = new List<SpriteRenderer>();

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        Vector3 dir = player.position - transform.position;
        Debug.DrawRay(transform.position, transform.forward * dist, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, dist, obstructionLayer);
        
        List<SpriteRenderer> currentlyHit = new List<SpriteRenderer>();

        foreach (var hit in hits)
        {
            SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                currentlyHit.Add(sr);
                if (!obscuredSprites.Contains(sr))
                {
                    SetSpriteAlpha(sr, translucentAlpha);
                    obscuredSprites.Add(sr);
                }
            }
        }
        for (int i = obscuredSprites.Count - 1; i >= 0; i--)
        {
            SpriteRenderer sr = obscuredSprites[i];
            if (!currentlyHit.Contains(sr))
            {
                SetSpriteAlpha(sr, 1.0f);
                obscuredSprites.RemoveAt(i);
            }
        }
    }

    void SetSpriteAlpha(SpriteRenderer sr, float alpha)
    {
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;
    }
}