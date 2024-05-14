using UnityEngine;
using UnityEngine.AI;

public class BossStart : MonoBehaviour
{
    public GameObject obj;
    public GameObject hpBar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerArmature")
        {
            obj.GetComponent<Animator>().SetTrigger("Start");
            obj.GetComponent<NavMeshAgent>().enabled = true;
            obj.GetComponent<BossPattern>().enabled = true;
            obj.GetComponent<FirstBossAnim>().enabled = true;
            obj.GetComponent<SphereCollider>().enabled = true;
            hpBar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerArmature")
        {
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
