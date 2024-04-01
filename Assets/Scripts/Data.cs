using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour//Mono어쩌구 지우고 파일로써 존재하는게 목표
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
        //data.SwordCombo.Clear();//개발용 초기화 코드
    }
}
