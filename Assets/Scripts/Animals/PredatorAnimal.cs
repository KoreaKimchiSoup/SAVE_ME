using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAnimal : Animal
{
 
    //맞을 때
    private void OnCollisionEnter(Collision collision)
    {
        //무기에 닿으면 
        if (collision.collider.CompareTag("WEAPON") || collision.collider.CompareTag("PREDATOR"))
        {
            {
                if (!isDead)
                {
                    //피 - 100 , 살아있으면  Run 죽으면 Die 호출.
                    Hp -= 20;
                    Debug.Log("맞았다.");
                    // 맞은 방향을 바라보도록 회전 설정
                    Vector3 directionToHit = (collision.transform.position- transform.position).normalized;
                    destination = Quaternion.LookRotation(directionToHit).eulerAngles;
                    // 맞은 방향으로 회전
                    transform.eulerAngles = new Vector3(0, destination.y, 0);
                    Attack();
                    //피가 0이하 일때
                    if (Hp <= 0)
                    {
                        Die();
                    }
                }
            }
        }
    }
}
