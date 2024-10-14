using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGetImage : MonoBehaviour
{
    public GameObject UI_getImage;
    public Transform playerTransform;

    private void Start()
    {
        UI_getImage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            Debug.Log("Enter 호출");
            UI_getImage.SetActive(true);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("PLAYER"))
    //    {
    //        Debug.Log("Stay 호출");
    //        UI_getImage.transform.LookAt(playerTransform);
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Debug.Log("Exit 호출");
            UI_getImage.SetActive(false);
        }
    }

    private void LookAtPlayerUI()
    {
        // 플레이어를 향하는 방향 계산
        //Vector3 direction = playerTransform.position - UI_getImage.transform.position;
        //Quaternion lookPlayerRotation = Quaternion.LookRotation(direction);

        //UI_getImage.transform.rotation = lookPlayerRotation;

    }
}


