using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject[] ProfCanArray = new GameObject[2];
    [SerializeField] private GameObject[] SwordTypeArray = new GameObject[2];
    private GameObject[][] CanArray = new GameObject[2][];
    private int canNum;
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
    private int _nowWeapon = 0;

    [SerializeField] private Animator playerAnimator;



    //test

    private void Awake()
    {
        canNum = 0;
        CanArray[0] = ProfCanArray;
        CanArray[1] = SwordTypeArray;
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
        if (Input.GetKeyUp(KeyCode.Tab))
            EquipmentWindowOpen();

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
            mainMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ComboSlots = GameObject.FindGameObjectsWithTag("ComboSlot");
            ComboRam = Data.data.ComboList[canNum].ToList();
        }
    }

    private void EquipmentWindowOpen()
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
    }

    public void WeaponChange(int weaponNum)
    {
        _nowWeapon = weaponNum;
        playerAnimator.runtimeAnimatorController = animators[weaponNum];
        Debug.Log(_nowWeapon);
    }

    public void Btnclick(GameObject can)
    {

        foreach (GameObject[] array in CanArray)
        {
            if (Array.Exists(array, x => x == can))
            {
                canNum = Array.IndexOf(CanArray, array);
                if (canNum == 0)//숙련도 창에서 무기를 바꾸는 버튼이라면
                {
                    profNum = Array.IndexOf(ProfCanArray, array);
                    ComboRam = Data.data.ComboList[canNum].ToList();
                }
            }
        }
        foreach (GameObject obj in CanArray[canNum])//Prof Canvas 전부 종료
        {
            obj.SetActive(false);
        }
        can.SetActive(true);//선택된 Canvas만 활성화
        ComboSlots = GameObject.FindGameObjectsWithTag("ComboSlot");
    }

    public void AddCombo(String code)//콤보 추가
    {
        if (ComboRam.Count < 3)
        {
            Image img = ComboSlots[ComboRam.Count].GetComponent<Image>();//슬롯에서 이미지 가져오기
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
        if (ComboRam.Count > 0)
            Data.data.SwordCombo = ComboRam.ToList();
        switch (Data.data.SwordCombo.Count)
        {
            case 1:
                animators[_nowWeapon]["FirstAttack"] = AtkAnimArray[_nowWeapon][int.Parse(Data.data.SwordCombo[0].Substring(2))];
                break;
            case 2:
                animators[_nowWeapon]["FirstAttack"] = AtkAnimArray[_nowWeapon][int.Parse(Data.data.SwordCombo[0].Substring(2))];
                animators[_nowWeapon]["SecondAttack"] = AtkAnimArray[_nowWeapon][int.Parse(Data.data.SwordCombo[1].Substring(2))];
                break;
            case 3:
                animators[_nowWeapon]["FirstAttack"] = AtkAnimArray[_nowWeapon][int.Parse(Data.data.SwordCombo[0].Substring(2))];
                animators[_nowWeapon]["SecondAttack"] = AtkAnimArray[_nowWeapon][int.Parse(Data.data.SwordCombo[1].Substring(2))];
                animators[_nowWeapon]["ThirdAttack"] = AtkAnimArray[_nowWeapon][int.Parse(Data.data.SwordCombo[2].Substring(2))];
                break;
        }
    }
}