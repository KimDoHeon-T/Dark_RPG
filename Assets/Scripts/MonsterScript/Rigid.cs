using UnityEngine;

public class Rigid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Yes");
    }
}
