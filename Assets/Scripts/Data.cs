using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour//Mono��¼�� ����� ���Ϸν� �����ϴ°� ��ǥ
{
    public static Data data;
    public List<string> SwordCombo;
    public List<string> SpearList;
    public List<List<string>> ComboList;
    private void Awake()
    {
        data = this;
        SwordCombo = new List<string>();
        ComboList = new List<List<string>>();
        ComboList.Add(SwordCombo);
        ComboList.Add(SpearList);
        SwordCombo.Add("0000");
        //data.SwordCombo.Clear();//���߿� �ʱ�ȭ �ڵ�
    }
}
