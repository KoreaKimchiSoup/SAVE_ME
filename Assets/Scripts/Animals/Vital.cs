using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vital : MonoBehaviour
{
    [SerializeField] private BoxCollider vitalColider; //급소 콜라이더 
    Animal animal; //동물을 죽ㄱㅔ 하기위해서 
    private void Awake()
    {
     //다른동물의 Die가 실행되는걸 막기 위해 부모의 Animal을 가져옴
      animal= this.GetComponentInParent<Animal>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("WEAPON"))
        {
            Debug.Log("급소 적중");
            //사망
            animal.Die();
            if (animal.isDead)
            {
                vitalColider.gameObject.SetActive(false);
            }
        }
    }
   

}
