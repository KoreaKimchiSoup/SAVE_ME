using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Rock : MonoBehaviour
{

    private int collisionCount = 0;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // �̺�Ʈ ����
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Rock"))
        {
            // �� ���� �浹 �߻� �� ī��Ʈ ����
            collisionCount++;
            Debug.Log($"Rock {name} collision count : {collisionCount}");

            // 3�� �浹 �� ������Ʈ ����
            if (collisionCount >= 3)
            {
                // ObjectManager.Instance.RemoveRock(gameObject);
            }
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log($"{name} grabbed");
        // �߰����� ������ �ʿ��� ��� ���⿡ �ۼ�
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        Debug.Log($"{name} released");
        // �߰����� ������ �ʿ��� ��� ���⿡ �ۼ�
    }
}