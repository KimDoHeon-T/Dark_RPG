using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Magic };
    public Type type;
    public int weaponType;//후에 Data의 nowWeapon을 weapon으로 바꾸기 위한 변수
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");//오류 발생 방지
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing()//IEnumerator: 열거형 함수
    {//yield: 결과 전달 키워드, null이면 1프레임을 대기하기 때문에 여러개를 사용하여 시간차 로직 작성 가능
        yield return new WaitForSeconds(0.1f); //0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);


        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        trailEffect.enabled = false;

        yield break;//코루틴 탈출
    }
    //일반함수 메인루틴 -> 서브루틴 -> 메인루팅
    //코루틴 메인루틴 + 코루틴(동시실행)


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "monster")//몬스터를 타격했을 경우
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
            mobAI.atkCoolTime = 0;
            Debug.Log(mob.hp);
        }
        else if (other.tag == "FirstBoss")
        {
            BossPattern boss = other.gameObject.GetComponent<BossPattern>();
            if (boss.phase == 1)
            {
                boss.shield -= Data.data.atkPower;
            }
            Debug.Log(boss.shield);
        }
    }
}
