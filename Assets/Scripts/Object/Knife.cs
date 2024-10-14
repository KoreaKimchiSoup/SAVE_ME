using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Knife : MonoBehaviour
{
    // 충돌 횟수 추적 변수
    private int collisionCount = 0;
    // TrimmedWoodStick 프리팹 참조
    public GameObject trimmedWoodStickPrefab;

    // 나뭇가지와 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체의 태그가 "WoodStick" 인지 확인
        if (collision.gameObject.CompareTag("WoodStick"))
        {
            // 충돌 횟수 증가
            collisionCount++;
            if (collisionCount == 3)
            {
                // WoodStick 인스턴스 삭제
                ObjectManager.instance.DestroyObject(collision.gameObject);

                // TrimmedWoodStick 생성
                GameObject trimmedWoodStick = Instantiate(trimmedWoodStickPrefab, collision.transform.position, Quaternion.identity);

                // 충돌 횟수 초기화
                collisionCount = 0;
            }
        }
    }
}
