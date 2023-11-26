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

    public class Bullet
    {
        int damage;
    }

    public class Gun : Item
    {
        public const int AMMO_INDEX = 0;

        public override void Use(GameObject user, Vector3 direction)
        {
            //this.subItems.items[AMMO] as

            
            Shot(user, direction);
            
            // �ش� �������� ���� �߻��մϴ�.


        }
        
        public override void Reload(GameObject user, Vector3 direction)
        {
            base.Reload(user, direction);
        }

        protected void Shot(GameObject user, Vector3 direction)
        {
            GameObject instantiatedObject = Instantiate(bulletGo, user.transform.position, Quaternion.Euler(direction));
            BulletController m_bulletComponent = instantiatedObject.GetComponent<BulletController>();
            m_bulletComponent.Init(direction);
        }
    }
}


// �ѱ� ���� ���������
// ź��

// ���� ���������
//