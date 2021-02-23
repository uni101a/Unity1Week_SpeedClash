using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private GameObject releaseObject = null;

    public GameObject GetReleaseObject()
    {
        return releaseObject;
    }

    public void Reset()
    {
        releaseObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        releaseObject = other.gameObject;
    }
}
