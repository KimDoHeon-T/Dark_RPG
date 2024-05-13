using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private List<BoxCollider> colliders = new List<BoxCollider>();
    private int knuckP;
    // Start is called before the first frame update
    void Start()
    {
        foreach (BoxCollider collider in GetComponents<BoxCollider>())
            colliders.Add(collider);


    }

    public void AttackStart(int power)
    {
        knuckP = power;
        foreach (BoxCollider collider in colliders)
            collider.enabled = true;
    }

    public void AttackEnd()
    {
        foreach (BoxCollider collider in colliders)
            collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerArmature")
        {
            other.GetComponent<ThirdPersonController>().KnuckBack(other.ClosestPointOnBounds(transform.position), knuckP);
        }
    }
}
