using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{

    public float lifeTime;
    void Start()
    {
        StartCoroutine(WaitForDestroy(lifeTime));
    }

    IEnumerator WaitForDestroy(float delay)
    {

        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);

    }

}
