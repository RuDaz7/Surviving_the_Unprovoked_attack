using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float WalkSpeed = 5.0f;
    public Animator anim;
    Camera _camera;
    CharacterController _Cotroller;
    public float RunSpeed = 10.0f;
    public float FinalSpeed;
    public bool cameraRot; //둘러보기 기능
    public float SmoothMove = 10.0f;
    public bool Run;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        _camera = Camera.main;
        _Cotroller = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Run = true;
        }
        else
            {
                Run = false;
            }

        // float WalkHorizontal = Input.GetAxis("Horizontal");
        // float WalkVertical = Input.GetAxis("Vertical");
        // transform.Translate(Vector3.right * Time.deltaTime * WalkSpeed * WalkHorizontal, Space.World);
        // transform.Translate(Vector3.forward  * Time.deltaTime * WalkSpeed * WalkVertical, Space.World);

        if (Input.GetKey(KeyCode.CapsLock))
        {
            cameraRot = true;
        }
        else
        {
            cameraRot = false;
        }
        InputMove();
    }

    void LateUpdate()
    {
        if (cameraRot != true)
        {
            Vector3 PlayerRot = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerRot), Time.deltaTime * SmoothMove);
        }
    }

    void InputMove()
    {
        FinalSpeed = (Run) ? RunSpeed : WalkSpeed; //만약에 뛴다면 런스피트 아니라면 워크스피드

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 MoveDir = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal"); //Raw는 키보드의 부드러움을 뺌 보다 즉각적 반응
        _Cotroller.Move(MoveDir.normalized * FinalSpeed * Time.deltaTime);

        float percent = ((Run) ? 1 : 0.5f) * MoveDir.magnitude;
        anim.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}
