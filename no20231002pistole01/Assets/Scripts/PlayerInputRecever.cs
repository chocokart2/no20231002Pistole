using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputRecever : MonoBehaviour
{
    KeyCode forwardKey = KeyCode.W;
    KeyCode backKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;

    UnitController myUnitController;

    // Start is called before the first frame update
    void Start()
    {
        myUnitController = GetComponent<UnitController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveKeyInput();
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
}
