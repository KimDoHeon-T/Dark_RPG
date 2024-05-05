using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FirstBossAnim : MonoBehaviour
{
    private NavMeshAgent Nav;
    private Animator animator;
    private float vel;
    public int[] animLen;
    public int nowAtkNum;

    private void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animLen = new int[4];
        animLen[0] = 253;
        animLen[1] = 218;
        animLen[2] = 200;
        animLen[3] = 240;
        StartCoroutine("TrackingSpeed");
    }

    private IEnumerator TrackingSpeed()
    {
        while (true)
        {
            vel = Nav.velocity.normalized.magnitude;
            animator.SetFloat("Speed", Mathf.Epsilon + vel);
            yield return null;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("AtkNum", nowAtkNum = Random.Range(1, 5));
    }
}
