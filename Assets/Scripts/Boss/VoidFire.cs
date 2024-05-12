using StarterAssets;
using System.Collections;
using UnityEngine;

public class VoidFire : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine("LifeTime");
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerArmature" && !other.GetComponent<ThirdPersonController>().invincibility)
        {
            other.GetComponent<ThirdPersonController>().KnuckBack(transform.position, 10);
        }
    }
}
