using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        public int ammoMagazine = 3;

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

    public class GunBuilder<GunType> : ItemBuilder<GunType> where GunType : Gun, new()
    {
        public GunBuilder<GunType> SetMagazineSize(int val)
        {
            this.returnValue.ammoMagazine = val;
            return this;
        }
    }

    public class Pistol : Gun
    {
        public Pistol()
        {
            ammoMagazine = 12;
        }
    }
    // ���� ����� ������ ȭ���Դϴ�. ���콺 Ȧ��� �۵��մϴ�. ����� �Ҽ��� �ݵ����� ���Ͽ� ź������ �ֽ��ϴ�.
    public class Rifle : Gun
    {
        public struct ShotSpread
        {
            public float amount;
            public float addPerFire;
            public float reducePerSecond;
            public float max;
        }

        public float nextShotTerm;
        public float nextShotLeftTime;
        public ShotSpread shotSpread;

        public Rifle() : base()
        {
            ammoMagazine = 30;
            shotSpread = new ShotSpread()
            {
                amount = 0.0f,
                addPerFire = 1.0f,
                reducePerSecond = 5.0f,
                max = 30.0f
            };

            nextShotTerm = 0.5f;
            nextShotLeftTime = 0.0f;
        }
        public override void Update()
        {
            if (nextShotLeftTime > 0)
            {
                nextShotLeftTime -= Time.deltaTime;
            }
            if (shotSpread.amount >= 0)
            {
                shotSpread.amount -= Time.deltaTime * shotSpread.reducePerSecond;
            }
            Hack.Say($"{shotSpread.amount}");
        }
        public override void UseHold(GameObject user, Vector3 direction)
        {
            if (nextShotLeftTime <= 0)
            {
                SpreadShot(user, direction, shotSpread.amount);
                shotSpread.amount = 
                    Mathf.Min(
                        shotSpread.max,
                        shotSpread.amount + shotSpread.addPerFire + nextShotTerm * shotSpread.reducePerSecond);
                nextShotLeftTime += nextShotTerm;
            }

            base.UseHold(user, direction);
        }

        protected void SpreadShot(GameObject user, Vector3 direction, float wideness)
        {
            Direction newVec = new Direction();
            newVec.Forward = direction;
            newVec.AngleDegree += Random.Range(-wideness, wideness);
            Shot(user, newVec.Forward);
        }
    }

    public class RifleBuilder<RifleType> : GunBuilder<RifleType> where RifleType : Rifle, new()
    {
        public RifleBuilder<RifleType> SetShotSpreadAmount(float val)
        {
            returnValue.shotSpread.amount = val;
            return this;
        }
        public RifleBuilder<RifleType> SetShotSpreadPerFire(float val)
        {
            returnValue.shotSpread.addPerFire = val;
            return this;
        }
        public RifleBuilder<RifleType> SetShotSpreadReduce(float val)
        {
            returnValue.shotSpread.reducePerSecond = val;
            return this;
        }
        public RifleBuilder<RifleType> SetShotSpreadMax(float val)
        {
            returnValue.shotSpread.max = val;
            return this;
        }
        public RifleBuilder<RifleType> SetShotTerm(float val)
        {
            returnValue.nextShotTerm = val;
            return this;
        }
    }


    public class Stg44 : Rifle
    {

    }
}


// �ѱ� ���� ���������
// ź��

// ���� ���������
//