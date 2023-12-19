using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ItemDerived : ItemBase
{
    public interface IMagazine
    {

    }

    public interface IBullet
    {
        void Active(GameObject user, Vector3 direction);
    }

    public class Fist : Item
    {
        public override void Use(GameObject user, Vector3 direction)
        {
            base.Use(user, direction);
        }
    }
    
    // 단점 : 한종류의 총알밖에 안들어감.
    //public class Magazine<AmmoType> : Item
    //    where AmmoType : Bullet
    public class Magazine : Item
    {
        // 작동방식 : Queue처럼 행동합니다.
        // 탄알이 넣어지면, 안쪽으로 들어가지고, 가장 나중에 넣은 탄부터 나옵니다.
        // 탄알읜 겹쳐지지 않습니다.

        public int ammoCount;
        public int capacity; // subItem의 인덱스입니다.

        public Magazine() : base()
        {
            ammoCount = 0;
            capacity = 10;
            subItems = new ItemList();
            subItems.items = new Item[capacity];
        }
        //public AmmoType Pop()
        public Item Pop()
        {
            if (ammoCount > 0)
            {
                ammoCount--;
                //return subItems.items[ammoCount] as AmmoType;
                return subItems.items[ammoCount];
            }
            return null;
        }
        //public bool Push(AmmoType item)
        public bool Push<BulletType>(BulletType item) where BulletType : Bullet
        {
            if (ammoCount >= capacity) return false;

            if (subItems == null)
            {
                Hack.Err("서브아이템 객제가 존재하지 않습니다.");
            }
            if (subItems.items == null)
            {
                Hack.Say("서브아이템 객체의 멤버변수가 존재하지 않습니다.");
            }
            if (item == null)
            {
                Hack.Err("아이템이 비어있습니다.");
            }
            subItems.items[ammoCount] = item;
            Hack.Say($"subItems[{ammoCount}] = <{typeof(BulletType).Name}>({item.name})");
            ammoCount++;
            return true;
        }
    }

    //public class MagazineBuilder<MagType, BulletType> : ItemBuilder<MagType>
    public class MagazineBuilder<MagType> : ItemBuilder<MagType>
    //    where MagType : Magazine<BulletType>, new()
        where MagType : Magazine, new()
    //    where BulletType : Bullet
    {
        public MagazineBuilder() : base()
        {
            returnValue.subItems = new ItemList();
            returnValue.capacity = 0;
            returnValue.ammoCount = 0;
            returnValue.subItems.items = new Item[0];
        }

        //public MagazineBuilder<MagType, BulletType> SetCapacity(int size)
        public MagazineBuilder<MagType> SetCapacity(int size)
        {
            returnValue.capacity = size;
            returnValue.subItems.items = new Item[returnValue.capacity];
            return this;
        }
        //public MagazineBuilder<MagType, BulletType> Fill(BulletType bullet)
        public MagazineBuilder<MagType> Fill<BulletType>(BulletType bullet) where BulletType : Bullet, new()
        {
            return Fill(bullet, returnValue.capacity);
        }
        //public MagazineBuilder<MagType, BulletType> Fill(BulletType bullet, int count)
        public MagazineBuilder<MagType> Fill<BulletType>(BulletType bullet, int count) where BulletType : Bullet, new()
        {
            for (int i = 0; i < count; ++i)
            {
                returnValue.Push(bullet.DeepCopy());
            }
            return this;
        }
    }

    [Serializable]
    public class Shotable : Item
    {
        protected GameObject Shot(GameObject user, Vector3 direction)
        {
            GameObject instantiatedObject = Instantiate(bulletGo, user.transform.position, Quaternion.Euler(direction));
            BulletController m_bulletComponent = instantiatedObject.GetComponent<BulletController>();
            m_bulletComponent.Init(direction);
            user.GetComponent<UnitController>().PlaySound(UnitController.SoundName.gunShot);
            return instantiatedObject;
        }
    }

    [Serializable]
    public class Bullet : Shotable, IBullet
    {
        public bool isUsed;
        public int damage;

        public void Active(GameObject user, Vector3 direction)
        {
            Hack.Say("Bullet.Active()");

            if (isUsed)
            {
                Hack.Say("탄알을 사용할 수 없습니다.");
                return;
            }
            ThrowBullet(user, direction);
            isUsed = true;
        }

        virtual protected void ThrowBullet(GameObject user, Vector3 direction)
        {
            Hack.Say("Bullet.ThrowBullet()");
            Shot(user, direction).GetComponent<BulletController>().damage = damage;
        }
    }

    public class BulletBuilder<BulletType> : ItemBuilder<BulletType> where BulletType : Bullet, new()
    {
        public BulletBuilder() : base()
        {
            returnValue.isUsed = false;
            returnValue.damage = 10;
            returnValue.name = "bullet";
        }
        public BulletBuilder<BulletType> SetDamage(int damage)
        {
            returnValue.damage = damage;
            return this;
        }
    }

    [Serializable]
    public class ShotGunShell : Bullet
    {
        public int beadCount;
        public float spreadAngleMax;

        protected override void ThrowBullet(GameObject user, Vector3 direction)
        {
            Hack.Say($"ShotGunShell.ThrowBullet() : beadCount = {beadCount}");
            for (int i = 0; i < beadCount; ++i)
            {
                Direction newAngle = new Direction();
                newAngle.Forward = direction;
                newAngle.AngleDegree += UnityEngine.Random.Range(-spreadAngleMax, spreadAngleMax);
                Shot(user, newAngle.Forward);
            }
        }
    }

    public class ShotGunShellBuilder<ShotGunShellType> : BulletBuilder<ShotGunShellType>
        where ShotGunShellType : ShotGunShell, new()
    {
        public ShotGunShellBuilder() : base()
        {
            Hack.Say($"new ShotGunShellBuilder()");

            returnValue.beadCount = 12;
            returnValue.spreadAngleMax = 0.5f;
            returnValue.name = "shotgun Shell";
        }

        public ShotGunShellBuilder<ShotGunShellType> SetBeadCount(int beadCount)
        {
            returnValue.beadCount = beadCount;
            return this;
        }

        public ShotGunShellBuilder<ShotGunShellType> SetSpreadAngleMax(float spreadAngleMax)
        {
            returnValue.spreadAngleMax = spreadAngleMax;
            return this;
        }
    }

    [Serializable]
    public class Birdshot : ShotGunShell
    {
        // 납탄의 크기가 작고, 갯수가 많습니다.
    }

    [Serializable]
    public class BuckShot : ShotGunShell
    {
        // 납탄의 크기가 크고, 갯수가 적습니다.
    }

    [Serializable]
    public class Slug : ShotGunShell
    {
        // 단 하나의 납덩이입니다,.
    }

    [Serializable]
    public class Gun : Shotable
    {
        public const int AMMO_CHAMBER_INDEX = 0;
        public const int MAGAZINE_INDEX = 1;
        // 임시 값입니다. 만약 인벤토리 시스템이 갖춰졌으면 제거됩니다.
        public int ammoMagazine = 3;

        protected UnitController.SoundName reloadSound = UnitController.SoundName.gunReload1;

        public override void Use(GameObject user, Vector3 direction)
        {
            if (subItems.items[AMMO_CHAMBER_INDEX] == null)
            {
                Hack.Say("챔버에 탄알이 없습니다.");
                return;
            }

            // 챔버 속 탄환을 발사합니다.
            IBullet one = subItems.items[AMMO_CHAMBER_INDEX] as IBullet;
            if (one == null)
            {
                Hack.Say("bullet as 박싱 실패!");
            }
            one.Active(user, direction);
            //Shot(user, direction);

            // 탄창에 탄환을 하나 꺼내서 챔버에 넣습니다.
            if (subItems.items[MAGAZINE_INDEX] == null)
            {
                Hack.Say("Subitem의 탄알에 아이템이 없습니다.");
                return;
            }
            if (subItems.items[MAGAZINE_INDEX].stackCount < 1)
            {
                Hack.Say("Subitem의 탄창이 비었습니다.");
                return;
            }

            M_LoadBullet();
        }

        public override void Reload(GameObject user, Vector3 direction)
        {
            M_SetMagazine(ref subItems.items[MAGAZINE_INDEX]);

            user.GetComponent<UnitController>().PlaySound(reloadSound);
            if (subItems.items[AMMO_CHAMBER_INDEX] == null) { M_LoadBullet(); }
            else if (((Bullet)subItems.items[AMMO_CHAMBER_INDEX]).isUsed) { M_LoadBullet(); }
        }
        
        // 클립의 탄환을 장전합니다.
        protected virtual void M_LoadBullet()
        {
            // 탄창에 탄환을 하나 꺼내서 챔버에 넣습니다
            //Magazine<Bullet> mag = subItems.items[MAGAZINE_INDEX] as Magazine<Bullet>;
            Magazine mag = subItems.items[MAGAZINE_INDEX] as Magazine;
            if (mag != null)
            {
                subItems.items[AMMO_CHAMBER_INDEX] = mag.Pop();
            }
            else
            {
                Hack.Err("캐스팅 실패!");
            }
        }
        protected virtual void M_SetMagazine(ref Item magazineSlot)
        {
            //magazineSlot = new MagazineBuilder<Magazine<Bullet>, Bullet>()
            magazineSlot = new MagazineBuilder<Magazine>()
                .SetCapacity(ammoMagazine)
                .Fill(
                    new BulletBuilder<Bullet>()
                    .Build()
                )
                .SetStackCount(1)
                .Build();
        }
    }

    public class GunBuilder<GunType> : ItemBuilder<GunType> where GunType : Gun, new()
    {
        public GunBuilder() : base()
        {
            returnValue.subItems.items = new Item[2];
        }

        virtual public GunBuilder<GunType> SetMagazineSize(int size)
        {
            returnValue.ammoMagazine = size;
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
    // 연속 사격이 가능한 화기입니다. 마우스 홀드로 작동합니다. 사격을 할수록 반동으로 인하여 탄퍼짐이 있습니다.
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
                reducePerSecond = 10.0f,
                max = 15.0f
            };

            nextShotTerm = 0.5f;
            nextShotLeftTime = 0.0f;
        }
        public override void Use(GameObject user, Vector3 direction)
        {
            
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
            if (subItems.items[MAGAZINE_INDEX] == null)
            {
                return;
            }
            if (subItems.items[MAGAZINE_INDEX].stackCount <= 0)
            {
                return;
            }

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
            newVec.AngleDegree += UnityEngine.Random.Range(-wideness, wideness);
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

    public class ShotGun : Gun
    {
        // 샷건의 탄환에 따라 달라집니다. -> subItem의 use를 호출합니다.

        public int bulletCount = 0;

        //public override void Use(GameObject user, Vector3 direction)
        //{
        //    if (subItems.items)
        //    {
        //        
        //    }
        //    if (bulletCount > 0)
        //    {
        //        bulletCount--;
        //        ShotGunShell one = subItems.items[bulletCount] as ShotGunShell;
        //        if (one == null)
        //        {
        //            Hack.Say("샷건 as 박싱 실패!");
        //            return;
        //        }
        //        one.Active(user, direction);
        //    }
        //}

        //public override void Reload(GameObject user, Vector3 direction)
        //{
        //    Hack.Say($"ammomag = {ammoMagazine}, size = {subItems.items.Length}");
        //    for (int index = 0; index < ammoMagazine; ++index)
        //    {
        //        this.subItems.items[index] =
        //            new ShotGunShellBuilder<ShotGunShell>()
        //            .SetSpreadAngleMax(1.0f)
        //            .Build();
        //    }
        //    bulletCount = ammoMagazine;
        //    user.GetComponent<UnitController>().PlaySound(UnitController.SoundName.gunReload1);
        //    if (subItems.items[AMMO_CHAMBER_INDEX] == null) { M_LoadBullet(); }
        //    else if (subItems.items)
        //}
        protected override void M_SetMagazine(ref Item magazineSlot)
        {
            Hack.Say("샷건 리로딩!");
            magazineSlot =
                //new MagazineBuilder<Magazine<ShotGunShell>, ShotGunShell>()
                new MagazineBuilder<Magazine>()
                    .SetCapacity(8)
                    .Fill(
                        new ShotGunShellBuilder<ShotGunShell>()
                            .SetBeadCount(6)
                            .SetSpreadAngleMax(2.0f)
                            .Build()
                        )
                    .Build();
        }
    }

    public class ShotGunBuilder<ShotGunType> : GunBuilder<ShotGunType>
        where ShotGunType : ShotGun, new()
    {
        public ShotGunBuilder() : base()
        {
            returnValue.bulletCount = 0;
            returnValue.subItems.items[1]
                //= new MagazineBuilder<Magazine<ShotGunShell>, ShotGunShell>()
                = new MagazineBuilder<Magazine>()
                .SetCapacity(6)
                .Build();
        }
#warning 되게 나쁜 디자인입니다. 망치질 하다 말았어요. => Magazine을 넣자.
        public override GunBuilder<ShotGunType> SetMagazineSize(int size)
        {
            if (returnValue.subItems == null) returnValue.subItems = new ItemList();
            returnValue.subItems.items = new Item[size];
            returnValue.ammoMagazine = size;
            return this;
        }

        public void SetMagFull()
        {
            returnValue.subItems.items[1]
                //= new MagazineBuilder<Magazine<ShotGunShell>, ShotGunShell>()
                = new MagazineBuilder<Magazine>()
                .SetCapacity(8)
                .Fill(
                    new ShotGunShellBuilder<ShotGunShell>()
                    .SetSpreadAngleMax(1.0f)
                    .Build(),
                    returnValue.ammoMagazine
                    )
                .Build();

        }
    }


}


// 총기 공통 서브아이템
// 탄알

// 권총 서브아이템
//