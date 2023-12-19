using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : TypeDefinition
{
    public enum SoundName
    {
        gunShot,
        gunReload1
    }

    public Direction unitViewDirection;

    float speed = 3.0f;
    //structure
    ItemBase.Inventory inventory;

    //component
    Rigidbody myRigidbody;
    AudioSource myGunSound;
    AudioSource myGunSoundReload1;

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
    public void ActHold(Vector3 direction)
    {
        inventory.UseHold(direction);
    }
    public void Reload(Vector3 direction)
    {
        inventory.Reload(direction);
    }

    public void PlaySound(SoundName sound)
    {
        switch (sound)
        {
            case SoundName.gunShot: myGunSound.Play(); break;
            case SoundName.gunReload1: myGunSoundReload1.Play(); break;
            default: break;
        }
    }

    private void SoundArrange()
    {
        AudioSource[] soundComponents = GetComponents<AudioSource>();
        myGunSound = soundComponents[0];
        myGunSoundReload1 = soundComponents[1];
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
        inventory.items[0] = new ItemDerived.ShotGunBuilder<ItemDerived.ShotGun>()
            .SetMagazineSize(4)
            .SetName("ShotGun")
            .SetSubItems(
                null,
                new ItemDerived.MagazineBuilder<
                    //ItemDerived.Magazine<ItemDerived.ShotGunShell>, ItemDerived.ShotGunShell>()
                    ItemDerived.Magazine>()
                    .SetCapacity(4)
                    .Build()
                    )
            .Build();

        SoundArrange();
    }

    // Update is called once per frame
    void Update()
    {
        //Move(Direction90.forward);
        //unitViewDirection.AngleDegree += Time.deltaTime * 60;

        inventory.Update();
    }
}
