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

    private bool playerSpotted = false;
    private float timeSinceLastSeen = 0f;
    private float searchStartTime = 0;
    private Vector3 lastSeenPosition;
    private bool isSearching = false;
    private float timeSinceLastSearchPoint = 0f;
    private float currentFieldOfView;
    private float eyeHeight = 1.8f; // 몬스터의 눈 높이. 실제 모델에 따라 조정이 필요합니다.


    private FirstBossAnim FBA;
    private float atkLen;
    private float lastTime;

    private void Start()
    {
        currentFieldOfView = normalFieldOfView;
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
            yield return new WaitForEndOfFrame();
            if (IsPlayerInView() && HasLineOfSight())
            {
                playerSpotted = true;
                isSearching = false;
                timeSinceLastSeen = 0f;
                lastSeenPosition = player.position;
                agent.SetDestination(player.position);
                currentFieldOfView = alertedFieldOfView;
                //Debug.Log("Player spotted and in line of sight.");
            }
            else
            {
                timeSinceLastSeen += Time.deltaTime;
                //Debug.Log("Searching for player...");

                if (playerSpotted && !isSearching && timeSinceLastSeen > lostSightDelay)//시야에서 사라진 지 lostSightDelay정도 지나면 마지막 발견 위치까지 searchInterval 동안 이동, 탐색 시작
                {
                    isSearching = true;
                    searchStartTime = Time.time;
                    agent.SetDestination(lastSeenPosition);
                }
                else if (playerSpotted && !isSearching && timeSinceLastSeen < lostSightDelay)//시야에서 잠깐 사라진 정도로는 계속 추적 진행
                {
                    lastSeenPosition = player.position;
                    agent.SetDestination(player.position);
                }
                else if (isSearching)//주변 탐색 시작 searchDuration동안 진행
                {
                    if (Time.time - searchStartTime > searchDuration)
                    {
                        playerSpotted = false;
                        isSearching = false;
                        agent.SetDestination(transform.position);
                        currentFieldOfView = normalFieldOfView;
                    }
                    else if (Time.time - timeSinceLastSearchPoint > searchInterval)//searchInterval마다 주변 탐색
                    {
                        MoveToNextSearchPoint();
                    }
                }
            }

            if (IsPlayerInAttackRange() && playerSpotted && HasLineOfSight() && !FBA.isAtk)
            {
                AttackPlayer();
            }
        }
    }

    private void MoveToNextSearchPoint()//마지막 목격 지점 근처로 이동
    {
        Vector3 searchPoint = lastSeenPosition + (Random.insideUnitSphere * searchRadius);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(searchPoint, out hit, searchRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            //Debug.Log("Moving to next search point...");
        }
        timeSinceLastSearchPoint = Time.time;
    }

    private bool IsPlayerInView()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        bool isInView = angle < currentFieldOfView * 0.5f && Vector3.Distance(transform.position, player.position) < chaseRange;
        //Debug.Log($"Is player in view: {isInView}");
        return isInView;
    }

    private bool HasLineOfSight()
    {
        Vector3 eyePosition = transform.position + Vector3.up * 1.6f; // "눈"의 높이로 가정한 위치. 적절히 조정이 필요할 수 있음.
        Vector3[] targetPoints = new Vector3[3];
        targetPoints[0] = player.position + Vector3.up * 1.6f; // 플레이어의 머리 위치 추정
        targetPoints[1] = player.position + Vector3.up * 0.8f; // 플레이어의 중심 위치 추정
        targetPoints[2] = player.position; // 플레이어의 발 위치 (기본 위치)

        foreach (Vector3 targetPoint in targetPoints)
        {
            Vector3 directionToTarget = (targetPoint - eyePosition).normalized;
            float distanceToTarget = Vector3.Distance(eyePosition, targetPoint);

            if (Physics.Raycast(eyePosition, directionToTarget, out RaycastHit hit, distanceToTarget, lineOfSightMask))
            {
                Debug.DrawLine(eyePosition, hit.point, Color.red); // 장애물을 통해 플레이어가 가린 경우
            }
            else
            {
                //Debug.Log("bb");
                // 레이캐스트가 플레이어의 머리, 몸, 발 중 하나라도 감지한 경우
                Debug.DrawRay(eyePosition, directionToTarget * distanceToTarget, Color.green); // 레이캐스트 도달 거리 표시
                return true;
            }
        }

        // 모든 레이캐스트가 플레이어를 감지하지 못했다면
        return false;
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
            FBA.animator.SetTrigger("AtkEnd");
            StartCoroutine("PlayerTracking");
        }
    }
}