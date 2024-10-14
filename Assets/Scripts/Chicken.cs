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

    //���� ����
    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 3); //idle, walk, Eat 

        if (_random == 0) //0�϶� Idel ����
        {
            Idel();
        }
        else if (_random == 1)//1�϶� Walk ����
        {
            Walk();
        }
        else if (_random == 2) //2�϶� Eat ����
        {
            Eat();
        }
    }
}
