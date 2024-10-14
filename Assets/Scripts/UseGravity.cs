using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UseGravity : MonoBehaviour
{
    // 플레이어가 덩굴을 잡으면 gravity를 사용함
    private Rigidbody rb;
    public XRDirectInteractor interacter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (interacter != null)
        {
            // XRDirectInteractor 컴포넌트에 함수를 추가한다
            interacter.selectExited.AddListener(HandleSelectExited);
        }
    }

    // 플레이어가 덩굴을 Grab 했을 때 실행할 함수
    private void HandleSelectExited(SelectExitEventArgs arg)
    {
        rb.useGravity = true;
    }
}
