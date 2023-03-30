using System.Collections;
using UnityEngine;

public class RapidSfxTest : MonoBehaviour
{
    [SerializeField]
    private float delay;
    [SerializeField]
    private SfxData sfx;

    private void Start()
    {
        StartCoroutine(RapidSfxCoroutine());
    }

    private IEnumerator RapidSfxCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay * Random.Range(0.9f, 1.1f)); ;
            sfx.Play();
        }
    }
}
