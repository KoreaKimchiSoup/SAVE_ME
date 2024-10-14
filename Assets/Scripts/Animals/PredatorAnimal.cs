using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAnimal : Animal
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
                    Hp -= 20;
                    Debug.Log("�¾Ҵ�.");
                    // ���� ������ �ٶ󺸵��� ȸ�� ����
                    Vector3 directionToHit = (collision.transform.position- transform.position).normalized;
                    destination = Quaternion.LookRotation(directionToHit).eulerAngles;
                    // ���� �������� ȸ��
                    transform.eulerAngles = new Vector3(0, destination.y, 0);
                    Attack();
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
