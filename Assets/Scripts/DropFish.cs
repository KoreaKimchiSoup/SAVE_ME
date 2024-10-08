using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFish : MonoBehaviour
{
    // 물고기 오브젝트
    public GameObject[] fishes;
    // 플레이어의 위치값
    public Transform playerPos;
    // 플레이어로부터의 상대적 위치
    private Vector3 fishPosOffset = new Vector3(0, 3, 0.5f);

    private void Awake()
    {
        fishes[0].SetActive(false);
        fishes[1].SetActive(false);
    }

    public void isFishingSuccess()
    {
        Debug.Log("checkHaptic이 true로 바뀜!");
        CreateFishes();

        // 첫 번째, 두 번째 물고기는 50프로의 확률로 나옴
    }

    private void CreateFishes()
    {
        int fishIndex = Random.Range(0, 3);

        switch (fishIndex)
        {
            case 0:
                SetActiveFish(fishIndex);
                break;
            case 1:
                SetActiveFish(fishIndex);
                break;
            default:
                Debug.Log("꽝");
                break;
        }
    }

    private void SetActiveFish(int index)
    {
        fishes[index].SetActive(true);

        // 플레이어의 전방 방향에 물고기 오브젝트 생성
        Vector3 forwardDirection = Camera.main.transform.forward;
        // 메인 카메라(플레이어 시점)의 전방 벡터
        Vector3 spawnPosition = fishes[index].transform.position + forwardDirection + fishPosOffset;

        // 물고기의 위치를 설정
        fishes[index].transform.position = spawnPosition;
    }
}
