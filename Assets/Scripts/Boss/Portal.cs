using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("LifeTime");
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        yield return null;
    }
}
