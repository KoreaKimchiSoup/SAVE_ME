using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // 컨트롤러 설명서 UI
    public GameObject controllerUI;
    // 게임 메뉴 UI
    public GameObject gameMenuUI;
    // 제작 재료 레시피 UI
    public GameObject howToMakeMaterials;
    // 도구 레시피 UI
    public GameObject howToMakeTools;
    // 아이템 레시피 UI
    public GameObject howToMakeItems;
    // 시계 상태 전환용 컨트롤러 키 매핑
    public InputActionReference switchWindowMapping;
    // 게임 메뉴 키 매핑
    public InputActionReference gameMenuMapping;
    // 플레이어의 위치값
    public Transform playerPos;
    // 매핑된 시계 키를 누른 횟수
    private int touchcount = 0;
    // 재료 레시피 토글용
    private bool isOnCraftMaterialRecipe = true;
    // 아이템 레시피 토글용
    private bool isOnCraftItemRecipe = true;
    // 메뉴 토글용
    private bool isMenu = true;
    // ControllerUI 토글용
    private bool isController = true;
    // 아이템 제작 UI 토글용
    private bool isItems = true;

    // 컨트롤러 UI 토글
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

    // 재료 제작법을 토글함
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

    // 아이템 UI 토글용
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

    // 아이템 제작법을 토글함
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

    // 게임 메뉴 toggle
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
    //클릭 시 화면이 바뀌게 하는 로직
    public void SwitchWindows(InputAction.CallbackContext obj)
    {
        if (gameObject != null)
        {   //처음 클릭시 상태창 => 시계 창
            if (touchcount == 0)
            {
                transform.Find("PlayerState").gameObject.SetActive(false);
                touchcount++;
            }
            //다음 클릭시 시계 => 나침반 화면
            else if (touchcount == 1)
            {
                transform.Find("Clock").gameObject.SetActive(false);
                touchcount++;
            }
            //마지막 클릭시 다시 나침반 => 상태 창
            else if (touchcount == 2)
            {
                transform.Find("PlayerState").gameObject.SetActive(true);
                transform.Find("Clock").gameObject.SetActive(true);
                touchcount -= 2;
            }
        }
    }
    // 플레이어가 사망 시 시계에 띄워줄 이미지
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
    // 플레이어가 바라보는 방향에서 생성되는 함수
    public void LookPlayer(GameObject obj)
    {
        // UI를 카메라 앞쪽으로 이동 및 회전
        float offsetDistance = 0.3f; // 필요한 거리로 설정
        obj.transform.position = playerPos.position + playerPos.forward * offsetDistance;
        obj.transform.rotation = Quaternion.LookRotation(playerPos.forward);
    }
}

// 인벤토리 페이지 이동 스크립트들
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

// 현재 페이지 로드
//private void UpdateInventoryPage()
//{
//    for (int i = 0; i < inventoryPages.Length; i++)
//    {
//        inventoryPages[i].SetActive(i == currentPageIndex);
//        LookPlayer(inventoryPages[i]);
//        LookPlayer(nextPageArrow);
//    }
//}