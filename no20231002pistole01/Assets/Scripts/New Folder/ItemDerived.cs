using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDerived : ItemBase
{

    public class Fist : Item
    {
        public override void Use(GameObject user, Vector3 direction)
        {
            base.Use(user, direction);
        }
    }

    public class Pistol : Item
    {
        public override void Use(GameObject user, Vector3 direction)
        {
            base.Use(user, direction);
        }
    }
}
