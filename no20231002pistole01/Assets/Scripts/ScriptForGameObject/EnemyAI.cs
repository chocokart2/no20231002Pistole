using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    bool isFoundPlayer = false;
    // myComponent
    UnitController myUnitController;
    // otherComponent
    GameObject playerGameObject;


    // Start is called before the first frame update
    void Start()
    {
        myUnitController = GetComponent<UnitController>();

        playerGameObject = GameObject.Find("Player");
        if (playerGameObject != null) isFoundPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    void LookToPlayer()
    {
        if (!isFoundPlayer)
        {
            Debug.Log("DEBUG_EnemyAI.LookToPlayer : 플레이어를 찾을 수 없습니다.");
            return;
        }
        if (myUnitController == null)
        {
            Debug.Log("DEBUG_EnemyAI.LookToPlayer : 해당 에너미에 UnitController 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        Debug.Log("AAAAA");

        myUnitController.TurnTo(playerGameObject.transform.position);
    }
    void MoveToPlayer()
    {
        LookToPlayer();
        myUnitController.Move(UnitController.Direction90.forward);
    }

}
