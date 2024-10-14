using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public InputActionReference SwitchWindowMapping;
    int touchcount = 0;
    //클릭 시 화면이 바뀌게 하는 로직
    public void SwitchWindows(InputAction.CallbackContext obj)
    {
        if (gameObject != null)
        {   //처음 클릭시 상태창 => 시계 창
            if (touchcount == 0)
            {
                transform.Find("PlayerState").gameObject.SetActive(false);
                touchcount++;
            }
            //다음 클릭시 시계 => 나침반 화면
            else if (touchcount == 1)
            {
                transform.Find("Clock").gameObject.SetActive(false);
                touchcount++;
            }
            //마지막 클릭시 다시 나침반 => 상태 창
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

