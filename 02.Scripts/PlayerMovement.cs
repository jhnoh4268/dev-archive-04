using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Vector3 cameraDir = Vector3.zero;

    private Camera m_Camera;
    private CharacterController controller;
    private Animator animator;

    private Vector3 moveDir;
    void Start()
    {
        m_Camera = Camera.main;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        Animate();
        CameraFollow();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(h, 0, v).normalized;

        controller.SimpleMove(moveDir * moveSpeed);
    }

    void Rotate()
    {
        Transform target = Player.instance.target;

        if(target != null)
        {
            Vector3 dirToMonster = Player.instance.GetDirectionToMonster();
            dirToMonster.y = 0;

            RotateToQuaternion(dirToMonster);
        }
        else if(moveDir.sqrMagnitude > 0.01f)
        {
            RotateToQuaternion(moveDir);
        }
    }

    void RotateToQuaternion(Vector3 dir)
    {
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
    }

    void Animate()
    {
        animator.SetFloat("SPEED", moveDir.magnitude);
    }

    void CameraFollow()
    {
        m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, 
            transform.position + cameraDir,
            2.0f * Time.deltaTime);
    }
}
