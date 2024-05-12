using StarterAssets;
using System.Collections;
using UnityEngine;

public class ThunderDetroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LifeTime");
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerArmature" && !other.GetComponent<ThirdPersonController>().invincibility)
        {
            other.GetComponent<ThirdPersonController>().KnuckBack(transform.position, 5);
        }
    }
}
