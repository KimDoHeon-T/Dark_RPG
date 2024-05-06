using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FirstBossAnim : MonoBehaviour
{
    private NavMeshAgent Nav;
    public Animator animator;
    private float vel;
    public float[] animLen;
    public int nowAtkNum;
    public bool isAtk = false;

    private void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animLen = new float[5];
        animLen[0] = 0;
        animLen[1] = 1.9f;
        animLen[2] = 2.0f;
        animLen[3] = 2.0f;
        animLen[4] = 3.3f;
        StartCoroutine("TrackingSpeed");
    }

    private IEnumerator TrackingSpeed()
    {
        while (true)
        {
            if (!isAtk)
            {
                vel = Nav.velocity.normalized.magnitude;
                animator.SetFloat("Speed", Mathf.Epsilon + vel);
            }
            yield return null;
        }
    }

    public void Attack()
    {
        isAtk = true;
        animator.SetFloat("Speed", Mathf.Epsilon);
        animator.SetTrigger("Attack");
        animator.SetInteger("AtkNum", nowAtkNum = Random.Range(1, 5));
    }
}
