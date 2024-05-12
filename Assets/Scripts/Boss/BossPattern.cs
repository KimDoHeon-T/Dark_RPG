using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossPattern : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject playerCharacter;
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

    public int phase = 1;
    private FirstBossAnim FBA;
    private float atkLen;
    private float lastTime;

    public float shield = 20;
    [SerializeField] private GameObject shieldpar;
    [SerializeField] private GameObject shieldbreak;

    [SerializeField] private GameObject voidOrb;
    [SerializeField] private GameObject voidThunder;

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
        atkLen = FBA.animLen[FBA.nowAtkNum];
        agent.isStopped = true;
    }


    private void Update()
    {
        FBA.animator.SetFloat("Shield", shield);
        lastTime = atkLen;
        atkLen -= Time.deltaTime;
        if (shield <= 0)
        {
            shieldpar.SetActive(false);
            shieldbreak.SetActive(true);
        }
        if (lastTime > 0 && atkLen < 0)
        {
            transform.LookAt(player.position);
            if (shield > 0)
            {
                agent.isStopped = false;
                FBA.isAtk = false;
                FBA.animator.SetTrigger("AtkEnd");
                StartCoroutine("PlayerTracking");
            }
            else
            {
                FBA.animator.SetTrigger("AtkEnd");
            }
        }
    }

    public void LookPlayer()
    {
        transform.LookAt(player.position);
    }

    public void Thunder()
    {
        voidThunder.SetActive(true);
    }
    public void OrbOn()
    {
        voidOrb.SetActive(true);
    }
    public void KnuckBack()
    {
        playerCharacter.GetComponent<ThirdPersonController>().KnuckBack(transform.position, 5);
    }
}