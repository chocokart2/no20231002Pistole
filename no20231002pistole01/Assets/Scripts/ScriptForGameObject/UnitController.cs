using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : TypeDefinition
{
    public Direction unitViewDirection;

    float speed = 3.0f;
    //structure
    ItemBase.Inventory inventory;

    //component
    Rigidbody myRigidbody;



    public void Turn(float angle)
    {
        unitViewDirection.AngleDegree += angle;
    }
    public void TurnTo(Vector3 targetPoint)
    {
        Vector3 vector = targetPoint - transform.position;
        vector.y = 0;
        unitViewDirection.Forward
            = vector.normalized;
    }

    public void Move(Direction90 direction)
    {
        switch (direction)
        {
            case Direction90.forward: Move(unitViewDirection.Forward); break;
            case Direction90.back: Move(-unitViewDirection.Forward); break;
            case Direction90.left: Move(-unitViewDirection.Right); break;
            case Direction90.right: Move(unitViewDirection.Right); break;
        }
    }
    public void Move(Vector3 direction)
    {
        transform.position += Time.deltaTime * speed * direction;
    }
    public void Act(Vector3 direction)
    {
        inventory.Use(direction);
    }
    public void Reload(Vector3 direction)
    {
        inventory.Reload(direction);
    }

    // Start is called before the first frame update
    void Start()
    {
        unitViewDirection = new Direction();
        unitViewDirection.Forward = Vector3.forward;
        myRigidbody = GetComponent<Rigidbody>();
        
        inventory = new ItemBase.Inventory();
        inventory.user = gameObject;
        inventory.items = new ItemBase.Item[1];
        inventory.items[0] = new ItemBase.ItemBuilder<ItemDerived.Gun>()
            .SetName("Gun")
            .SetSubItems(new ItemBase.ItemList() { items = new ItemBase.Item[1] })
            .Build();
    }

    // Update is called once per frame
    void Update()
    {
        //Move(Direction90.forward);
        //unitViewDirection.AngleDegree += Time.deltaTime * 60;
    }
}
