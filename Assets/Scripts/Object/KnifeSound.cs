using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KnifeSound : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;  // 그랩을 위한 XRGrabInteractable
    public float movementThreshold = 0.1f;       // 움직임의 강도를 감지할 기준 값
    public AudioSource audioSource;              // 소리를 재생할 AudioSource
    public AudioClip swingSfx;                   //휘두르는 소리
    public AudioClip hitSfx;                     //동물에 닿았을 때 나는 소리.

    private bool isGrabbed = false;              // 칼이 그랩된 상태를 추적
    private Vector3 previousPosition;            // 이전 프레임의 칼 위치
    private Quaternion previousRotation;         // 이전 프레임의 칼 회전값

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        // 그랩 상태 이벤트 등록
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
        previousPosition = transform.position;   // 초기에 칼의 위치 저장
        previousRotation = transform.rotation;   // 초기에 칼의 회전 저장
    }

    void Update()
    {
        if (isGrabbed)
        {
            // 현재 위치와 이전 위치의 차이를 계산하여 움직임 감지
            float movementDistance = Vector3.Distance(transform.position, previousPosition);
            float rotationDifference = Quaternion.Angle(transform.rotation, previousRotation);

            // 움직임 강도(이동 또는 회전)가 기준값을 넘으면 소리 재생
            if ((movementDistance > movementThreshold || rotationDifference > movementThreshold) && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(swingSfx);
            }

            // 현재 위치와 회전을 다음 프레임을 위한 이전 값으로 업데이트
            previousPosition = transform.position;
            previousRotation = transform.rotation;
        }
    }

    // 칼이 그랩되었을 때 호출되는 함수
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        previousPosition = transform.position;   // 그랩 시점에서 위치 저장
        previousRotation = transform.rotation;   // 그랩 시점에서 회전 저장
    }

    // 칼이 놓였을 때 호출되는 함수
    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        audioSource.Stop();  // 소리 정지
    }

    private void OnCollisionEnter(Collision collision)
    {
        //닿은 대상이 급소거나 동물일때.
        if (collision.collider.CompareTag("Animal") || (collision.collider.CompareTag("PREDATOR"))
            ||(collision.collider.CompareTag("VITAL")))
        {
            audioSource.PlayOneShot(hitSfx);
        }
    }
}
