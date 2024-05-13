using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject[] ProfCanArray = new GameObject[2];//각 무기별 캔버스 저장소
    [SerializeField] private GameObject[] SwordTypeArray = new GameObject[2];//검의 타입별 캔버스 저장소
    [SerializeField] private GameObject[] SpearTypeArray = new GameObject[2];//창의 타입별 캔버스 저장소
    private GameObject[][] CanArray = new GameObject[2][];
    private int canNum = 0;
    private int profNum;

    [SerializeField] private Sprite[] SwordAtkImage = new Sprite[2];
    [SerializeField] private Sprite[] SpearAtkImage = new Sprite[0];
    private Sprite[][] AtkImgArray = new Sprite[2][];
    [SerializeField] private Sprite nullSprite;

    public GameObject[] ComboSlots = new GameObject[3];//콤보 저장용
    public List<String> ComboRam = new List<String>();//콤보 저장 전 일시적 목록(Ram)

    [SerializeField] private AnimatorOverrideController[] animators;

    [Header("Knuckle Animation")]
    [SerializeField] private AnimationClip k0000;
    [SerializeField] private AnimationClip k0001;
    [SerializeField] private AnimationClip k0002;

    [Header("Great Sword Animation")]
    [SerializeField] private AnimationClip g0000;
    [SerializeField] private AnimationClip g0001;
    [SerializeField] private AnimationClip g0002;

    [Header("Spear Animation")]
    [SerializeField] private AnimationClip s0000;
    [SerializeField] private AnimationClip s0001;
    [SerializeField] private AnimationClip s0002;

    private AnimationClip[] KnuckleAnimationArray;
    private AnimationClip[] GreatSwordAnimationArray;
    private AnimationClip[] SpearAnimationArray;

    private AnimationClip[][] AtkAnimArray;//공격애니메이션 배열의 배열

    [SerializeField] private GameObject EquipmentWindow;

    [SerializeField] private Animator playerAnimator;



    //test

    private void Awake()
    {
        CanArray[0] = SwordTypeArray;
        CanArray[1] = SpearTypeArray;
        AtkImgArray[0] = SwordAtkImage;
        AtkImgArray[1] = SpearAtkImage;
        KnuckleAnimationArray = new AnimationClip[] { k0000, k0001, k0002 };
        GreatSwordAnimationArray = new AnimationClip[] { g0000, g0001, g0002 };
        SpearAnimationArray = new AnimationClip[] { s0000, s0001, s0002 };
        AtkAnimArray = new AnimationClip[][] { KnuckleAnimationArray, GreatSwordAnimationArray, SpearAnimationArray };
    }


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            MainMenuCtrl();
        /*if (Input.GetKeyUp(KeyCode.Tab))
            EquipmentWindowOpen();*/

    }

    private void MainMenuCtrl()//메인메뉴 컨트롤, 지금은 숙련도창 전용이다. 후에 메인메뉴는 따로 생성 필요
    {
        if (mainMenu.activeSelf)
        {
            //playerController.uiOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            mainMenu.SetActive(false);
        }
        else
        {
            foreach (GameObject can in ProfCanArray)
            {
                can.SetActive(false);
            }
            ProfCanArray[Data.data.nowWeapon].SetActive(true);
            mainMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ComboSlots = GameObject.FindGameObjectsWithTag("ComboSlot");
            ComboRam = Data.data.ComboList[Data.data.nowWeapon].ToList();
        }
    }

    /*private void EquipmentWindowOpen()
    {
        if (EquipmentWindow.activeSelf)
        {
            //playerController.uiOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            EquipmentWindow.SetActive(false);
        }
        else
        {
            EquipmentWindow.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }*/

    /*public void WeaponChange(int weaponNum)
    {
        Data.data.nowWeapon = weaponNum;
        playerAnimator.runtimeAnimatorController = animators[weaponNum];
        Debug.Log(Data.data.nowWeapon);
    }*/

    public void Btnclick(GameObject can)
    {
        foreach (GameObject typeCan in CanArray[Data.data.nowWeapon])
        {
            typeCan.SetActive(false);
        }
        can.SetActive(true);//선택된 Canvas만 활성화
    }

    public void AddCombo(String code)//콤보 추가
    {
        if (ComboRam.Count < 3)
        {
            Image img = ComboSlots[ComboRam.Count].GetComponent<Image>();//슬롯에서 이미지컴포넌트 가져오기
            int weaponNum = int.Parse(code.Substring(0, 1));//코드에서 무기 추출
            int atkNum = int.Parse(code.Substring(2));//코드에서 기술 추출
            img.sprite = AtkImgArray[weaponNum][atkNum];//슬롯 이미지 변경
            ComboRam.Add(code);//콤보 리스트에 기술 추가
        }
    }

    public void ClearCombo()//콤보 삭제
    {
        foreach (GameObject comboSlots in ComboSlots)
        {
            comboSlots.GetComponent<Image>().sprite = nullSprite;
        }
        ComboRam.Clear();
    }

    public void SaveCombo()
    {
        Debug.Log(ComboRam.Count);
        if (ComboRam.Count > 0)
        {
            Data.data.ComboList[Data.data.nowWeapon] = ComboRam.ToList();
            Debug.Log("alskdjflk");
        }
        switch (Data.data.ComboList[Data.data.nowWeapon].Count)
        {
            case 1:
                animators[Data.data.nowWeapon + 1]["FirstAttack"] = AtkAnimArray[Data.data.nowWeapon + 1][int.Parse(Data.data.ComboList[Data.data.nowWeapon][0].Substring(2))];
                break;
            case 2:
                animators[Data.data.nowWeapon + 1]["FirstAttack"] = AtkAnimArray[Data.data.nowWeapon + 1][int.Parse(Data.data.ComboList[Data.data.nowWeapon][0].Substring(2))];
                animators[Data.data.nowWeapon + 1]["SecondAttack"] = AtkAnimArray[Data.data.nowWeapon + 1][int.Parse(Data.data.ComboList[Data.data.nowWeapon][1].Substring(2))];
                break;
            case 3:
                animators[Data.data.nowWeapon + 1]["FirstAttack"] = AtkAnimArray[Data.data.nowWeapon + 1][int.Parse(Data.data.ComboList[Data.data.nowWeapon][0].Substring(2))];
                animators[Data.data.nowWeapon + 1]["SecondAttack"] = AtkAnimArray[Data.data.nowWeapon + 1][int.Parse(Data.data.ComboList[Data.data.nowWeapon][1].Substring(2))];
                animators[Data.data.nowWeapon + 1]["ThirdAttack"] = AtkAnimArray[Data.data.nowWeapon + 1][int.Parse(Data.data.ComboList[Data.data.nowWeapon][2].Substring(2))];
                break;
        }
    }//+1들은 임시방편
}