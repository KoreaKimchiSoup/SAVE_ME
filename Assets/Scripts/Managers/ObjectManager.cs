using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //게임매니저의 인스턴스를 담는 전역변수
    //이 게임 내에서 게임매니저 인스턴스는 이 instance에 담긴 녀석만 존재하게 할 것이다.
    public static ObjectManager instance = null;

    // 나뭇가지 원본 프리팹
    public GameObject woodStick;
    // 나뭇가지 복사본
    private GameObject treeBranchInstance;
    // 나뭇가지 생성 위치
    public Transform treeBranchPos;
    // 나뭇가지 인스턴스가 담길 리스트
    public List<GameObject> listWoodStick = new List<GameObject>();

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
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
        // 나뭇가지 복사본 오브젝트의 부모 오브젝트
        GameObject treeBranchParentObject = new GameObject();
        treeBranchParentObject.transform.name = "TreeBranchParentObject";

        // 랜덤값 생성
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(5f, 5f));
        // 오브젝트가 생성될 위치
        Vector3 treeBranchSpawnPosition = treeBranchPos.position + randomOffset;

        for (int i = 0; i < 5; i++)
        {
            // 프리팹 복사본 생성
            treeBranchInstance = Instantiate(woodStick, treeBranchSpawnPosition, Quaternion.identity);
            // 리스트에 추가
            listWoodStick.Add(treeBranchInstance);
            // 생성된 복사본을 부모 오브젝트에 할당
            treeBranchInstance.transform.SetParent(treeBranchParentObject.transform);
        }
    }

    public void DestroyObject(GameObject obj)
    {
        if (listWoodStick.Contains(obj))
        {
            // 목록에서 객체 제거
            listWoodStick.Remove(obj);
            // 객체 제거
            Destroy(obj);
        }
    }
}
