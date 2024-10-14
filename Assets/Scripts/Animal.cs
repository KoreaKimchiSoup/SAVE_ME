using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Animal : MonoBehaviour
{
    //동물의 이름
    [SerializeField] protected string animalName;
    //동물 체력
    [SerializeField] protected int Hp = 100;
    //걷는 속도
    [SerializeField] protected float walkSpeed;
    //뛰는 속도
    [SerializeField] protected float runSpeed;
    //토끼에 적용되는  속도 Move()안에서 walk와 Run 속도를 다르게 내게 하기 위해서
    protected float applySpeed;

    protected Vector3 direction; //방향

    //상태변수
    protected bool isAction;  //행동 중인지 아닌지 판별
    protected bool isWalking; //걷는지 안 걷는지 판별
    protected bool isRunning; //뛰고 있는지 판별
    protected bool isDead; // 살아있는지 판별

    [SerializeField] protected float walkTime; //걷기 시간
    [SerializeField] protected float waitTime; //대기 시간
    [SerializeField] protected float runTime;   //뛰기 시간 
    protected float currentTime;

    //필요한 컴포넌트들
    [SerializeField] protected Animator anim; //애니메이터
    [SerializeField] protected Rigidbody rb; // 리지드 바디
    [SerializeField] protected MeshCollider meshCol; //박스콜라이더
    [SerializeField] protected BoxCollider vitalcollider; //급소 콜라이더
    protected XRGrabInteractable grab; //죽었을 때 그랩 가능하도록

    //플레이어 포식자를 감지하기 위한 변수 
    [SerializeField] protected LayerMask PREDATORLayer; //포식자 레이어
    [SerializeField] protected float detectRadius = 5f; //감지 반경

    private void Start()
    {
        currentTime = waitTime; //대기 시키기 위해서
        isAction = true; //행동 중
        grab = gameObject.GetComponent<XRGrabInteractable>(); //그랩가능을 위해
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
            rb.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }
    //회전을 위해서
    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            //자연스러운 회전을 위해  Lerp 사용
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rb.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    //애니메이션 리셋을 위한 함수
    protected virtual void ResetAnim()
    {
        isWalking = false; //계속 걷지 않게
        isAction = true; //다음 행동을 위해 True
        isRunning = false; //계속 뛰지 않게
        applySpeed = walkSpeed; //걷는 속도가 Move()의 기본이기에 

        anim.SetBool("Walking", isWalking); //애니메이션 걷기 종료
        anim.SetBool("Running", isRunning); //뛰기 애니메이션 종료
        direction.Set(0f, Random.Range(0f, 360f), 0f); //무작위 방향으로 가기 위해서
    }

    //기본 동작
    protected void Idel()
    {
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
        applySpeed = walkSpeed; //걷는 속도 적용
        Debug.Log("Walk");
    }
    //도망가기 
    public void Run(Vector3 _targetPos)
    {
        //방향은 플레이어 반대 방향을 도망가게
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed; //뛰는 속도 적용    
        anim.SetBool("Running", isRunning);
    }


    //사망
    public void Die()
    {
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
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
    //플레이어와 포식자등을 만나면 도망가는 함수. 
   public virtual void  DetectPredator()
    {
        //detectRadius 반경 내에 모든 콜라이더 감지.
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, PREDATORLayer);
        foreach (Collider collider in colliders)
        {
            //만약 감지된 콜라이더의 태그가 PLAYER or PREDATOR
            if (collider.CompareTag("PLAYER") || collider.CompareTag("PREDATOR"))
            {
                if (!isDead)
                {
                    Debug.Log("포식자 조우");
                    Run(collider.transform.position);
                }
             
                break;
            }
        }
    }
    //맞을 때
    private void OnCollisionEnter(Collision collision)
    {
        //무기에 닿으면 
        if (collision.collider.CompareTag("WEAPON"))
        {
            //살아있을때만
            if (!isDead)
            {
                Die();
                Farming();
            }
        }
    }

    //기즈모를 이용해서  감지 반경 보이게 하기 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

}
