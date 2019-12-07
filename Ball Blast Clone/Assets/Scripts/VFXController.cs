using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{

    public float lifeTime;

    IEnumerator WaitForDestroy(float delay)
    {

        yield return new WaitForSeconds(delay);
        GetComponent<ParticleSystem>().Clear();
        GameObjectFactory.Instance.ReleaseHitEffect(this.gameObject);
        this.gameObject.SetActive(false);

    }

    public void CallCoroutine()
    {

        StartCoroutine(WaitForDestroy(lifeTime));

    }

}
