using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFOV : MonoBehaviour
{
    [SerializeField] private float viewAngle; //�þ߰� 
    [SerializeField] private float viewDistance; //�þ߰Ÿ�
    [SerializeField] private LayerMask targetMask; //Ÿ�� ����ũ(������ ����ũ)

    Animal Animal; //Run �Լ��� �������� ���ؼ�.
    private void Start()
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

        //�þ߰��� �� �信�� ���� �� �ְ� ����.
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.blue);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.blue);

        //������ ���Ǿ�� ���� �������� (��������)
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            //������ ����� �±װ� �÷��̾� Ȥ�� PREDATOR�϶� 
            if (_targetTf.CompareTag("PLAYER") || _targetTf.CompareTag("PREDATOR"))
            {
                Vector3 _direction = (_targetTf.transform.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);
                //�þ߰��� �ݿ� ���� �� �� 
                if (_angle < viewAngle * 0.5f)
                {
                    Debug.Log($"�þ߳��� {_targetTf.name}�� �ֽ��ϴ�");
                    //�ü��ȿ� �ִ� ��� �ݴ�� Run ����.
                    Animal.Run(_targetTf.transform.position);
                }

            }
        }
    }
    private void OnDrawGizmos()
    {
        //���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

    }
}
