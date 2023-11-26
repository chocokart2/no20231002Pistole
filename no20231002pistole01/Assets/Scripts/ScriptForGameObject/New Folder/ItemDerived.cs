using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public class Bullet
    {
        int damage;
    }

    public class Gun : Item
    {
        public const int AMMO_INDEX = 0;
        protected int ammoMagazine = 3;

        public override void Use(GameObject user, Vector3 direction)
        {
            if (subItems.items[AMMO_INDEX] == null)
            {
                Hack.Say("Subitem�� ź�˿� �������� �����ϴ�.");
                return;
            }
            if (subItems.items[AMMO_INDEX].stackCount == 0)
            {
                Hack.Say("Subitem�� źâ�� ������ϴ�.");

                return;
            }

            //this.subItems.items[AMMO] as

            
            Shot(user, direction);
            
            // �ش� �������� ���� �߻��մϴ�.


        }
        
        public override void Reload(GameObject user, Vector3 direction)
        {
            subItems.items[AMMO_INDEX] = new ItemBuilder<Item>()
                .SetName("GunBullet")
                .SetStackCount(ammoMagazine)
                .Build();
        }

        protected void Shot(GameObject user, Vector3 direction)
        {
            GameObject instantiatedObject = Instantiate(bulletGo, user.transform.position, Quaternion.Euler(direction));
            BulletController m_bulletComponent = instantiatedObject.GetComponent<BulletController>();
            m_bulletComponent.Init(direction);
        }
    }

    public class Pistol : Gun
    {
        public Pistol()
        {
            ammoMagazine = 12;
        }
    }
}


// �ѱ� ���� ���������
// ź��

// ���� ���������
//