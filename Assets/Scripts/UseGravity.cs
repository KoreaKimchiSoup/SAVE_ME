using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UseGravity : MonoBehaviour
{
    // �÷��̾ ������ ������ gravity�� �����
    private Rigidbody rb;
    public XRDirectInteractor interacter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (interacter != null)
        {
            // XRDirectInteractor ������Ʈ�� �Լ��� �߰��Ѵ�
            interacter.selectExited.AddListener(HandleSelectExited);
        }
    }

    // �÷��̾ ������ Grab ���� �� ������ �Լ�
    private void HandleSelectExited(SelectExitEventArgs arg)
    {
        rb.useGravity = true;
    }
}
