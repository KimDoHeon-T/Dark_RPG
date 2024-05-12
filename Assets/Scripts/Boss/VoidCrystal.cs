using StarterAssets;
using System.Collections;
using UnityEngine;

public class VoidCrystal : MonoBehaviour
{
    private GameObject obj;
    private BoxCollider[] boxColliders = new BoxCollider[4];
    void OnEnable()
    {
        boxColliders = GetComponents<BoxCollider>();
        StartCoroutine("LifeTime");
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(0.09f);
        boxColliders[1].enabled = true;
        yield return new WaitForSeconds(0.09f);
        boxColliders[2].enabled = true;
        yield return new WaitForSeconds(0.1f);
        boxColliders[3].enabled = true;
        yield return new WaitForSeconds(1.32f);
        if (obj != null)
        {
            obj.GetComponent<ThirdPersonController>().SlowEnd();
        }
        gameObject.SetActive(false);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        obj = other.gameObject;
        if (other.name == "PlayerArmature")
        {
            other.GetComponent<ThirdPersonController>().SlowStart();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerArmature")
        {
            other.GetComponent<ThirdPersonController>().SlowEnd();
        }
    }
}
