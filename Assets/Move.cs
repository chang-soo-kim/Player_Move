using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    Rigidbody rg;
    CapsuleCollider capsule;

    Vector3 Dir;
    Vector3 CheckSpherePos;
    float inputX;
    float inputZ;

    [SerializeField]
    float Speed = 5f;

    [SerializeField]
    float jumppower = 5f;

    bool isground = false;
    bool isMove = false;
    float CheckSpherePosY;

    int GroundedLayer;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        GroundedLayer = 1 << LayerMask.NameToLayer("Water");
        CheckSpherePos = new Vector3(0, capsule.height / 2 , 0);
    }



    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        
        isground = Physics.CheckSphere(transform.position - CheckSpherePos, 0.05f, GroundedLayer) == true ? true : false;

        //�ٻ�ġ�� ã����
        isMove = !Mathf.Approximately(inputX, 0f) || !Mathf.Approximately(inputZ, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && isground)
            playerJump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position - CheckSpherePos, 0.05f);
    }

    private void FixedUpdate()
    {
        
        playerMove();
    }

    void playerMove()
    {
        Dir = new Vector3(inputX, 0f, inputZ).normalized;
        if (isMove)
        {
            rg.velocity += Speed * Time.deltaTime * Dir;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), 0.2f);
        }
        //�ӵ��� ũ��
        if (rg.velocity.magnitude > 5f)
        {
            //����� ���͸� ũ�Ⱑ �����Ǿ��ִ� maxLength�� �����Ѵ�.
            rg.velocity = Vector3.ClampMagnitude(rg.velocity, 5f);
        }
    }

    void playerJump()
    {
        rg.AddForce(Vector3.up * jumppower, ForceMode.Impulse);
    }
}
