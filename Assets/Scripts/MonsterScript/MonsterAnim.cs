using UnityEngine;
using UnityEngine.AI;

public class MonsterAnim : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Nav;
    [SerializeField] private Animator animator;
    private Vector3 vec3;
    private float vel;

    private void Awake()
    {
        Nav.velocity = Vector3.zero;
    }

    private void Update()
    {
        vec3 = Nav.velocity;
        vec3 = vec3.normalized;
        vel = vec3.magnitude;
        animator.SetFloat("Speed", Mathf.Epsilon + vel);
    }
}
