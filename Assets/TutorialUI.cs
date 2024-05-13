using System.Collections;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LifeTime");
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSecondsRealtime(5f);
        Destroy(gameObject);
    }
}
