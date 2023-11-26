using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCameraController : MonoBehaviour
{
    GameObject player;
    Vector3 positionDelta = new Vector3(0, 10, -7);
    TypeDefinition.Direction direction;

    private void MoveToPlayer()
    {
        transform.position = player.transform.position + positionDelta;
    }
    private void SetPlayer()
    {
        player = GameObject.Find("Player");
        direction = new TypeDefinition.Direction();
        direction.Forward = new Vector3(0, -1, 1).normalized;
        if (player != null)
        {
            MoveToPlayer();
            direction.Forward = (player.transform.position - transform.position).normalized;
            Hack.Say($"���������� �÷��̾ ã�ҽ��ϴ�./n�÷��̾� ��ġ = {player.transform.position}, ���� = {direction.Forward}");
        }
        else
        {
            Hack.Err("StageCameraController.SetPlayer() : �÷��̾ ����.");
        }
        transform.rotation = Quaternion.LookRotation(direction.Forward, Vector3.Cross(direction.Forward, direction.Right));
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            MoveToPlayer();
        }
        else
        {
            SetPlayer();
        }
    }
}
