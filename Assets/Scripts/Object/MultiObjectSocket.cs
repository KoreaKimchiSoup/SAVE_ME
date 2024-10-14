using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MultiObjectSocket : XRSocketInteractor
{
    // ���Ͽ� ���� ������Ʈ���� ������ ����Ʈ
    public List<XRBaseInteractable> interactables = new List<XRBaseInteractable>();

    protected override void Awake()
    {
        base.Awake();
        // ���Ͽ� ������Ʈ�� ���� �� �̺�Ʈ ������ �߰�
        selectEntered.AddListener(OnSelectEnter);
        // ���Ͽ��� ������Ʈ�� ���� �� �̺�Ʈ ������ �߰�
        selectExited.AddListener(OnSelectExit);
    }

    // ������Ʈ�� ���Ͽ� ���� �� ȣ��Ǵ� �Լ�
    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        // ����Ʈ�� �ش� ������Ʈ�� ������ �߰�
        if (!interactables.Contains(args.interactable))
        {
            interactables.Add(args.interactable);
            Debug.Log("Object added to socket: " + args.interactable.name);
        }
    }

    // ������Ʈ�� ���Ͽ��� ���� �� ȣ��Ǵ� �Լ�
    private void OnSelectExit(SelectExitEventArgs args)
    {
        // ����Ʈ�� �ִ� ������Ʈ��� ����
        if (interactables.Contains(args.interactable))
        {
            interactables.Remove(args.interactable);
            Debug.Log("Object removed from socket: " + args.interactable.name);
        }
    }

    // ���Ͽ� ������Ʈ�� �߰��� �� �ִ��� Ȯ���ϴ� �Լ�
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // ���ϴ� ������ ���� ������Ʈ ���� ������ �� ����
        return true;
    }

    public void CreateRockAxe()
    {
        int trimmedWoodStick = 0;
        int trimmedRock = 0;
        int vine = 0;

        for (int i = 0; i < interactables.Count; i++)
        {
            if (interactables[i].CompareTag("TrimmedWoodStick"))
            {
                trimmedWoodStick++;
            }
            else if (interactables[i].CompareTag("TrimmedRock"))
            {
                trimmedRock++;
            }
            else if (interactables[i].CompareTag("Vine"))
            {
                vine++;
            }
        }

        if (trimmedWoodStick == 2 && trimmedRock == 2 && vine == 1)
        {
            // ������ ����
            Debug.Log("������ ����!");

            // ���տ� ���� ��� ������ �ı�
        }
        else
        {
            Debug.Log("�����ǿ� ���� �����Դϴ�.");
        }
    }
}
