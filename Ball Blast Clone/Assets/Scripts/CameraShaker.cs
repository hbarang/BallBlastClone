using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraShaker : MonoBehaviour
{
    public float shakeDuration;

    public static CameraShaker Instance;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(Instance);
        }
    }

    public void Shake(float shakeMagnitude)
    {
        StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude));
    }

    IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            //float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, originalPosition.y, originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPosition;
    }


}
