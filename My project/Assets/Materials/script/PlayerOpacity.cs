using UnityEngine;
using System.Collections.Generic;

public class SpriteObstructionHandler : MonoBehaviour
{
    public Transform player;
    public LayerMask obstructionLayer;

    [Range(0, 1)]
    public float translucentAlpha = 0.4f;

    private List<SpriteRenderer> obscuredSprites = new List<SpriteRenderer>();

    void Update()
    {
        if (player == null)
            return;

        // 🔥 CAMERA → PLAYER DIRECTION
        Vector3 directionToPlayer = player.position - transform.position;

        // 🔥 DISTANCE TO PLAYER
        float distanceToPlayer = directionToPlayer.magnitude;

        // 🔥 DEBUG RAY CONNECTED TO PLAYER
        Debug.DrawRay(
            transform.position,
            directionToPlayer,
            Color.red
        );

        // 🔥 SPHERE CAST DIRECTLY TO PLAYER
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,          // CAMERA POSITION
            0.5f,                        // THICKNESS
            directionToPlayer.normalized,
            distanceToPlayer,
            obstructionLayer
        );

        List<SpriteRenderer> currentlyHit = new List<SpriteRenderer>();

        foreach (RaycastHit hit in hits)
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

        // 🔥 RESTORE SPRITES NO LONGER BLOCKING
        for (int i = obscuredSprites.Count - 1; i >= 0; i--)
        {
            SpriteRenderer sr = obscuredSprites[i];

            if (!currentlyHit.Contains(sr))
            {
                SetSpriteAlpha(sr, 1f);
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