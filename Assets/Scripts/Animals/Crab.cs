using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab :WeakAnimal
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

        int _random = Random.Range(0, 2); //idel, walk

        if (_random == 0) //0�϶� Idel ����
        {
            Idel();
        }
        else if (_random == 1)//1�϶� Walk ����
        {
            Walk();
        }
    }
}
