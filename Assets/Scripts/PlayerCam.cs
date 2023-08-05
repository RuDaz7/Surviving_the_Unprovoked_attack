using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform FindPlayer; // 따라가야할 오브젝트 정보
    public float CamSpeed = 30.0f; //따라갈 스피드
    public float MouseSensitivity = 100.0f; //마우스 감도
    public float CamAngle = 70.0f; //카메라 각도 제한
    private float Mouse_Rot_X; //입력받을 마우스 x축 값
    private float Mouse_Rot_Y; //입력받을 마우스 y축 값

    public Transform RealCamera; //카메라의 정보
    public Vector3 dir; //방향
    public Vector3 Finaldir; //최종적으로 정해진 방향 저장
    public float MinDistance; //최소거리 
    public float MaxDistance; //최대거리 
    public float FinalDistance; //최종결정거리
    public float smooth = 10.0f;
    void Start()
    {
        //첫 시작땐 
        Mouse_Rot_X = transform.localRotation.eulerAngles.x; //입력값 초기화
        Mouse_Rot_Y = transform.localRotation.eulerAngles.y;

        dir = RealCamera.localPosition.normalized; //방향
        FinalDistance = -RealCamera.localPosition.magnitude; //크기

        //인게임에서 마우스커서 가리기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //매 프레임마다 인력값 받기
        //Mouse_Rot_X += Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime; //아래보면 하늘 바라봄 반대로 해줘야함
        Mouse_Rot_X += -(Input.GetAxis("Mouse Y")) * MouseSensitivity * Time.deltaTime;
        Mouse_Rot_Y += Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;

        Mouse_Rot_X = Mathf.Clamp(Mouse_Rot_X, -CamAngle, CamAngle); //앵글최소값, 최대값
        Quaternion rot = Quaternion.Euler(Mouse_Rot_X, Mouse_Rot_Y, 0);
        transform.rotation = rot;
    }
    //레이트 엡데이트란, 업데이트가 끝난 다음 동작하는 함수   
    void LateUpdate() 
    {
        transform.position = Vector3.MoveTowards(transform.position, FindPlayer.position, CamSpeed * Time.deltaTime);
        Finaldir = transform.TransformPoint(dir * MaxDistance); //로컬스페이스에서 > 월드스페이스로 트랜스폼 바꿔줌
        //카메라 사이에 방해물 존재 여부 판단하는법

        RaycastHit hit;

        if(Physics.Linecast(transform.position, Finaldir, out hit))//시작, 끝
        {
            FinalDistance = Mathf.Clamp(hit.distance, MinDistance, MaxDistance);
        }
        else
        {
            FinalDistance = MaxDistance;
        }
            //Lerp함수는 두 구간 사이를 부드럽게 해줌 
            RealCamera.localPosition = Vector3.Lerp(RealCamera.localPosition, dir * FinalDistance, Time.deltaTime * smooth);
        }
    }
