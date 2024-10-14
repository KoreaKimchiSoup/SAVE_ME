using System.Collections;
using System.Collections.Generic;
// Direct Interacter ��Ű�� �������� ����
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VelocityTracker : MonoBehaviour
{
    // ������Ʈ�� Grab�Ǿ����� ����
    private XRDirectInteractor interacter;
    // ������ ����
    private float rayLength = 15f;
    // �÷��̾�κ����� ����� ��ġ
    private Vector3 fishingTutorialUIOffset = new Vector3(1f, 0.3f, 0);
    // ���̿� �´�� ������ ���ܳ� ������Ʈ
    public GameObject fishingVirtualHook;
    // ���˴븦 ����� �� ����� ���� Ʃ�丮�� UI
    public GameObject objectFollowUI;
    // ������Ʈ�� ��ġ�� �÷��̾��� ��ġ�� ����
    public Transform fishingTutorialPos;
    // ���̰� �߻�� �ʱ� ��ġ
    public Transform startRay;
    // ������ ��ȣ�ۿ� ���̾��ũ ����
    public LayerMask racastLayerMask;
    // ����ĳ��Ʈ�� ���� ������Ʈ�� ����
    public RaycastHit hit;
    // ������ ��Ʈ�ѷ��� ���˴븦 ��Ҵ����� ���� ���θ� �Ǵ��� ����
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
            // �׷���ư�� �� �� ������ �� HandleSelectEntered �Լ� ����
            interacter.selectEntered.AddListener(HandleSelectEntered);
            // �׷���ư�� �� �� ������ �� HandleSelectExited �Լ� ����
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
            // ������Ʈ�� �ı��� �� ��ϵ� �Լ��� �����Ѵ� (�ڿ�����)
            interacter.selectEntered.RemoveListener(HandleSelectEntered);
            interacter.selectExited.RemoveListener(HandleSelectExited);
        }
    }

    // HandheldSelecEntered �Լ��� ������Ʈ�� ���õǾ��� �� �����Ѵ�
    private void HandleSelectEntered(SelectEnterEventArgs arg)
    {
        if (arg.interactableObject.transform.CompareTag("FishingRod"))
        {
            isFishingRodGrabbed = true;
            ActiveFishingUI();
        }
    }

    // HandheldSelecEntered �Լ��� ������Ʈ�� ���� �����Ǿ��� �� �����Ѵ�
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

    // ���� ���� UI
    private void ActiveFishingUI()
    {
        // UI �˾� Ȱ��ȭ
        objectFollowUI.SetActive(true);
        // �÷��̾��� ���� ���⿡ UI ������Ʈ ����
        Vector3 forwardDirection = Camera.main.transform.forward;
        // ���� ī�޶�(�÷��̾� ����)�� ���� ����
        Vector3 spawnPosition = fishingTutorialPos.transform.position + forwardDirection * fishingTutorialUIOffset.magnitude;

        // UI�� ��ġ�� ����
        objectFollowUI.transform.position = spawnPosition;
        // UI ������Ʈ�� ī�޶��� �ݴ� ������ �ٶ󺸵��� ȸ�� ����
        objectFollowUI.transform.rotation = Quaternion.LookRotation(forwardDirection);
    }

    // ���̿� ���� �κп� ������ ���� �� ����
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

    // ���� ���� �ݱ�
    public void FishingUIOff()
    {
        objectFollowUI.SetActive(false);
    }
}