using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPattern : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] Anchors = new Transform[4];
    [SerializeField] private FirstBossSecondPhaseAnim FBSA;

    private float distance;
    private float[] AtkLen = new float[3];

    [SerializeField] private GameObject[] Magics = new GameObject[3];
    private List<GameObject> VoidLightning = new List<GameObject>();

    private bool isAtk = false;
    private int atkNum = 0;

    private bool isAlive = true;


    private void Start()
    {
        FBSA = GetComponent<FirstBossSecondPhaseAnim>();
        GetComponent<CapsuleCollider>().enabled = true;
        AtkLen[0] = 3.0f;
        AtkLen[1] = 4.0f;
        AtkLen[2] = 5.0f;
        StartCoroutine("Teleportation");
        StartCoroutine("CalDistance");
    }

    IEnumerator Teleportation()
    {
        while (isAlive)
        {
            transform.position = Anchors[Random.Range(0, 4)].position;
            yield return new WaitForSeconds(0.01f);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            isAtk = true;
            if (distance < 6f)
            {
                atkNum = 0;
            }
            else if (distance >= 6.0f && distance < 15.0f)
            {
                atkNum = 1;
            }
            else
            {
                atkNum = 2;
            }
            FBSA.animator.SetInteger("AtkNum", atkNum);
            Attack();
            Debug.Log(atkNum);
            yield return new WaitForSeconds(AtkLen[atkNum]);
        }
    }

    IEnumerator CalDistance()
    {
        while (true)
        {
            distance = Vector3.Distance(transform.position, player.position);
            yield return null;
        }
    }

    public void Attack()
    {
        FBSA.animator.SetTrigger("Atk");
    }

    public void AttackParticle()//애니메이션 이벤트
    {
        switch (atkNum)
        {
            case 0:
                Magics[atkNum].SetActive(true); break;
            case 1:
                Debug.Log("SecondAtk");
                Magics[atkNum].SetActive(true); break;
            case 2:
                VoidLightning.Add(Instantiate(Magics[atkNum].gameObject));
                VoidLightning[VoidLightning.Count - 1].transform.position = player.position + new Vector3(0, 0.01f, 0); break;
        }

    }
}
