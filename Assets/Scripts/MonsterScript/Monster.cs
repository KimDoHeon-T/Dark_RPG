using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp;
    private int atkPower;
    private int armor;


    private void Update()
    {
        if (hp <= 0)
        {
            GetComponent<Animator>().SetTrigger("Death");
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("weapon"))
        {
        }
    }
}
