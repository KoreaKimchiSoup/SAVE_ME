using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viper : PredatorAnimal
{
    protected override void ResetAnim()
    {
        base.ResetAnim();
        RandomAction();
    }
    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 3); //idel, glide, hiss

        if (_random == 0) //0�϶� Idel ����
        {
            Idel();
        }
        else if (_random == 1)//1�϶� glide ����
        {
            Walk();
        }
        else if (_random == 2) //2�϶� hiss ����
        {
            Eat();
        }
    }
}
