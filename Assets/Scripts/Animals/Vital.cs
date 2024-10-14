using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vital : MonoBehaviour
{
    [SerializeField] private BoxCollider vitalColider; //�޼� �ݶ��̴� 
    Animal animal; //������ �פ��� �ϱ����ؼ� 
    private void Awake()
    {
     //�ٸ������� Die�� ����Ǵ°� ���� ���� �θ��� Animal�� ������
      animal= this.GetComponentInParent<Animal>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("WEAPON"))
        {
            Debug.Log("�޼� ����");
            //���
            animal.Die();
            if (animal.isDead)
            {
                vitalColider.gameObject.SetActive(false);
            }
        }
    }
   

}
