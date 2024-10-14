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
            Debug.Log("Enter ȣ��");
            UI_getImage.SetActive(true);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("PLAYER"))
    //    {
    //        Debug.Log("Stay ȣ��");
    //        UI_getImage.transform.LookAt(playerTransform);
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Debug.Log("Exit ȣ��");
            UI_getImage.SetActive(false);
        }
    }

    private void LookAtPlayerUI()
    {
        // �÷��̾ ���ϴ� ���� ���
        //Vector3 direction = playerTransform.position - UI_getImage.transform.position;
        //Quaternion lookPlayerRotation = Quaternion.LookRotation(direction);

        //UI_getImage.transform.rotation = lookPlayerRotation;

    }
}


