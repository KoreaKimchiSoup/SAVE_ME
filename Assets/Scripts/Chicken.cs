using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken :Animal
{
    protected override void ResetAnim()
    {
        base.ResetAnim();
        RandomAction();
    }

    //랜덤 동작
    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 3); //idle, walk, Eat 

        if (_random == 0) //0일때 Idel 동작
        {
            Idel();
        }
        else if (_random == 1)//1일때 Walk 동작
        {
            Walk();
        }
        else if (_random == 2) //2일때 Eat 동작
        {
            Eat();
        }
    }
}
