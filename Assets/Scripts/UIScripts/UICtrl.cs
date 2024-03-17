using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject[] ProfCanArray = new GameObject[2];
    [SerializeField] private GameObject[] SwordTypeArray = new GameObject[2];
    private GameObject[][] CanArray = new GameObject[2][];
    private int canNum;

    [SerializeField] private Sprite[] SwordAtkImage = new Sprite[2];
    private Sprite[][] AtkImgArray = new Sprite[1][];
    [SerializeField] private Sprite nullSprite;

    public GameObject[] ComboSlots = new GameObject[3];//�޺� �����
    public List<String> SwordCombo = new List<String>();


    //test
    public ComboAnimCtrl comboAnimCtrl;

    private void Awake()
    {
        CanArray[0] = ProfCanArray;
        CanArray[1] = SwordTypeArray;
        AtkImgArray[0] = SwordAtkImage;
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
    }

    private void MainMenuCtrl()//���θ޴� ��Ʈ��, ������ ���õ�â �����̴�. �Ŀ� ���θ޴��� ���� ���� �ʿ�
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
        }
    }

    public void Btnclick(GameObject can)
    {

        foreach (GameObject[] array in CanArray)
        {
            if (Array.Exists(array, x => x == can))
            {
                canNum = Array.IndexOf(CanArray, array);
            }
        }
        foreach (GameObject obj in CanArray[canNum])//Prof Canvas ���� ����
        {
            obj.SetActive(false);
        }
        can.SetActive(true);//���õ� Canvas�� Ȱ��ȭ
        ComboSlots = GameObject.FindGameObjectsWithTag("ComboSlot");
    }

    public void AddCombo(String code)//�޺� �߰�
    {
        Image img = ComboSlots[Data.data.SwordCombo.Count].GetComponent<Image>();//���Կ��� �̹��� ��������
        int weaponNum = int.Parse(code.Substring(0, 1));//�ڵ忡�� ���� ����
        int atkNum = int.Parse(code.Substring(2));//�ڵ忡�� ��� ����
        img.sprite = AtkImgArray[weaponNum][atkNum];//���� �̹��� ����
        if (Data.data.SwordCombo.Count < 3)
        {
            Data.data.SwordCombo.Add(code);//�޺� ����Ʈ�� ��� �߰�
        }
    }

    public void ClearCombo()//�޺� ����
    {
        foreach (GameObject comboSlots in ComboSlots)
        {
            comboSlots.GetComponent<Image>().sprite = nullSprite;
        }
        Data.data.SwordCombo.Clear();
    }
}