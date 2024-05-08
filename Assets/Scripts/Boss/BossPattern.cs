using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossPattern : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask lineOfSightMask; // 시야 체크에 사용할 레이어 마스크
    public float chaseRange = 10.0f;
    public float attackRange = 2.0f;
    public float normalFieldOfView = 110f;
    public float alertedFieldOfView = 270f;
    public float lostSightDelay = 1f;
    //public float lostSightTime = 30f;
    public float searchDuration = 30f;
    public float searchRadius = 5f;
    public float searchInterval = 5f;


    private FirstBossAnim FBA;
    private float atkLen;
    private float lastTime;

    public float hp = 2000;

    private void Start()
    {
        FBA = GetComponent<FirstBossAnim>();
        StartCoroutine("PlayerTracking");
    }

    public void StartTracking()
    {
        StopCoroutine("PlayerTracking");
        StartCoroutine("PlayerTracking");
    }

    private IEnumerator PlayerTracking()
    {
        while (true)
        {
            Debug.Log("추적중");

            if (!FBA.isAtk)
            {
                agent.SetDestination(player.position);
                if (IsPlayerInAttackRange())
                {
                    AttackPlayer();
                }
            }
            else
            {
                transform.LookAt(player.position);
            }
            yield return null;
        }
    }

    private bool IsPlayerInAttackRange()
    {
        bool isInAttackRange = Vector3.Distance(transform.position, player.position) < attackRange;
        //Debug.Log($"Is player in attack range: {isInAttackRange}");
        return isInAttackRange;
    }

    private void AttackPlayer()
    {
        StopCoroutine("PlayerTracking");
        FBA.Attack();
        Debug.Log(FBA.nowAtkNum);
        atkLen = FBA.animLen[FBA.nowAtkNum];
        agent.isStopped = true;
        Debug.Log("Attacking Player!");
    }


    private void Update()
    {
        Debug.Log(atkLen);
        lastTime = atkLen;
        atkLen -= Time.deltaTime;
        if (lastTime > 0 && atkLen < 0)
        {
            agent.isStopped = false;
            FBA.isAtk = false;
            transform.LookAt(player.position);
            //transform.LookAt(transform.position + (transform.position - player.position));
            FBA.animator.SetTrigger("AtkEnd");
            StartCoroutine("PlayerTracking");
        }
    }

    public void LookPlayer()
    {
        transform.LookAt(player.position);
    }
}