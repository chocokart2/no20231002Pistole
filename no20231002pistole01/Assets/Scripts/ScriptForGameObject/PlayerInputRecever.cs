using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 마우스나 키보드 입력을 하는것을 받는 장치입니다.
public class PlayerInputRecever : MonoBehaviour
{
    KeyCode forwardKey = KeyCode.W;
    KeyCode backKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;

    KeyCode reloadKey = KeyCode.R;

    Camera cam;

    UnitController myUnitController;

    public LayerMask unitObjectLayer;
    public LayerMask mapMaskLayer;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        myUnitController = GetComponent<UnitController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveKeyInput();
        ActionKeyInput();
        MouseInput();
    }

    void MoveKeyInput()
    {
        Vector3 delta = Vector3.zero;

        if (Input.GetKey(forwardKey)) delta += Vector3.forward;
        if (Input.GetKey(backKey)) delta += Vector3.back;
        if (Input.GetKey(leftKey)) delta += Vector3.left;
        if (Input.GetKey(rightKey)) delta += Vector3.right;

        if (delta.z > 0.0f) myUnitController.Move(UnitController.Direction90.forward);
        if (delta.z < 0.0f) myUnitController.Move(UnitController.Direction90.back);
        if (delta.x > 0.0f) myUnitController.Move(UnitController.Direction90.right);
        if (delta.x < 0.0f) myUnitController.Move(UnitController.Direction90.left);
    }
    void MouseInput()
    {
        // 화면에서 놓인 마우스를 기준으로 카메라에서 레이를 발사.
        // 지도에 부딛히는것과, 오브젝트에 부딛히는거를 구해둔다.

        RaycastHit m_hitOnMap;
        RaycastHit m_hitOnUnit;

        Ray m_rayFromMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(
            m_rayFromMouse.origin,
            m_rayFromMouse.direction,
            hitInfo: out m_hitOnMap,
            maxDistance: 200.0f,
            layerMask: mapMaskLayer))
        {
            myUnitController.unitViewDirection.Forward = (m_hitOnMap.point - transform.position).normalized;
            //Hack.Say($"direct = {m_hitOnMap.point}");
        }
        else
        {
            //Hack.Say("안 부딛혔어");
        }

        Physics.Raycast(
            m_rayFromMouse.origin,
            m_rayFromMouse.direction,
            hitInfo: out m_hitOnUnit,
            maxDistance: 200.0f,
            layerMask: mapMaskLayer);

        Debug.DrawRay(m_rayFromMouse.origin, m_rayFromMouse.direction * 200.0f, Color.red, 0.1f);



        // 마우스 클릭
        if (Input.GetMouseButtonDown(0))
        {
            Hack.Say("PlayerInputRecever.MouseInput() : 클릭됨");

            myUnitController.Act(myUnitController.unitViewDirection.Forward);
        }
        if (Input.GetMouseButton(0))
        {
            myUnitController.ActHold(myUnitController.unitViewDirection.Forward);
        }
    }
    void ActionKeyInput()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            Hack.Say("리로드");
            myUnitController.Reload(myUnitController.unitViewDirection.Forward);
        }
    }

    


    // 마우스 가져다대기
    // 마우스 클릭
    // -> 마우스 방향 따라 맵에 레이 쏘는 함수
    // -> ㄴ 맵에 맞는거 하나, 유닛에 맞는거 하나
    // -> 플레이어가 특정 좌표에 마우스 클릭을 할때 이벤트 발생
}
