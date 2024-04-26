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
}
