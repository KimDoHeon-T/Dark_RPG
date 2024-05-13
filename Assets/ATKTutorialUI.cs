using System.Collections;
using UnityEngine;

public class ATKTutorialUI : MonoBehaviour
{
    public GameObject obj;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerArmature")
        {
            obj.SetActive(true);
            StartCoroutine("LifeTime");
        }

    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSecondsRealtime(5f);
        Destroy(obj);
        Destroy(gameObject);
    }
}