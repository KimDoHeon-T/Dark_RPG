using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour//Mono��¼�� ����� ���Ϸν� �����ϴ°� ��ǥ
{
    public static Data data;
    public List<string> KnuckleCombo;
    public List<string> SwordCombo;
    public List<string> SpearCombo;
    public List<List<string>> ComboList;
    public int nowWeapon;
    private void Awake()
    {
        data = this;
        SwordCombo = new List<string>();
        ComboList = new List<List<string>>();
        ComboList.Add(KnuckleCombo);
        ComboList.Add(SwordCombo);
        ComboList.Add(SpearCombo);
        KnuckleCombo.Add("0000");
        SwordCombo.Add("0100");
        SpearCombo.Add("0200");
        //data.SwordCombo.Clear();//���߿� �ʱ�ȭ �ڵ�
    }
}
