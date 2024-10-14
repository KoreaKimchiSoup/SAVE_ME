using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PredatorFov : MonoBehaviour
{

    [SerializeField] private float viewAngle; //시야각 
    [SerializeField] private float viewDistance; //시야거리
    [SerializeField] private LayerMask targetMask; //타겟 마스크(포식자 마스크)
    Animal Animal; //Run 함수를 가져오기 위해서.
    private void Awake()
    {
        Animal = GetComponent<Animal>();
    }

    private void Update()
    {
        View();
    }
    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0, Mathf.Cos(_angle * Mathf.Deg2Rad));

    }
    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        //시야각을 씬 뷰에서 보일 수 있게 해줌.
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.blue);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.blue);

        //오버랩 스피어로 정보 가져오기 (포식자의)
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask );

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            //감지된 대상의 태그가 플레이어 혹은 PREDATOR일때 
            if (_targetTf.CompareTag("PLAYER") || _targetTf.CompareTag("Animal"))
            {
                Vector3 _direction = (_targetTf.transform.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);
                //시야각의 반에 포함 될 때 
                if (_angle < viewAngle * 0.5f)
                {
                    Debug.Log($"시야내에 {_targetTf.name}가 있습니다");
                    //시선안에 있던 대상 반대로 Run 실행.
                    Animal.Trace(_targetTf.transform.position);
                    //만약 공격 거리 안 이라면
                    if (Vector3.Distance(transform.position, _targetTf.position) <= Animal.attackRange)
                    {
                        Vector3 directionToHit = (_targetTf.transform.position - transform.position).normalized;
                       Animal.destination = Quaternion.LookRotation(directionToHit).eulerAngles;
                        transform.eulerAngles = new Vector3(0, Animal.destination.y, 0);
                        Animal.Attack();
                    }
                    else
                    {
                        Animal.Trace(_targetTf.transform.position);
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        //감지 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

    }
}


