using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ���콺�� Ű���� �Է��� �ϴ°��� �޴� ��ġ�Դϴ�.
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
        // ȭ�鿡�� ���� ���콺�� �������� ī�޶󿡼� ���̸� �߻�.
        // ������ �ε����°Ͱ�, ������Ʈ�� �ε����°Ÿ� ���صд�.

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
            //Hack.Say("�� �ε�����");
        }

        Physics.Raycast(
            m_rayFromMouse.origin,
            m_rayFromMouse.direction,
            hitInfo: out m_hitOnUnit,
            maxDistance: 200.0f,
            layerMask: mapMaskLayer);

        Debug.DrawRay(m_rayFromMouse.origin, m_rayFromMouse.direction * 200.0f, Color.red, 0.1f);



        // ���콺 Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            Hack.Say("PlayerInputRecever.MouseInput() : Ŭ����");

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
            Hack.Say("���ε�");
            myUnitController.Reload(myUnitController.unitViewDirection.Forward);
        }
    }

    


    // ���콺 �����ٴ��
    // ���콺 Ŭ��
    // -> ���콺 ���� ���� �ʿ� ���� ��� �Լ�
    // -> �� �ʿ� �´°� �ϳ�, ���ֿ� �´°� �ϳ�
    // -> �÷��̾ Ư�� ��ǥ�� ���콺 Ŭ���� �Ҷ� �̺�Ʈ �߻�
}
