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

        // 이벤트 구독
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Rock"))
        {
            // 돌 끼리 충돌 발생 시 카운트 증가
            collisionCount++;
            Debug.Log($"Rock {name} collision count : {collisionCount}");

            // 3번 충돌 시 오브젝트 제거
            if (collisionCount >= 3)
            {
                // ObjectManager.Instance.RemoveRock(gameObject);
            }
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log($"{name} grabbed");
        // 추가적인 로직이 필요할 경우 여기에 작성
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        Debug.Log($"{name} released");
        // 추가적인 로직이 필요할 경우 여기에 작성
    }
}