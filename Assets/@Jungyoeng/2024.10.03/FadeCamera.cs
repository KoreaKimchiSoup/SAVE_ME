using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeCamera : MonoBehaviour
{
    // ���̵� ��, �ƿ� �ִϸ��̼�
    private Animator animator;
    // �ִϸ����� �Ķ����
    private readonly string fadeIn = "isFadeIn";
    // ���� �̹���
    public GameObject fadeInImage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // SLEEP UI�� Ŭ������ �� ����� ��� �Լ�
    public void Sleep()
    {
        StartCoroutine(SleepCoroutine());
    }

    // ���̵� ��, �ƿ� �ڷ�ƾ
    private IEnumerator SleepCoroutine()
    {
        // ���� �̹��� Ȱ��ȭ
        fadeInImage.SetActive(true);
        // �Ķ���� true
        animator.SetBool(fadeIn, true);
        // ���̵� �� �ð��� ��ٷ��ֱ� ���� �ð�
        yield return new WaitForSeconds(1.5f);
        // �Ķ���� �ٽ� false
        animator.SetBool(fadeIn, false);
        // ���̵� �ƿ� �ð��� ��ٷ��ֱ� ���� �ð�
        yield return new WaitForSeconds(1.5f);
        // ��� ���� �̹����� Ȱ��ȭ���� �ʰ� ��
        fadeInImage.SetActive(false);
    }
}