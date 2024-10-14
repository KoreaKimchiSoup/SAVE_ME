using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class UseItem : MonoBehaviour
{
    // 오브젝트가 Grab되었는지 참조
    private XRDirectInteractor interacter;

    // 아이템 사용 시 플레이어 상태에 영향을 주기위한 함수
    StateManager stateManager;

    // 오른쪽 컨트롤러에 음식이 있는지 판별하는 변수
    // bool isFoodGrab;
    IXRSelectInteractable grabbedObject;

    [SerializeField]
    //A버튼 매핑하기
    InputActionReference AButton;

    private void Awake()
    {
        interacter = GetComponent<XRDirectInteractor>();
        stateManager = GameObject.Find("Watch").GetComponent<StateManager>();
    }

    void Start()
    {
        if (interacter != null)
        {
            // XRDirectInteractor 컴포넌트에 함수를 추가한다
            interacter.selectEntered.AddListener(HandleSelectEntered);
            interacter.selectExited.AddListener(HandleSelectExited);
        }
        else
        {
            Debug.Log(interacter);
        }
    }

    // 오브젝트가 선택되었을 때 실행하는 함수
    private void HandleSelectEntered(SelectEnterEventArgs arg)
    {
        //isFoodGrab = true;
        grabbedObject = arg.interactableObject;
    }

    // 오브젝트가 선택 해제되었을 때 실행하는 함수
    private void HandleSelectExited(SelectExitEventArgs arg)
    {
       // isFoodGrab = false;
        grabbedObject = null;
    }

    //들고 있는 아이템을 사용하는 함수
    void EatFood(IXRSelectInteractable interactableObject)
    {
        //구운 계란
        if (interactableObject.transform.name == "FiredEgg")
        {
            stateManager.friedEgg();
            Destroy(GameObject.Find("FiredEgg"));
        }
        // 생선 살
        else if (interactableObject.transform.name == "JustFish")
        {
            stateManager.Justfish();
            Destroy(GameObject.Find("JustFish"));
        }
        //구운 생선 살
        else if (interactableObject.transform.name == "FriedFish")
        {
            stateManager.Friedfish();
            Destroy(GameObject.Find("FriedFish"));
        }
        //구운 치킨
        else if (interactableObject.transform.name == "FriedChicken")
        {
            stateManager.FriedChicken();
            Destroy(GameObject.Find("FriedChicken"));
        }
        //구운 사슴 고기
        else if (interactableObject.transform.name == "FriedDeermeat")
        {
            stateManager.FriedDeermeat();
            Destroy(GameObject.Find("FriedDeermeat"));
        }
        //구운 멧돼지 고기
        else if (interactableObject.transform.name == "FriedBoarmeat")
        {
            stateManager.FriedChicken();
            Destroy(GameObject.Find("FriedBoarmeat"));
        }
        //구운 뱀 고기
        else if (interactableObject.transform.name == "FriedSnake")
        {
            stateManager.FriedSnake();
            Destroy(GameObject.Find("FriedSnake"));
        }
        //구운 악어 고기
        else if (interactableObject.transform.name == "FriedElegator")
        {
            stateManager.FriedElegatormeat();
            Destroy(GameObject.Find("FriedElegator"));
        }
        // 문어 
        else if (interactableObject.transform.name == "Octopus")
        {
            stateManager.Octopus();
            Destroy(GameObject.Find("Octopus"));
        }
        //구운 꽃게
        else if (interactableObject.transform.name == "FriedCrab")
        {
            stateManager.friedCrab();
            Destroy(GameObject.Find("FriedCrab"));
        }
        //코코넛
        else if (interactableObject.transform.name == "Coconut")
        {
            stateManager.Coconut();
            Destroy(GameObject.Find("Coconut"));
        }
        //파파야
        else if (interactableObject.transform.name == "Papaya")
        {
            stateManager.Papaya();
            Destroy(GameObject.Find("Papaya"));
        }
        //빗물
        else if (interactableObject.transform.name == "RainWater")
        {
            stateManager.rainWater();
            Destroy(GameObject.Find("RainWater"));
        }
    }

    //오른쪽 컨트롤러 A버튼을 눌렀을 때.
    public void RighT_A(InputAction.CallbackContext context)
    {
        //버튼을 눌렀을 때 
        if (context.performed)
        {
            //isPressA = true;
            if (null != grabbedObject)
            {
                EatFood(grabbedObject);
            }

            else
            {
                print("음식이 없어 먹을 수 없습니다");
            }
        }
    }

    private void OnEnable()
    {
        AButton.action.performed += RighT_A;
        AButton.action.canceled += RighT_A;
    }

    private void OnDisable()
    {
        AButton.action.performed -= RighT_A;
        AButton.action.canceled -= RighT_A;
    }
}
