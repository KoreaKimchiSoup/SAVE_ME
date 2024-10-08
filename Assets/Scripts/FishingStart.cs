using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class FishingStart : MonoBehaviour
{
    // 트리거 버튼 매핑을 위한 변수
    private XRGrabInteractable grabInteractable;
    // isFishingRodGrabbed과, rayReticle 비활성을 위한 참조
    public VelocityTracker velocityTracker;
    // 낚시 트리거 발동 시 컨트롤러 진동을 주기 위함
    public XRBaseController haptic;
    // 레이가 닿인 곳에 좌표를 트리거 한 번 누를 때 한 번 뽑아오기 위한 변수
    private Vector3 lastHitPoint = Vector3.zero;
    // 진동 강도
    private float hapticForce = 0.7f;
    // 진동 지속시간
    private float hapticDuration = 1f;
    // 진동 시간안에 로직을 수행했는지의 여부
    private bool checkHaptic = false;
    // 낚시가 진행중일 때 같은 키를 누르면 원래기능을 끄고,
    // checkHaptic만을 쓰기 위한 변수
    private bool haptingDeny = true;
    // 낚시 찌 오브젝트
    public GameObject fishingHook;
    // trigger버튼 매핑하기
    public InputActionReference trigger;
    // 낚시가 성공하면 드랍될 물고기
    public DropFish dropFish;
    // 낚시의 성공 여부를 이벤트 시스템에 넘김
    // 다른 스크립트에서 이를 참조하여 로직을 실행할 예정
    public UnityEvent fishingSuccess;

    private void Awake()
    {
        trigger.action.Enable();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }
    
    void Start()
    {
        // activated
        // 트리거를 눌렀을 때 메서드를 자동 실행시켜 주는 이벤트 처리 시스템
        // AddListener는 코루틴함수를 담을 수 없기 때문에 람다식을 사용함
        grabInteractable.activated.AddListener( (arg) => StartCoroutine(Fishing()) );
        fishingSuccess.AddListener( () => StartCoroutine(Fishing()) );
    }

    private void ActiveHaptic(float force, float duration)
    {
        Debug.Log("진동 발생!");
        // 진동의 강도, 진동의 지속시간
        haptic.SendHapticImpulse(force, duration);

        // 시간 안에 키 입력을 받으면 낚시 성공 조건이 true로 바뀜
        StartCoroutine(WaitForKeyInput());
    }

    private IEnumerator Fishing()
    {
        if (haptingDeny)
        {
            // 여러번 실행 안되게
            haptingDeny = false;
            // 트리거 버튼을 한 번 클릭할 때 마다 레이와 닿은 지점 위치값 저장
            lastHitPoint = velocityTracker.hit.point;
            // 닿은 지점에 오브젝트 생성
            GameObject go = Instantiate(fishingHook, lastHitPoint, Quaternion.identity);
            // 가상의 낚시 찌 오브젝트 활성화
            go.SetActive(true);
            // FishingUI 비활성화
            velocityTracker.fishingVirtualHook.SetActive(false);
            // isFishingRodGrabbed를 false로 하고 다시 rayReticle이 활성화되지 않게 설정함
            velocityTracker.isFishingRodGrabbed = false;

            // 1 ~ 4초 랜덤한 시간 대기 후
            yield return new WaitForSeconds(Random.Range(1f, 4f));

            // 진동 발생
            ActiveHaptic(hapticForce, hapticDuration);

            yield return new WaitForSeconds(hapticDuration); // 키 입력 대기 시간을 기다립니다

            // 성공조건의 여부에 따라서
            if (checkHaptic)
            {
                Debug.Log("낚시 성공");
                // 낚시 성공 시 드랍되는 물고기 구현
                // 1. 낚시 성공여부 체크
                fishingSuccess?.Invoke();
                // 2. 물고기 종류 설정
                // 3. 드랍되는 물고기의 종류는 확률
            }
            else
            {
                Debug.Log("낚시 실패");
            }

            // 가상의 낚시 찌 파괴
            Destroy(go);
            // 다시 낚시가 가능하게 설정
            velocityTracker.isFishingRodGrabbed = true;
            haptingDeny = true;
        }
    }

    // 진동 시간 내에 입력을 기다릴 코루틴
    private IEnumerator WaitForKeyInput()
    {
        float targetTime = hapticDuration;
        float startTime = Time.time;

        while (Time.time - startTime < targetTime)
        {
            // VR 컨트롤러의 트리거 버튼 입력 감지
            if (trigger.action.enabled && trigger.action.triggered)
            {
                // 진동 시간(물고기가 찌를 물었다면)내에 트리거 버튼을 누르면 true
                checkHaptic = true;
                yield break;
            }

            yield return null;
        }

        // 진동 시간 내에 트리거 버튼을 누르지 못했다면
        checkHaptic = false;
    }
}










// 진동 발생 시점 체크
// 이 시점에서 0.5초간 낚시 로직을 성공했는지 체크
// 1. 진동 발생 체크 코루틴
// 2. 진동이 발생할 때 호출 bool값 true
// 2. hapticDuration 만큼 대기 후 bool값 false

// ----------------내일 개발 목록--------------
// 진동발생 V
// 낚시 성공 로직 후보
// 1. 컨트롤러의 속도
// 2. 간단하게 오른손 컨트롤러 키 매핑
// 낚시 실패 로직
// 1 ~ 3초 랜덤 진동이 울리고 0.5초 안에 트리거 성공 로직 미만족시 실패
// 다시 isFishingRodGrabbed true로 설정 후 낚시를 재시작 할 수 있게함


// ActivateEventArgs
// 위 클래스는 트리거 버튼을 눌렀을 때
// 상호작용되는 오브젝트의 정보를 받아온다
// -----------------------------
// 그러나 받아올 정보는 아직 필요없기 때문에
// grabInteractable.activated에 메서드를 추가하기 위해서 필요하므로
// 필수로 매개변수 타입에 명시하여야 한다
//private void Fishing(ActivateEventArgs arg)
//{
//    // 트리거 버튼을 한 번 클릭할 때 마다 레이와 닿은 지점 벡터값 저장
//    lastHitPoint = velocityTracker.hit.point;
//    // 닿은 지점에 오브젝트 생성
//    GameObject go = Instantiate(fishingHook, lastHitPoint, Quaternion.identity);
//    // 오브젝트 활성화
//    go.SetActive(true);
//    // FishingUI 비활성화
//    velocityTracker.rayReticle.SetActive(false);
//    // isFishingRodGrabbed를 false로 하고 다시 rayReticle이 활성화되지 않게 설정함
//    velocityTracker.isFishingRodGrabbed = false;

//    // ----------------내일 개발 목록--------------
//    // 1 ~ 3초간 대기 후 (이 시점에 fishingHook 오브젝트가 계속 생성안되게 막아야함)
//    // yield return new WaitForSecond(Random.Range(1f, 3f)); 
//    // 진동발생
//    // 햅틱
//    // 낚시 성공 로직 후보
//    // 1. 컨트롤러의 속도
//    // 2. 간단하게 오른손 컨트롤러 키 매핑
//    // 낚시 실패 로직
//    // 1 ~ 3초 랜덤 진동이 울리고 0.5초 안에 트리거 성공 로직 미만족시 실패
//    // 다시 isFishingRodGrabbed true로 설정 후 낚시를 재시작 할 수 있게함
//}
