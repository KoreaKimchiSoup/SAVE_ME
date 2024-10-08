using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeCamera : MonoBehaviour
{
    // 페이드 인, 아웃 애니메이션
    private Animator animator;
    // 애니메이터 파라미터
    private readonly string fadeIn = "isFadeIn";
    // 검은 이미지
    public GameObject fadeInImage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // SLEEP UI를 클릭했을 때 실행될 등록 함수
    public void Sleep()
    {
        StartCoroutine(SleepCoroutine());
    }

    // 페이드 인, 아웃 코루틴
    private IEnumerator SleepCoroutine()
    {
        // 검은 이미지 활성화
        fadeInImage.SetActive(true);
        // 파라미터 true
        animator.SetBool(fadeIn, true);
        // 페이드 인 시간을 기다려주기 위한 시간
        yield return new WaitForSeconds(1.5f);
        // 파라미터 다시 false
        animator.SetBool(fadeIn, false);
        // 페이드 아웃 시간을 기다려주기 위한 시간
        yield return new WaitForSeconds(1.5f);
        // 계속 검은 이미지가 활성화되지 않게 함
        fadeInImage.SetActive(false);
    }
}