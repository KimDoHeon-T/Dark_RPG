using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour//Mono��¼�� ����� ���Ϸν� �����ϴ°� ��ǥ
{
    public static Data data;
    public List<string> SwordCombo;
    private void Awake()
    {
        data = this;
        SwordCombo = new List<string>();
        //data.SwordCombo.Clear();//���߿� �ʱ�ȭ �ڵ�
    }
}
