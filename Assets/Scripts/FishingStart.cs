using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class FishingStart : MonoBehaviour
{
    // Ʈ���� ��ư ������ ���� ����
    private XRGrabInteractable grabInteractable;
    // isFishingRodGrabbed��, rayReticle ��Ȱ���� ���� ����
    public VelocityTracker velocityTracker;
    // ���� Ʈ���� �ߵ� �� ��Ʈ�ѷ� ������ �ֱ� ����
    public XRBaseController haptic;
    // ���̰� ���� ���� ��ǥ�� Ʈ���� �� �� ���� �� �� �� �̾ƿ��� ���� ����
    private Vector3 lastHitPoint = Vector3.zero;
    // ���� ����
    private float hapticForce = 0.7f;
    // ���� ���ӽð�
    private float hapticDuration = 1f;
    // ���� �ð��ȿ� ������ �����ߴ����� ����
    private bool checkHaptic = false;
    // ���ð� �������� �� ���� Ű�� ������ ��������� ����,
    // checkHaptic���� ���� ���� ����
    private bool haptingDeny = true;
    // ���� �� ������Ʈ
    public GameObject fishingHook;
    // trigger��ư �����ϱ�
    public InputActionReference trigger;
    // ���ð� �����ϸ� ����� �����
    public DropFish dropFish;
    // ������ ���� ���θ� �̺�Ʈ �ý��ۿ� �ѱ�
    // �ٸ� ��ũ��Ʈ���� �̸� �����Ͽ� ������ ������ ����
    public UnityEvent fishingSuccess;

    private void Awake()
    {
        trigger.action.Enable();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }
    
    void Start()
    {
        // activated
        // Ʈ���Ÿ� ������ �� �޼��带 �ڵ� ������� �ִ� �̺�Ʈ ó�� �ý���
        // AddListener�� �ڷ�ƾ�Լ��� ���� �� ���� ������ ���ٽ��� �����
        grabInteractable.activated.AddListener( (arg) => StartCoroutine(Fishing()) );
        fishingSuccess.AddListener( () => StartCoroutine(Fishing()) );
    }

    private void ActiveHaptic(float force, float duration)
    {
        Debug.Log("���� �߻�!");
        // ������ ����, ������ ���ӽð�
        haptic.SendHapticImpulse(force, duration);

        // �ð� �ȿ� Ű �Է��� ������ ���� ���� ������ true�� �ٲ�
        StartCoroutine(WaitForKeyInput());
    }

    private IEnumerator Fishing()
    {
        if (haptingDeny)
        {
            // ������ ���� �ȵǰ�
            haptingDeny = false;
            // Ʈ���� ��ư�� �� �� Ŭ���� �� ���� ���̿� ���� ���� ��ġ�� ����
            lastHitPoint = velocityTracker.hit.point;
            // ���� ������ ������Ʈ ����
            GameObject go = Instantiate(fishingHook, lastHitPoint, Quaternion.identity);
            // ������ ���� �� ������Ʈ Ȱ��ȭ
            go.SetActive(true);
            // FishingUI ��Ȱ��ȭ
            velocityTracker.fishingVirtualHook.SetActive(false);
            // isFishingRodGrabbed�� false�� �ϰ� �ٽ� rayReticle�� Ȱ��ȭ���� �ʰ� ������
            velocityTracker.isFishingRodGrabbed = false;

            // 1 ~ 4�� ������ �ð� ��� ��
            yield return new WaitForSeconds(Random.Range(1f, 4f));

            // ���� �߻�
            ActiveHaptic(hapticForce, hapticDuration);

            yield return new WaitForSeconds(hapticDuration); // Ű �Է� ��� �ð��� ��ٸ��ϴ�

            // ���������� ���ο� ����
            if (checkHaptic)
            {
                Debug.Log("���� ����");
                // ���� ���� �� ����Ǵ� ����� ����
                // 1. ���� �������� üũ
                fishingSuccess?.Invoke();
                // 2. ����� ���� ����
                // 3. ����Ǵ� ������� ������ Ȯ��
            }
            else
            {
                Debug.Log("���� ����");
            }

            // ������ ���� �� �ı�
            Destroy(go);
            // �ٽ� ���ð� �����ϰ� ����
            velocityTracker.isFishingRodGrabbed = true;
            haptingDeny = true;
        }
    }

    // ���� �ð� ���� �Է��� ��ٸ� �ڷ�ƾ
    private IEnumerator WaitForKeyInput()
    {
        float targetTime = hapticDuration;
        float startTime = Time.time;

        while (Time.time - startTime < targetTime)
        {
            // VR ��Ʈ�ѷ��� Ʈ���� ��ư �Է� ����
            if (trigger.action.enabled && trigger.action.triggered)
            {
                // ���� �ð�(����Ⱑ � �����ٸ�)���� Ʈ���� ��ư�� ������ true
                checkHaptic = true;
                yield break;
            }

            yield return null;
        }

        // ���� �ð� ���� Ʈ���� ��ư�� ������ ���ߴٸ�
        checkHaptic = false;
    }
}










