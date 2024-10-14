using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // ��Ʈ�ѷ� ���� UI
    public GameObject controllerUI;
    // ���� �޴� UI
    public GameObject gameMenuUI;
    // ���� ��� ������ UI
    public GameObject howToMakeMaterials;
    // ���� ������ UI
    public GameObject howToMakeTools;
    // ������ ������ UI
    public GameObject howToMakeItems;
    // �ð� ���� ��ȯ�� ��Ʈ�ѷ� Ű ����
    public InputActionReference switchWindowMapping;
    // ���� �޴� Ű ����
    public InputActionReference gameMenuMapping;
    // �÷��̾��� ��ġ��
    public Transform playerPos;
    // ���ε� �ð� Ű�� ���� Ƚ��
    private int touchcount = 0;
    // ��� ������ ��ۿ�
    private bool isOnCraftMaterialRecipe = true;
    // ������ ������ ��ۿ�
    private bool isOnCraftItemRecipe = true;
    // �޴� ��ۿ�
    private bool isMenu = true;
    // ControllerUI ��ۿ�
    private bool isController = true;
    // ������ ���� UI ��ۿ�
    private bool isItems = true;

    // ��Ʈ�ѷ� UI ���
    public void controllerUIOff()
    {
        if (isController)
        {
            isController = false;
            controllerUI.SetActive(true);

            LookPlayer(controllerUI);
        }
        else
        {
            controllerUI.SetActive(false);
            isController = true;
        }
    }

    // ��� ���۹��� �����
    public void OnCraftMaterialRecipe()
    {
        if (isOnCraftMaterialRecipe)
        {
            isOnCraftMaterialRecipe = false;
            howToMakeMaterials.SetActive(true);
            LookPlayer(howToMakeMaterials);
        }
        else
        {
            howToMakeMaterials.SetActive(false);
            isOnCraftMaterialRecipe = true;
        }
    }

    // ������ UI ��ۿ�
    public void OnCraftItemRecipe()
    {
        if (isItems)
        {
            isItems = false;
            howToMakeItems.SetActive(true);
            LookPlayer(howToMakeItems);
        }
        else
        {
            howToMakeItems.SetActive(false);
            isItems = true;
        }
    }

    // ������ ���۹��� �����
    public void OnIToolsRecipe()
    {
        if (isOnCraftItemRecipe)
        {
            isOnCraftItemRecipe = false;
            howToMakeTools.SetActive(true);
            LookPlayer(howToMakeTools);
        }
        else
        {
            howToMakeTools.SetActive(false);
            isOnCraftItemRecipe = true;
        }
    }

    // ���� �޴� toggle
    public void GameMenuPopup(InputAction.CallbackContext context)
    {
        Debug.Log($"isMenu: {isMenu}");

        if (isMenu)
        {
            isMenu = false;
            gameMenuUI.SetActive(true);
            LookPlayer(gameMenuUI);
        }
        else
        {
            gameMenuUI.SetActive(false);
            isMenu = true;
        }
    }
    //Ŭ�� �� ȭ���� �ٲ�� �ϴ� ����
    public void SwitchWindows(InputAction.CallbackContext obj)
    {
        if (gameObject != null)
        {   //ó�� Ŭ���� ����â => �ð� â
            if (touchcount == 0)
            {
                transform.Find("PlayerState").gameObject.SetActive(false);
                touchcount++;
            }
            //���� Ŭ���� �ð� => ��ħ�� ȭ��
            else if (touchcount == 1)
            {
                transform.Find("Clock").gameObject.SetActive(false);
                touchcount++;
            }
            //������ Ŭ���� �ٽ� ��ħ�� => ���� â
            else if (touchcount == 2)
            {
                transform.Find("PlayerState").gameObject.SetActive(true);
                transform.Find("Clock").gameObject.SetActive(true);
                touchcount -= 2;
            }
        }
    }
    // �÷��̾ ��� �� �ð迡 ����� �̹���
    public void DieImage()
    {
        transform.Find("Die").gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        switchWindowMapping.action.performed += SwitchWindows;
        gameMenuMapping.action.performed += GameMenuPopup;
    }
    private void OnDisable()
    {
        switchWindowMapping.action.performed -= SwitchWindows;
        gameMenuMapping.action.performed -= GameMenuPopup;
    }
    // �÷��̾ �ٶ󺸴� ���⿡�� �����Ǵ� �Լ�
    public void LookPlayer(GameObject obj)
    {
        // UI�� ī�޶� �������� �̵� �� ȸ��
        float offsetDistance = 0.3f; // �ʿ��� �Ÿ��� ����
        obj.transform.position = playerPos.position + playerPos.forward * offsetDistance;
        obj.transform.rotation = Quaternion.LookRotation(playerPos.forward);
    }
}

// �κ��丮 ������ �̵� ��ũ��Ʈ��
//public void ShowPreviousPage()
//{
//    if (currentPageIndex == 0)
//    {
//        currentPageIndex = inventoryPages.Length - 1;
//    }
//    else
//    {
//        currentPageIndex--;
//    }

//    UpdateInventoryPage();
//}

//public void ShowNextPage()
//{
//    if (currentPageIndex == inventoryPages.Length - 1)
//    {
//        currentPageIndex = 0;
//    }
//    else
//    {
//        currentPageIndex++;
//    }

//    UpdateInventoryPage();
//}

// ���� ������ �ε�
//private void UpdateInventoryPage()
//{
//    for (int i = 0; i < inventoryPages.Length; i++)
//    {
//        inventoryPages[i].SetActive(i == currentPageIndex);
//        LookPlayer(inventoryPages[i]);
//        LookPlayer(nextPageArrow);
//    }
//}