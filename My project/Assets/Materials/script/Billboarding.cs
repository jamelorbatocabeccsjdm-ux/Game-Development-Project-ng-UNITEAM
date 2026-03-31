using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    List<Transform> objToBill = new List<Transform>();

    void Awake()
    {
        GameObject[] env = GameObject.FindGameObjectsWithTag("Environment");

        foreach(GameObject envi in env)
        {
            objToBill.Add(envi.transform);
        }
    }

    void LateUpdate()
    {
        foreach(Transform envi in objToBill)
        {
            Vector3 LateCamPos = Camera.main.transform.position;
            LateCamPos.y = envi.position.y;

            envi.LookAt(LateCamPos);
            envi.Rotate(180f, 180f, 0f);
        }
    }
}
