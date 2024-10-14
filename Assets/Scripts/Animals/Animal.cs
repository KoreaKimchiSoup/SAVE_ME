using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    //동물의 이름
    [SerializeField] protected string animalName;
    //동물 체력
    [SerializeField] protected int Hp;
    //걷는 속도
    [SerializeField] protected float walkSpeed;
    //뛰는 속도
    [SerializeField] protected float runSpeed;
    //토끼에 적용되는  속도 Move()안에서 walk와 Run 속도를 다르게 내게 하기 위해서
    protected float applySpeed;

    public Vector3 destination; //목적지
    public float attackRange; //공격범위

    //상태변수
    protected bool isAction;  //행동 중인지 아닌지 판별
    protected bool isWalking; //걷는지 안 걷는지 판별
    protected bool isRunning; //뛰고 있는지 판별
    public bool isDead; // 살아있는지 판별
    protected bool isAttack;//공격 중인지 아닌지 판별 

    [SerializeField] protected float walkTime; //걷기 시간
    [SerializeField] protected float waitTime; //대기 시간
    [SerializeField] protected float runTime;   //뛰기 시간 
    [SerializeField] protected float attackTime; //공격 시간
    protected float currentTime;

    [SerializeField] protected AudioClip attackSfx;  //공격소리
    [SerializeField] protected AudioClip dieSfx; //죽는 소리
    [SerializeField] protected AudioClip idleSfx; //기본 소리
    [SerializeField] protected AudioClip hurtSfx; //맞는 소리

    //필요한 컴포넌트들
    [SerializeField] protected Animator anim; //애니메이터
    [SerializeField] protected Rigidbody rb; // 리지드 바디
    [SerializeField] protected MeshCollider meshCol; //박스콜라이더
    protected AudioSource audioSource; //오디오 소스
    protected XRGrabInteractable grab; //죽었을 때 그랩 가능하도록
    protected NavMeshAgent nav;  //네비게이션.
    Vector3 deathPosition;
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        currentTime = waitTime; //대기 시키기 위해서
        isAction = true; //행동 중
        grab = gameObject.GetComponent<XRGrabInteractable>(); //그랩가능을 위해
        audioSource = gameObject.GetComponent<AudioSource>();   //오디오소스 
    }
    private void Update()
    {
        if (!isDead)
        {
            ElapseTIme();
            Move();
        }
    }
    //시간을 깎는 함수.
    private void ElapseTIme()
    {
        if (isAction) //행동 중일 때 
        {
            //currentTime 감소
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)  //0보다 작아졌을 때 다음 행동
            {
                ResetAnim();
            }
        }
    }
    //걷기 위한 함수
    private void Move()
    {
        if (isWalking || isRunning)
        {
            //네비게이션 방향설정
            nav.SetDestination(transform.position + destination * 5f);
        }
    }
    //애니메이션 리셋을 위한 함수
    protected virtual void ResetAnim()
    {
        isWalking = false; //계속 걷지 않게
        isAction = true; //다음 행동을 위해 True
        isRunning = false; //계속 뛰지 않게
        isAttack = false; //계속 공격하지않게. 
        nav.speed = walkSpeed; //걷는 속도가 Move()의 기본이기에 
        nav.ResetPath(); //네비게이션 초기화.

        anim.SetBool("Walking", isWalking); //애니메이션 걷기 종료
        anim.SetBool("Running", isRunning); //뛰기 애니메이션 종료
        anim.SetBool("Attack", isAttack); //공격 애니메이션 종료
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f)); //무작위 방향으로 가기 위해서
    }

    //기본 동작
    protected void Idel()
    {
        audioSource.PlayOneShot(idleSfx);
        currentTime = waitTime;
        Debug.Log("Idel");
    }
    //먹기 동작
    protected void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Eat");
    }
    //걷기 동작
    protected void Walk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed; //걷는 속도 적용
        Debug.Log("Walk");
    }

    //도망가기 
    public void Run(Vector3 _targetPos)
    {
        if (!isDead)
        {
            //방향은 플레이어 반대 방향을 도망가게
            destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;
            currentTime = runTime;
            isWalking = false;
            isRunning = true;
            nav.speed = runSpeed; //뛰는 속도 적용    
            anim.SetBool("Running", isRunning);
        }
    }
    //공격 
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
        //방향은 먹이 또는 플레이어
        destination = new Vector3(tracePos.x - transform.position.x, 0f, tracePos.z - transform.position.z).normalized;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        isAttack = false;
        nav.speed = runSpeed;
        anim.SetBool("Attack", isAttack);
        anim.SetBool("Running", isRunning);
    }


    //사망
    public void Die()
    {
        audioSource.PlayOneShot(dieSfx);
        isDead = true;
        isWalking = false;
        isRunning = false;
        anim.SetTrigger("Dead");
        Farming();
        deathPosition = transform.position; //죽은 위치 고정을 위해서
        rb.constraints = RigidbodyConstraints.FreezeAll; //몸이 안 움직이게.
        // 동물과 자식 오브젝트까지 포함하여 레이어를 "DeadAnimal"로 변경
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("ANIMALDIE"));
        ChangeTagOfObjectAndChildren(gameObject,"Untagged");
    }
    //죽었을때 그랩가능하도록
    protected void Farming()
    {
        if (isDead)
        {
            //죽으면 잡을 수 있게
            grab.enabled = true;

        }
    }
    //f레이어 변경을 위한 함수
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        // 현재 오브젝트의 레이어 변경
        obj.layer = newLayer;

        // 모든 자식 오브젝트의 레이어를 재귀적으로 변경
        foreach (Transform child in obj.transform)
        {
            if (child != null)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
    //태그 변경
    void ChangeTagOfObjectAndChildren(GameObject parentObject, string newTag)
    {
        // 부모와 모든 자식 오브젝트의 Transform을 가져옴
        Transform[] allChildren = parentObject.GetComponentsInChildren<Transform>();

        // 각 오브젝트에 대해 태그 변경
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
