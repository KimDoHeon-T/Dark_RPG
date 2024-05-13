using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask lineOfSightMask; // �þ� üũ�� ����� ���̾� ����ũ
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

    private Animator animator;
    public float atkLength;
    public float atkCoolTime;

    public GameObject weapon;
    private void Start()
    {
        animator = GetComponent<Animator>();
        currentFieldOfView = normalFieldOfView;
    }

    private void Update()
    {
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

            if (playerSpotted && !isSearching && timeSinceLastSeen > lostSightDelay)//�þ߿��� ����� �� lostSightDelay���� ������ ������ �߰� ��ġ���� searchInterval ���� �̵�, Ž�� ����
            {
                isSearching = true;
                searchStartTime = Time.time;
                agent.SetDestination(lastSeenPosition);
                Debug.Log("Lost sight of player, starting search...");
            }
            else if (playerSpotted && !isSearching && timeSinceLastSeen < lostSightDelay)//�þ߿��� ��� ����� �����δ� ��� ���� ����
            {
                lastSeenPosition = player.position;
                agent.SetDestination(player.position);
                Debug.Log("�÷��̾� ���� ���� ��");
            }
            else if (isSearching)//�ֺ� Ž�� ���� searchDuration���� ����
            {
                if (Time.time - searchStartTime > searchDuration)
                {
                    playerSpotted = false;
                    isSearching = false;
                    agent.SetDestination(transform.position);
                    currentFieldOfView = normalFieldOfView;
                    Debug.Log("Search duration exceeded, resetting...");
                }
                else if (Time.time - timeSinceLastSearchPoint > searchInterval)//searchInterval���� �ֺ� Ž��
                {
                    MoveToNextSearchPoint();
                }
            }
        }

        if (IsPlayerInAttackRange() && playerSpotted && HasLineOfSight() && atkCoolTime <= 0)
        {
            AttackPlayer();
        }
        if (atkCoolTime > 0)
        {
            atkCoolTime -= Time.deltaTime;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void MoveToNextSearchPoint()//������ ��� ���� ��ó�� �̵�
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
        Vector3 eyePosition = transform.position + Vector3.up * 1.6f; // "��"�� ���̷� ������ ��ġ. ������ ������ �ʿ��� �� ����.
        Vector3[] targetPoints = new Vector3[3];
        targetPoints[0] = player.position + Vector3.up * 1.6f; // �÷��̾��� �Ӹ� ��ġ ����
        targetPoints[1] = player.position + Vector3.up * 0.8f; // �÷��̾��� �߽� ��ġ ����
        targetPoints[2] = player.position; // �÷��̾��� �� ��ġ (�⺻ ��ġ)

        foreach (Vector3 targetPoint in targetPoints)
        {
            Vector3 directionToTarget = (targetPoint - eyePosition).normalized;
            float distanceToTarget = Vector3.Distance(eyePosition, targetPoint);

            if (Physics.Raycast(eyePosition, directionToTarget, out RaycastHit hit, distanceToTarget, lineOfSightMask))
            {
                Debug.DrawLine(eyePosition, hit.point, Color.red); // ��ֹ��� ���� �÷��̾ ���� ���
            }
            else
            {
                //Debug.Log("bb");
                // ����ĳ��Ʈ�� �÷��̾��� �Ӹ�, ��, �� �� �ϳ��� ������ ���
                Debug.DrawRay(eyePosition, directionToTarget * distanceToTarget, Color.green); // ����ĳ��Ʈ ���� �Ÿ� ǥ��
                return true;
            }
        }

        // ��� ����ĳ��Ʈ�� �÷��̾ �������� ���ߴٸ�
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
        animator.SetTrigger("Attack");
        agent.isStopped = true;
        atkCoolTime += atkLength + 0.5f;
    }

    public void AttackStart(int power)
    {
        weapon.GetComponent<EnemyWeapon>().AttackStart(power);
    }

    public void AttackEnd()
    {
        weapon.GetComponent<EnemyWeapon>().AttackEnd();
    }
}