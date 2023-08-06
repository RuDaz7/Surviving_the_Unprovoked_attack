using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    Camera _camera;
    CharacterController _Cotroller;
    public float WalkSpeed = 5.0f;
    public float RunSpeed = 10.0f;
    public float FinalSpeed;
    public bool cameraRot; //둘러보기 기능
    public float SmoothMove = 10.0f;
    public bool Run; 
    public bool Jumping = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        _camera = Camera.main;
        _Cotroller = this.GetComponent<CharacterController>();
    }

    void FixedUpdate() 
    {
       if(Input.GetKey(KeyCode.Space) && Jumping == false)
        {
        Jump();
        }
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
        Vector3 forward = transform.TransformDirection(Vector3.forward); //디렉션은 방향
        Vector3 right = transform.TransformDirection(Vector3.right); //라이트는 지상에서x좌표를 뜻함
        Vector3 MoveDir = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal"); //Raw는 키보드의 부드러움을 뺌 보다 즉각적 반응
        _Cotroller.Move(MoveDir.normalized * FinalSpeed * Time.deltaTime); //Move메소드 사용

        float percent = ((Run) ? 1 : 0.5f) * MoveDir.magnitude;
        anim.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
    void Jump()
    {   
         Vector3 up = transform.TransformDirection(Vector3.up); //y방향
         Vector3 DirY = up * 10.0f; //점프
        _Cotroller.Move(DirY.normalized * 15 * Time.deltaTime); //Move메소드 사용
    }

      private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            Debug.Log("땅에 닿음");
            Jumping = false;
        }
        else
        {
            Jumping = true;
            Debug.Log("땅에서 떨어짐");
        }
    }
}
