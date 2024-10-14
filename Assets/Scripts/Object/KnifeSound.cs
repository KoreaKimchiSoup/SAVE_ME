using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KnifeSound : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;  // �׷��� ���� XRGrabInteractable
    public float movementThreshold = 0.1f;       // �������� ������ ������ ���� ��
    public AudioSource audioSource;              // �Ҹ��� ����� AudioSource
    public AudioClip swingSfx;                   //�ֵθ��� �Ҹ�
    public AudioClip hitSfx;                     //������ ����� �� ���� �Ҹ�.

    private bool isGrabbed = false;              // Į�� �׷��� ���¸� ����
    private Vector3 previousPosition;            // ���� �������� Į ��ġ
    private Quaternion previousRotation;         // ���� �������� Į ȸ����

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        // �׷� ���� �̺�Ʈ ���
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
        previousPosition = transform.position;   // �ʱ⿡ Į�� ��ġ ����
        previousRotation = transform.rotation;   // �ʱ⿡ Į�� ȸ�� ����
    }

    void Update()
    {
        if (isGrabbed)
        {
            // ���� ��ġ�� ���� ��ġ�� ���̸� ����Ͽ� ������ ����
            float movementDistance = Vector3.Distance(transform.position, previousPosition);
            float rotationDifference = Quaternion.Angle(transform.rotation, previousRotation);

            // ������ ����(�̵� �Ǵ� ȸ��)�� ���ذ��� ������ �Ҹ� ���
            if ((movementDistance > movementThreshold || rotationDifference > movementThreshold) && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(swingSfx);
            }

            // ���� ��ġ�� ȸ���� ���� �������� ���� ���� ������ ������Ʈ
            previousPosition = transform.position;
            previousRotation = transform.rotation;
        }
    }

    // Į�� �׷��Ǿ��� �� ȣ��Ǵ� �Լ�
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        previousPosition = transform.position;   // �׷� �������� ��ġ ����
        previousRotation = transform.rotation;   // �׷� �������� ȸ�� ����
    }

    // Į�� ������ �� ȣ��Ǵ� �Լ�
    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        audioSource.Stop();  // �Ҹ� ����
    }

    private void OnCollisionEnter(Collision collision)
    {
        //���� ����� �޼Ұų� �����϶�.
        if (collision.collider.CompareTag("Animal") || (collision.collider.CompareTag("PREDATOR"))
            ||(collision.collider.CompareTag("VITAL")))
        {
            audioSource.PlayOneShot(hitSfx);
        }
    }
}
