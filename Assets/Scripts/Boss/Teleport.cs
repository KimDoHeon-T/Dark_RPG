using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform[] Anchors = new Transform[3];
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Teleportation();
        }
    }

    public void Teleportation()
    {
        gameObject.transform.position = Anchors[Random.Range(0, 3)].position;
    }
}
