using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour//Mono어쩌구 지우고 파일로써 존재하는게 목표
{
    public static Data data;
    public List<string> SwordCombo;
    private void Awake()
    {
        data = this;
        SwordCombo = new List<string>();
        //data.SwordCombo.Clear();//개발용 초기화 코드
    }
}
