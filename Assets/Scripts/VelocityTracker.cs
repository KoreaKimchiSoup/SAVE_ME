using System.Collections;
using System.Collections.Generic;
// Direct Interacter 패키지 가져오기 위함
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VelocityTracker : MonoBehaviour
{
    // 오브젝트가 Grab되었는지 참조
    private XRDirectInteractor interacter;
    // 레이의 길이
    private float rayLength = 15f;
    // 플레이어로부터의 상대적 위치
    private Vector3 fishingTutorialUIOffset = new Vector3(1f, 0.3f, 0);
    // 레이와 맞닿는 지점에 생겨날 오브젝트
    public GameObject fishingVirtualHook;
    // 낚싯대를 잡았을 때 생기는 낚시 튜토리얼 UI
    public GameObject objectFollowUI;
    // 오브젝트의 위치를 플레이어의 위치로 설정
    public Transform fishingTutorialPos;
    // 레이가 발사될 초기 위치
    public Transform startRay;
    // 레이의 상호작용 레이어마스크 설정
    public LayerMask racastLayerMask;
    // 레이캐스트에 닿은 오브젝트의 정보
    public RaycastHit hit;
    // 오른쪽 컨트롤러가 낚싯대를 잡았는지에 대한 여부를 판단할 변수
    [HideInInspector]
    public bool isFishingRodGrabbed;

    void Awake()
    {
        interacter = GetComponent<XRDirectInteractor>();
    }

    private void Start()
    {
        if (interacter != null)
        {
            // 그랩버튼을 한 번 눌렀을 때 HandleSelectEntered 함수 실행
            interacter.selectEntered.AddListener(HandleSelectEntered);
            // 그랩버튼을 한 번 눌렀을 때 HandleSelectExited 함수 실행
            interacter.selectExited.AddListener(HandleSelectExited);
        }
    }

    private void Update()
    {
        if (isFishingRodGrabbed)
        {
            StartCoroutine(CreateFishingRaycast());
        }
    }

    void OnDestroy()
    {
        if (interacter != null)
        {
            // 오브젝트가 파괴될 때 등록된 함수를 제거한다 (자원관리)
            interacter.selectEntered.RemoveListener(HandleSelectEntered);
            interacter.selectExited.RemoveListener(HandleSelectExited);
        }
    }

    // HandheldSelecEntered 함수는 오브젝트가 선택되었을 때 실행한다
    private void HandleSelectEntered(SelectEnterEventArgs arg)
    {
        if (arg.interactableObject.transform.CompareTag("FishingRod"))
        {
            isFishingRodGrabbed = true;
            ActiveFishingUI();
        }
    }

    // HandheldSelecEntered 함수는 오브젝트가 선택 해제되었을 때 실행한다
    private void HandleSelectExited(SelectExitEventArgs arg)
    {
        if (arg.interactableObject.transform.CompareTag("FishingRod"))
        {
            isFishingRodGrabbed = false;
            fishingVirtualHook.SetActive(false);

            if (objectFollowUI != null)
            {
                objectFollowUI.SetActive(false);
            }
        }
    }

    // 낚시 설명서 UI
    private void ActiveFishingUI()
    {
        // UI 팝업 활성화
        objectFollowUI.SetActive(true);
        // 플레이어의 전방 방향에 UI 오브젝트 생성
        Vector3 forwardDirection = Camera.main.transform.forward;
        // 메인 카메라(플레이어 시점)의 전방 벡터
        Vector3 spawnPosition = fishingTutorialPos.transform.position + forwardDirection * fishingTutorialUIOffset.magnitude;

        // UI의 위치를 설정
        objectFollowUI.transform.position = spawnPosition;
        // UI 오브젝트가 카메라의 반대 방향을 바라보도록 회전 설정
        objectFollowUI.transform.rotation = Quaternion.LookRotation(forwardDirection);
    }

    // 레이와 닿은 부분에 가상의 낚시 찌 생성
    private IEnumerator CreateFishingRaycast()
    {
        if (Physics.Raycast(startRay.position, startRay.forward, out hit, rayLength, racastLayerMask))
        {
            fishingVirtualHook.SetActive(true);
            Debug.Log($"rayReticle.activeSelf: {fishingVirtualHook.activeSelf}");
            fishingVirtualHook.transform.position = hit.point;
        }
        else
        {
            fishingVirtualHook.SetActive(false);
            yield break;
        }

        yield return null;
    }

    // 낚시 설명서 닫기
    public void FishingUIOff()
    {
        objectFollowUI.SetActive(false);
    }
}