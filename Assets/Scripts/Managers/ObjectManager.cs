using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //���ӸŴ����� �ν��Ͻ��� ��� ��������
    //�� ���� ������ ���ӸŴ��� �ν��Ͻ��� �� instance�� ��� �༮�� �����ϰ� �� ���̴�.
    public static ObjectManager instance = null;

    // �������� ���� ������
    public GameObject woodStick;
    // �������� ���纻
    private GameObject treeBranchInstance;
    // �������� ���� ��ġ
    public Transform treeBranchPos;
    // �������� �ν��Ͻ��� ��� ����Ʈ
    public List<GameObject> listWoodStick = new List<GameObject>();

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
    public static ObjectManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // �������� ���纻 ������Ʈ�� �θ� ������Ʈ
        GameObject treeBranchParentObject = new GameObject();
        treeBranchParentObject.transform.name = "TreeBranchParentObject";

        // ������ ����
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(5f, 5f));
        // ������Ʈ�� ������ ��ġ
        Vector3 treeBranchSpawnPosition = treeBranchPos.position + randomOffset;

        for (int i = 0; i < 5; i++)
        {
            // ������ ���纻 ����
            treeBranchInstance = Instantiate(woodStick, treeBranchSpawnPosition, Quaternion.identity);
            // ����Ʈ�� �߰�
            listWoodStick.Add(treeBranchInstance);
            // ������ ���纻�� �θ� ������Ʈ�� �Ҵ�
            treeBranchInstance.transform.SetParent(treeBranchParentObject.transform);
        }
    }

    public void DestroyObject(GameObject obj)
    {
        if (listWoodStick.Contains(obj))
        {
            // ��Ͽ��� ��ü ����
            listWoodStick.Remove(obj);
            // ��ü ����
            Destroy(obj);
        }
    }
}