// ���� �߻� ���� üũ
// �� �������� 0.5�ʰ� ���� ������ �����ߴ��� üũ
// 1. ���� �߻� üũ �ڷ�ƾ
// 2. ������ �߻��� �� ȣ�� bool�� true
// 2. hapticDuration ��ŭ ��� �� bool�� false

// ----------------���� ���� ���--------------
// �����߻� V
// ���� ���� ���� �ĺ�
// 1. ��Ʈ�ѷ��� �ӵ�
// 2. �����ϰ� ������ ��Ʈ�ѷ� Ű ����
// ���� ���� ����
// 1 ~ 3�� ���� ������ �︮�� 0.5�� �ȿ� Ʈ���� ���� ���� �̸����� ����
// �ٽ� isFishingRodGrabbed true�� ���� �� ���ø� ����� �� �� �ְ���


// ActivateEventArgs
// �� Ŭ������ Ʈ���� ��ư�� ������ ��
// ��ȣ�ۿ�Ǵ� ������Ʈ�� ������ �޾ƿ´�
// -----------------------------
// �׷��� �޾ƿ� ������ ���� �ʿ���� ������
// grabInteractable.activated�� �޼��带 �߰��ϱ� ���ؼ� �ʿ��ϹǷ�
// �ʼ��� �Ű����� Ÿ�Կ� ����Ͽ��� �Ѵ�
//private void Fishing(ActivateEventArgs arg)
//{
//    // Ʈ���� ��ư�� �� �� Ŭ���� �� ���� ���̿� ���� ���� ���Ͱ� ����
//    lastHitPoint = velocityTracker.hit.point;
//    // ���� ������ ������Ʈ ����
//    GameObject go = Instantiate(fishingHook, lastHitPoint, Quaternion.identity);
//    // ������Ʈ Ȱ��ȭ
//    go.SetActive(true);
//    // FishingUI ��Ȱ��ȭ
//    velocityTracker.rayReticle.SetActive(false);
//    // isFishingRodGrabbed�� false�� �ϰ� �ٽ� rayReticle�� Ȱ��ȭ���� �ʰ� ������
//    velocityTracker.isFishingRodGrabbed = false;

//    // ----------------���� ���� ���--------------
//    // 1 ~ 3�ʰ� ��� �� (�� ������ fishingHook ������Ʈ�� ��� �����ȵǰ� ���ƾ���)
//    // yield return new WaitForSecond(Random.Range(1f, 3f)); 
//    // �����߻�
//    // ��ƽ
//    // ���� ���� ���� �ĺ�
//    // 1. ��Ʈ�ѷ��� �ӵ�
//    // 2. �����ϰ� ������ ��Ʈ�ѷ� Ű ����
//    // ���� ���� ����
//    // 1 ~ 3�� ���� ������ �︮�� 0.5�� �ȿ� Ʈ���� ���� ���� �̸����� ����
//    // �ٽ� isFishingRodGrabbed true�� ���� �� ���ø� ����� �� �� �ְ���
//}
