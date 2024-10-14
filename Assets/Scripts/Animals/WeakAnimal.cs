using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    //���� ��
    private void OnCollisionEnter(Collision collision)
    {
        //���⿡ ������ 
        if (collision.collider.CompareTag("WEAPON") || collision.collider.CompareTag("PREDATOR"))
        {
            {
                if (!isDead)
                {
                    //�� - 100 , ���������  Run ������ Die ȣ��.
                    Hp -= 100;
                    Debug.Log("�¾Ҵ�.");
                    Run(collision.transform.position);
                    //�ǰ� 0���� �϶�
                    if (Hp <= 0)
                    {
                        Die();
                    }
                }
            }
        }
    }
}
