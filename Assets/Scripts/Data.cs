using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour//Mono어쩌구 지우고 파일로써 존재하는게 목표
{
    public static Data data;
    public List<string> KnuckleCombo;
    public List<string> SwordCombo;
    public List<string> SpearCombo;
    public List<List<string>> ComboList;
    public int nowWeapon;//현재 장착중인 무기 타입
    public int atkPower;
    public GameObject equipWeapon;//현재 장착중인 무기, 후에 nowWeapon을 없애고 이 객체의 필드 사용
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
        //data.SwordCombo.Clear();//개발용 초기화 코드
    }
}
