using System.Collections.Generic;
using UnityEngine;

public class PlayerProfSystem : MonoBehaviour
{
    public List<int> weaponProf;//무기 숙련도 리스트 생성
    //knuckle, knife, sword, ...

    //숙련도 리스트 길이 조정
    private void ProfCreate(List<int> list, int num)
    {
        while (weaponProf.Count < num)
        {
            weaponProf.Add(0);
        }
    }

    public void ProfStart()
    {
        weaponProf = new List<int>();
        ProfCreate(weaponProf, 3);
    }


}
