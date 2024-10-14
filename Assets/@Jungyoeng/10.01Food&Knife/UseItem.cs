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
    // ������Ʈ�� Grab�Ǿ����� ����
    private XRDirectInteractor interacter;

    // ������ ��� �� �÷��̾� ���¿� ������ �ֱ����� �Լ�
    StateManager stateManager;

    // ������ ��Ʈ�ѷ��� ������ �ִ��� �Ǻ��ϴ� ����
<<<<<<< HEAD
    // bool isFoodGrab;
=======
   // bool isFoodGrab;
>>>>>>> 958cb0710d1caf50c64f1259666055c536bb8b0c
    IXRSelectInteractable grabbedObject;

    [SerializeField]
    //A��ư �����ϱ�
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
            // XRDirectInteractor ������Ʈ�� �Լ��� �߰��Ѵ�
            interacter.selectEntered.AddListener(HandleSelectEntered);
            interacter.selectExited.AddListener(HandleSelectExited);
        }
        else
        {
            Debug.Log(interacter);
        }
    }

<<<<<<< HEAD
=======

>>>>>>> 958cb0710d1caf50c64f1259666055c536bb8b0c
    // ������Ʈ�� ���õǾ��� �� �����ϴ� �Լ�
    private void HandleSelectEntered(SelectEnterEventArgs arg)
    {
        //isFoodGrab = true;
        grabbedObject = arg.interactableObject;
    }

<<<<<<< HEAD
=======

>>>>>>> 958cb0710d1caf50c64f1259666055c536bb8b0c
    // ������Ʈ�� ���� �����Ǿ��� �� �����ϴ� �Լ�
    private void HandleSelectExited(SelectExitEventArgs arg)
    {
       // isFoodGrab = false;
        grabbedObject = null;
    }
<<<<<<< HEAD

=======
>>>>>>> 958cb0710d1caf50c64f1259666055c536bb8b0c
    //��� �ִ� �������� ����ϴ� �Լ�
    void EatFood(IXRSelectInteractable interactableObject)
    {
        //���� ���
        if (interactableObject.transform.name == "FiredEgg")
        {
            stateManager.friedEgg();
            Destroy(GameObject.Find("FiredEgg"));
        }
        // ���� ��
        else if (interactableObject.transform.name == "JustFish")
        {
            stateManager.Justfish();
            Destroy(GameObject.Find("JustFish"));
        }
        //���� ���� ��
        else if (interactableObject.transform.name == "FriedFish")
        {
            stateManager.Friedfish();
            Destroy(GameObject.Find("FriedFish"));
        }
        //���� ġŲ
        else if (interactableObject.transform.name == "FriedChicken")
        {
            stateManager.FriedChicken();
            Destroy(GameObject.Find("FriedChicken"));
        }
        //���� �罿 ���
        else if (interactableObject.transform.name == "FriedDeermeat")
        {
            stateManager.FriedDeermeat();
            Destroy(GameObject.Find("FriedDeermeat"));
        }
        //���� ����� ���
        else if (interactableObject.transform.name == "FriedBoarmeat")
        {
            stateManager.FriedChicken();
            Destroy(GameObject.Find("FriedBoarmeat"));
        }
        //���� �� ���
        else if (interactableObject.transform.name == "FriedSnake")
        {
            stateManager.FriedSnake();
            Destroy(GameObject.Find("FriedSnake"));
        }
        //���� �Ǿ� ���
        else if (interactableObject.transform.name == "FriedElegator")
        {
            stateManager.FriedElegatormeat();
            Destroy(GameObject.Find("FriedElegator"));
        }
        // ���� 
        else if (interactableObject.transform.name == "Octopus")
        {
            stateManager.Octopus();
            Destroy(GameObject.Find("Octopus"));
        }
        //���� �ɰ�
        else if (interactableObject.transform.name == "FriedCrab")
        {
            stateManager.friedCrab();
            Destroy(GameObject.Find("FriedCrab"));
        }
        //���ڳ�
        else if (interactableObject.transform.name == "Coconut")
        {
            stateManager.Coconut();
            Destroy(GameObject.Find("Coconut"));
        }
        //���ľ�
        else if (interactableObject.transform.name == "Papaya")
        {
            stateManager.Papaya();
            Destroy(GameObject.Find("Papaya"));
        }
        //����
        else if (interactableObject.transform.name == "RainWater")
        {
            stateManager.rainWater();
            Destroy(GameObject.Find("RainWater"));
        }
<<<<<<< HEAD
    }

=======


    }
>>>>>>> 958cb0710d1caf50c64f1259666055c536bb8b0c
    //������ ��Ʈ�ѷ� A��ư�� ������ ��.
    public void RighT_A(InputAction.CallbackContext context)
    {
        //��ư�� ������ �� 
        if (context.performed)
        {
            //isPressA = true;
            if (null != grabbedObject)
            {
                EatFood(grabbedObject);
            }

            else
            {
                print("������ ���� ���� �� �����ϴ�");
            }
        }
    }
<<<<<<< HEAD

=======
>>>>>>> 958cb0710d1caf50c64f1259666055c536bb8b0c
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
