using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Animal : MonoBehaviour
{
    //������ �̸�
    [SerializeField] protected string animalName;
    //���� ü��
    [SerializeField] protected int Hp = 100;
    //�ȴ� �ӵ�
    [SerializeField] protected float walkSpeed;
    //�ٴ� �ӵ�
    [SerializeField] protected float runSpeed;
    //�䳢�� ����Ǵ�  �ӵ� Move()�ȿ��� walk�� Run �ӵ��� �ٸ��� ���� �ϱ� ���ؼ�
    protected float applySpeed;

    protected Vector3 direction; //����

    //���º���
    protected bool isAction;  //�ൿ ������ �ƴ��� �Ǻ�
    protected bool isWalking; //�ȴ��� �� �ȴ��� �Ǻ�
    protected bool isRunning; //�ٰ� �ִ��� �Ǻ�
    protected bool isDead; // ����ִ��� �Ǻ�

    [SerializeField] protected float walkTime; //�ȱ� �ð�
    [SerializeField] protected float waitTime; //��� �ð�
    [SerializeField] protected float runTime;   //�ٱ� �ð� 
    protected float currentTime;

    //�ʿ��� ������Ʈ��
    [SerializeField] protected Animator anim; //�ִϸ�����
    [SerializeField] protected Rigidbody rb; // ������ �ٵ�
    [SerializeField] protected MeshCollider meshCol; //�ڽ��ݶ��̴�
    [SerializeField] protected BoxCollider vitalcollider; //�޼� �ݶ��̴�
    protected XRGrabInteractable grab; //�׾��� �� �׷� �����ϵ���

    //�÷��̾� �����ڸ� �����ϱ� ���� ���� 
    [SerializeField] protected LayerMask PREDATORLayer; //������ ���̾�
    [SerializeField] protected float detectRadius = 5f; //���� �ݰ�

    private void Start()
    {
        currentTime = waitTime; //��� ��Ű�� ���ؼ�
        isAction = true; //�ൿ ��
        grab = gameObject.GetComponent<XRGrabInteractable>(); //�׷������� ����
    }
    private void Update()
    {
        if (!isDead)
        {
            ElapseTIme();
            Move();
            Rotation();
            DetectPredator();
        }
    }
    //�ð��� ��� �Լ�.
    private void ElapseTIme()
    {
        if (isAction) //�ൿ ���� �� 
        {
            //currentTime ����
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)  //0���� �۾����� �� ���� �ൿ
            {
                ResetAnim();
            }
        }
    }
    //�ȱ� ���� �Լ�
    private void Move()
    {
        if (isWalking || isRunning)
        {
            rb.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }
    //ȸ���� ���ؼ�
    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            //�ڿ������� ȸ���� ����  Lerp ���
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rb.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    //�ִϸ��̼� ������ ���� �Լ�
    protected virtual void ResetAnim()
    {
        isWalking = false; //��� ���� �ʰ�
        isAction = true; //���� �ൿ�� ���� True
        isRunning = false; //��� ���� �ʰ�
        applySpeed = walkSpeed; //�ȴ� �ӵ��� Move()�� �⺻�̱⿡ 

        anim.SetBool("Walking", isWalking); //�ִϸ��̼� �ȱ� ����
        anim.SetBool("Running", isRunning); //�ٱ� �ִϸ��̼� ����
        direction.Set(0f, Random.Range(0f, 360f), 0f); //������ �������� ���� ���ؼ�
    }

    //�⺻ ����
    protected void Idel()
    {
        currentTime = waitTime;
        Debug.Log("Idel");
    }
    //�Ա� ����
    protected void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Eat");
    }
    //�ȱ� ����
    protected void Walk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed; //�ȴ� �ӵ� ����
        Debug.Log("Walk");
    }
    //�������� 
    public void Run(Vector3 _targetPos)
    {
        //������ �÷��̾� �ݴ� ������ ��������
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed; //�ٴ� �ӵ� ����    
        anim.SetBool("Running", isRunning);
    }


    //���
    public void Die()
    {
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }
    //�׾����� �׷������ϵ���
    protected void Farming()
    {
        if (isDead)
        {
            //������ ���� �� �ְ�
            grab.enabled = true;

        }
    }
    //�÷��̾�� �����ڵ��� ������ �������� �Լ�. 
   public virtual void  DetectPredator()
    {
        //detectRadius �ݰ� ���� ��� �ݶ��̴� ����.
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, PREDATORLayer);
        foreach (Collider collider in colliders)
        {
            //���� ������ �ݶ��̴��� �±װ� PLAYER or PREDATOR
            if (collider.CompareTag("PLAYER") || collider.CompareTag("PREDATOR"))
            {
                if (!isDead)
                {
                    Debug.Log("������ ����");
                    Run(collider.transform.position);
                }
             
                break;
            }
        }
    }
    //���� ��
    private void OnCollisionEnter(Collision collision)
    {
        //���⿡ ������ 
        if (collision.collider.CompareTag("WEAPON"))
        {
            //�����������
            if (!isDead)
            {
                Die();
                Farming();
            }
        }
    }

    //����� �̿��ؼ�  ���� �ݰ� ���̰� �ϱ� 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

}
