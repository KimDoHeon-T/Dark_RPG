using System.Collections.Generic;
using UnityEngine;

public class PlayerProfSystem : MonoBehaviour
{
    public List<int> weaponProf;//���� ���õ� ����Ʈ ����
    //knuckle, knife, sword, ...

    //���õ� ����Ʈ ���� ����
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
