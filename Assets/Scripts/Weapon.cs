using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Magic };
    public Type type;
    public int weaponType;//�Ŀ� Data�� nowWeapon�� weapon���� �ٲٱ� ���� ����
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");//���� �߻� ����
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing()//IEnumerator: ������ �Լ�
    {//yield: ��� ���� Ű����, null�̸� 1�������� ����ϱ� ������ �������� ����Ͽ� �ð��� ���� �ۼ� ����
        yield return new WaitForSeconds(0.1f); //0.1�� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);


        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        trailEffect.enabled = false;

        yield break;//�ڷ�ƾ Ż��
    }
    //�Ϲ��Լ� ���η�ƾ -> �����ƾ -> ���η���
    //�ڷ�ƾ ���η�ƾ + �ڷ�ƾ(���ý���)


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "monster")//���͸� Ÿ������ ���
        {
            Monster mob = other.gameObject.GetComponent<Monster>();
            MonsterAI mobAI = mob.GetComponent<MonsterAI>();
            mob.hp -= Data.data.atkPower;
            Animator mobAnim = mob.GetComponent<Animator>();
            if (mobAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                mobAnim.SetTrigger("Back Hit");
            }
            mobAnim.SetTrigger("Hit");
            mobAI.AttackEnd();
            mob.GetComponent<AudioSource>().Play();
            mobAI.agent.isStopped = true;
            mobAI.atkCoolTime += mobAI.atkLength + 0.5f;
            mobAI.atkCoolTime = 0;
            Debug.Log(mob.hp);
        }
        else if (other.tag == "FirstBoss")
        {
            if (other.gameObject.GetComponent<BossPattern>() != null && other.gameObject.GetComponent<BossPattern>().enabled)
            {
                other.gameObject.GetComponent<BossPattern>().shield -= Data.data.atkPower;
            }
            else if (other.gameObject.GetComponent<SecondPattern>() != null && other.gameObject.GetComponent<SecondPattern>().enabled)
            {
                other.gameObject.GetComponent<SecondPattern>().hp -= Data.data.atkPower;
            }
        }
    }
}
