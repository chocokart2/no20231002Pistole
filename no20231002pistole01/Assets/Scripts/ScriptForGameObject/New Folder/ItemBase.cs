using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : TypeDefinition
{
    static public GameObject fistGo;
    static public GameObject bulletGo;


    // 게임매니저 전용 함수
    static public void InitGameObject()
    {
        Hack.Say("ItemBase.InitGameObject()");

        fistGo = Resources.Load<GameObject>("Prefabs/Fist");
        if (fistGo == null ) { Hack.Err("ItemBase.InitGameObject() : fistGo를 로드할 수 없음."); }
        bulletGo = Resources.Load<GameObject>("Prefabs/Bullet");
        if (bulletGo == null ) { Hack.Err("ItemBase.InitGameObject() : bulletGo를 로드할 수 없음."); }
    }



    public class Item
    {
        public string name;
        public ItemList subItems;
        public int stackCount;

        public virtual void Update() { }
        public virtual void Use(GameObject user, Vector3 direction) { }
        public virtual void UseHold(GameObject user, Vector3 direction) { }
        public virtual void Reload(GameObject user, Vector3 direction) { }
    }

    public class ItemBuilder<T> where T : Item, new()
    {
        protected T returnValue;

        public ItemBuilder()
        {
            returnValue = new T();
        }

        public ItemBuilder<T> SetName(string val)
        {
            returnValue.name = val;
            return this;
        }
        public ItemBuilder<T> SetSubItems(ItemList val)
        {
            returnValue.subItems = val;
            return this;
        }
        public ItemBuilder<T> SetStackCount(int val)
        {
            returnValue.stackCount = val;
            return this;
        }
        public Item Build()
        {
            return returnValue;
        }
    }

    public class ItemList
    {
        // 아이템은 내부의 다른 아이템을 포함시킬 수 있지만, 타입에 따라서 어떤 아이템이 포함되어야 하는지에 대한
        // 탬플릿같은게 존재했으면 좋겠다.
        // 예를 들어 권총이 있다면, 권총 내부에는 탄알집과 탄알, 그리고 그외 각종 부품이 정해진 자리에 들어있어야 하고
        // 다른 장비들에는 각자의 규칙 대로 아이템이 들어가야 하고, 밎지 않는 아이템은 들어가선 안 된다.

        public Item[] items;
    }
    public class Inventory : ItemList
    {
        public GameObject user;
        public int itemCursor;

        public void Update()
        {
            for (int index = 0; index < items.Length; ++index)
            {
                items[index].Update();
            }
        }
        public void Use(Vector3 direction) => items[itemCursor].Use(user, direction);
        public void UseHold(Vector3 direction) => items[itemCursor].UseHold(user, direction);
        public void Reload(Vector3 direction) => items[itemCursor].Reload(user, direction);
        public void SetItem(int index, Item item)
        {
            items[index] = item;
        }
        public void SetItem<ItemType>(int index, ItemType item) where ItemType : Item
        {
            items[index] = item;
        }
    }

}
