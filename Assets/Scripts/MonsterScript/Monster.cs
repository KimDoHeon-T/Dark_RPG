using UnityEngine;

public class Monster : MonoBehaviour
{
    protected int hp;
    private int atkPower;
    private int armor;

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("weapon"))
        {
            hp -= Data.data.atkPower;
            Debug.Log(hp);
        }
    }
}
