using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
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
                    Hp -= 100;
                    Debug.Log("맞았다.");
                    Run(collision.transform.position);
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
