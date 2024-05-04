using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    Transform _transform;
    Vector3 direction;
    float CoolTime = 0;
    private void Start()
    {
        _transform = GetComponent<Transform>();
        direction = _transform.forward;
        StartCoroutine("Swing");
    }

    IEnumerator Swing()
    {
        while (true)
        {
            _transform.Translate(direction * 1 * Time.deltaTime);
            CoolTime += Time.deltaTime;
            if (CoolTime > 3)
            {
                direction = -direction;
                CoolTime = 0;
            }
            yield return null;
        }
    }
}
