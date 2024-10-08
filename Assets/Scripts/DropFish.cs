using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFish : MonoBehaviour
{
    // ����� ������Ʈ
    public GameObject[] fishes;
    // �÷��̾��� ��ġ��
    public Transform playerPos;
    // �÷��̾�κ����� ����� ��ġ
    private Vector3 fishPosOffset = new Vector3(0, 3, 0.5f);

    private void Awake()
    {
        fishes[0].SetActive(false);
        fishes[1].SetActive(false);
    }

    public void isFishingSuccess()
    {
        Debug.Log("checkHaptic�� true�� �ٲ�!");
        CreateFishes();

        // ù ��°, �� ��° ������ 50������ Ȯ���� ����
    }

    private void CreateFishes()
    {
        int fishIndex = Random.Range(0, 3);

        switch (fishIndex)
        {
            case 0:
                SetActiveFish(fishIndex);
                break;
            case 1:
                SetActiveFish(fishIndex);
                break;
            default:
                Debug.Log("��");
                break;
        }
    }

    private void SetActiveFish(int index)
    {
        fishes[index].SetActive(true);

        // �÷��̾��� ���� ���⿡ ����� ������Ʈ ����
        Vector3 forwardDirection = Camera.main.transform.forward;
        // ���� ī�޶�(�÷��̾� ����)�� ���� ����
        Vector3 spawnPosition = fishes[index].transform.position + forwardDirection + fishPosOffset;

        // ������� ��ġ�� ����
        fishes[index].transform.position = spawnPosition;
    }
}
