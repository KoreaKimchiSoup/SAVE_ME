using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Knife : MonoBehaviour
{
    // �浹 Ƚ�� ���� ����
    private int collisionCount = 0;
    // TrimmedWoodStick ������ ����
    public GameObject trimmedWoodStickPrefab;

    // ���������� �浹���� ��
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �±װ� "WoodStick" ���� Ȯ��
        if (collision.gameObject.CompareTag("WoodStick"))
        {
            // �浹 Ƚ�� ����
            collisionCount++;
            if (collisionCount == 3)
            {
                // WoodStick �ν��Ͻ� ����
                ObjectManager.instance.DestroyObject(collision.gameObject);

                // TrimmedWoodStick ����
                GameObject trimmedWoodStick = Instantiate(trimmedWoodStickPrefab, collision.transform.position, Quaternion.identity);

                // �浹 Ƚ�� �ʱ�ȭ
                collisionCount = 0;
            }
        }
    }
}
