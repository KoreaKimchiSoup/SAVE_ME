using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public InputActionReference SwitchWindowMapping;
    int touchcount = 0;
    //Ŭ�� �� ȭ���� �ٲ�� �ϴ� ����
    public void SwitchWindows(InputAction.CallbackContext obj)
    {
        if (gameObject != null)
        {   //ó�� Ŭ���� ����â => �ð� â
            if (touchcount == 0)
            {
                transform.Find("PlayerState").gameObject.SetActive(false);
                touchcount++;
            }
            //���� Ŭ���� �ð� => ��ħ�� ȭ��
            else if (touchcount == 1)
            {
                transform.Find("Clock").gameObject.SetActive(false);
                touchcount++;
            }
            //������ Ŭ���� �ٽ� ��ħ�� => ���� â
            else if (touchcount == 2)
            {
                transform.Find("PlayerState").gameObject.SetActive(true);
                transform.Find("Clock").gameObject.SetActive(true);
                touchcount -= 2;
            }
        }
    }
    private void OnEnable()
    {
        SwitchWindowMapping.action.performed += SwitchWindows;
    }

    private void OnDisable()
    {
        SwitchWindowMapping.action.performed -= SwitchWindows;
    }
    public void DieImage()
    {
        transform.Find("Die").gameObject.SetActive(true);
    }
    


}

