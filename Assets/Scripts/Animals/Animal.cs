using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    //������ �̸�
    [SerializeField] protected string animalName;
    //���� ü��
    [SerializeField] protected int Hp;
    //�ȴ� �ӵ�
    [SerializeField] protected float walkSpeed;
    //�ٴ� �ӵ�
    [SerializeField] protected float runSpeed;
    //�䳢�� ����Ǵ�  �ӵ� Move()�ȿ��� walk�� Run �ӵ��� �ٸ��� ���� �ϱ� ���ؼ�
    protected float applySpeed;

    public Vector3 destination; //������
    public float attackRange; //���ݹ���

    //���º���
    protected bool isAction;  //�ൿ ������ �ƴ��� �Ǻ�
    protected bool isWalking; //�ȴ��� �� �ȴ��� �Ǻ�
    protected bool isRunning; //�ٰ� �ִ��� �Ǻ�
    public bool isDead; // ����ִ��� �Ǻ�
    protected bool isAttack;//���� ������ �ƴ��� �Ǻ� 

    [SerializeField] protected float walkTime; //�ȱ� �ð�
    [SerializeField] protected float waitTime; //��� �ð�
    [SerializeField] protected float runTime;   //�ٱ� �ð� 
    [SerializeField] protected float attackTime; //���� �ð�
    protected float currentTime;

    [SerializeField] protected AudioClip attackSfx;  //���ݼҸ�
    [SerializeField] protected AudioClip dieSfx; //�״� �Ҹ�
    [SerializeField] protected AudioClip idleSfx; //�⺻ �Ҹ�
    [SerializeField] protected AudioClip hurtSfx; //�´� �Ҹ�

    //�ʿ��� ������Ʈ��
    [SerializeField] protected Animator anim; //�ִϸ�����
    [SerializeField] protected Rigidbody rb; // ������ �ٵ�
    [SerializeField] protected MeshCollider meshCol; //�ڽ��ݶ��̴�
    protected AudioSource audioSource; //����� �ҽ�
    protected XRGrabInteractable grab; //�׾��� �� �׷� �����ϵ���
    protected NavMeshAgent nav;  //�׺���̼�.
    Vector3 deathPosition;
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        currentTime = waitTime; //��� ��Ű�� ���ؼ�
        isAction = true; //�ൿ ��
        grab = gameObject.GetComponent<XRGrabInteractable>(); //�׷������� ����
        audioSource = gameObject.GetComponent<AudioSource>();   //������ҽ� 
    }
    private void Update()
    {
        if (!isDead)
        {
            ElapseTIme();
            Move();
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
            //�׺���̼� ���⼳��
            nav.SetDestination(transform.position + destination * 5f);
        }
    }
    //�ִϸ��̼� ������ ���� �Լ�
    protected virtual void ResetAnim()
    {
        isWalking = false; //��� ���� �ʰ�
        isAction = true; //���� �ൿ�� ���� True
        isRunning = false; //��� ���� �ʰ�
        isAttack = false; //��� ���������ʰ�. 
        nav.speed = walkSpeed; //�ȴ� �ӵ��� Move()�� �⺻�̱⿡ 
        nav.ResetPath(); //�׺���̼� �ʱ�ȭ.

        anim.SetBool("Walking", isWalking); //�ִϸ��̼� �ȱ� ����
        anim.SetBool("Running", isRunning); //�ٱ� �ִϸ��̼� ����
        anim.SetBool("Attack", isAttack); //���� �ִϸ��̼� ����
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f)); //������ �������� ���� ���ؼ�
    }

    //�⺻ ����
    protected void Idel()
    {
        audioSource.PlayOneShot(idleSfx);
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
        nav.speed = walkSpeed; //�ȴ� �ӵ� ����
        Debug.Log("Walk");
    }

    //�������� 
    public void Run(Vector3 _targetPos)
    {
        if (!isDead)
        {
            //������ �÷��̾� �ݴ� ������ ��������
            destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;
            currentTime = runTime;
            isWalking = false;
            isRunning = true;
            nav.speed = runSpeed; //�ٴ� �ӵ� ����    
            anim.SetBool("Running", isRunning);
        }
    }
    //���� 
    public void Attack()
    {
        audioSource.PlayOneShot(attackSfx);
        isAttack = true;
        anim.SetBool("Attack", isAttack);
        currentTime = attackTime;
        isWalking = false;
        isRunning = false;
    }
    public void Trace(Vector3 tracePos)
    {
        //������ ���� �Ǵ� �÷��̾�
        destination = new Vector3(tracePos.x - transform.position.x, 0f, tracePos.z - transform.position.z).normalized;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        isAttack = false;
        nav.speed = runSpeed;
        anim.SetBool("Attack", isAttack);
        anim.SetBool("Running", isRunning);
    }


    //���
    public void Die()
    {
        audioSource.PlayOneShot(dieSfx);
        isDead = true;
        isWalking = false;
        isRunning = false;
        anim.SetTrigger("Dead");
        Farming();
        deathPosition = transform.position; //���� ��ġ ������ ���ؼ�
        rb.constraints = RigidbodyConstraints.FreezeAll; //���� �� �����̰�.
        // ������ �ڽ� ������Ʈ���� �����Ͽ� ���̾ "DeadAnimal"�� ����
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("ANIMALDIE"));
        ChangeTagOfObjectAndChildren(gameObject,"Untagged");
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
    //f���̾� ������ ���� �Լ�
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        // ���� ������Ʈ�� ���̾� ����
        obj.layer = newLayer;

        // ��� �ڽ� ������Ʈ�� ���̾ ��������� ����
        foreach (Transform child in obj.transform)
        {
            if (child != null)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
    //�±� ����
    void ChangeTagOfObjectAndChildren(GameObject parentObject, string newTag)
    {
        // �θ�� ��� �ڽ� ������Ʈ�� Transform�� ������
        Transform[] allChildren = parentObject.GetComponentsInChildren<Transform>();

        // �� ������Ʈ�� ���� �±� ����
        foreach (Transform child in allChildren)
        {
            child.gameObject.tag = newTag;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
