using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MultiObjectSocket : XRSocketInteractor
{
    // 소켓에 들어온 오브젝트들을 저장할 리스트
    public List<XRBaseInteractable> interactables = new List<XRBaseInteractable>();

    protected override void Awake()
    {
        base.Awake();
        // 소켓에 오브젝트가 들어올 때 이벤트 리스너 추가
        selectEntered.AddListener(OnSelectEnter);
        // 소켓에서 오브젝트가 나갈 때 이벤트 리스너 추가
        selectExited.AddListener(OnSelectExit);
    }

    // 오브젝트가 소켓에 들어올 때 호출되는 함수
    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        // 리스트에 해당 오브젝트가 없으면 추가
        if (!interactables.Contains(args.interactable))
        {
            interactables.Add(args.interactable);
            Debug.Log("Object added to socket: " + args.interactable.name);
        }
    }

    // 오브젝트가 소켓에서 나갈 때 호출되는 함수
    private void OnSelectExit(SelectExitEventArgs args)
    {
        // 리스트에 있는 오브젝트라면 제거
        if (interactables.Contains(args.interactable))
        {
            interactables.Remove(args.interactable);
            Debug.Log("Object removed from socket: " + args.interactable.name);
        }
    }

    // 소켓에 오브젝트를 추가할 수 있는지 확인하는 함수
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // 원하는 로직에 따라 오브젝트 수를 제한할 수 있음
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
            // 돌도끼 생성
            Debug.Log("돌도끼 생성!");

            // 조합에 사용된 재료 아이템 파괴
        }
        else
        {
            Debug.Log("레시피에 없는 조합입니다.");
        }
    }
}
