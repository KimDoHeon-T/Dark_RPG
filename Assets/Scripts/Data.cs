using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour//Mono��¼�� ����� ���Ϸν� �����ϴ°� ��ǥ
{
    public static Data data;
    public List<string> KnuckleCombo;
    public List<string> SwordCombo;
    public List<string> SpearCombo;
    public List<List<string>> ComboList;
    public int nowWeapon;//���� �������� ���� Ÿ��
    public int atkPower;
    public GameObject equipWeapon;//���� �������� ����, �Ŀ� nowWeapon�� ���ְ� �� ��ü�� �ʵ� ���
    private void Awake()
    {
        data = this;
        data.SwordCombo = new List<string>();
        data.ComboList = new List<List<string>>();
        data.ComboList.Add(KnuckleCombo);
        data.ComboList.Add(SwordCombo);
        data.ComboList.Add(SpearCombo);
        data.KnuckleCombo.Add("0000");
        data.SwordCombo.Add("0100");
        data.SpearCombo.Add("0200");
        data.atkPower = 10;
        //data.SwordCombo.Clear();//���߿� �ʱ�ȭ �ڵ�
    }
}
