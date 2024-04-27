using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp;
    private int atkPower;
    private int armor;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("weapon"))
        {
            hp -= Data.data.atkPower;
            Debug.Log(hp);
        }
    }
}
