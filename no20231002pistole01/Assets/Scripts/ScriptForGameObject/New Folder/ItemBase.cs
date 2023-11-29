using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : TypeDefinition
{
    static public GameObject fistGo;
    static public GameObject bulletGo;


    // ���ӸŴ��� ���� �Լ�
    static public void InitGameObject()
    {
        Hack.Say("ItemBase.InitGameObject()");

        fistGo = Resources.Load<GameObject>("Prefabs/Fist");
        if (fistGo == null ) { Hack.Err("ItemBase.InitGameObject() : fistGo�� �ε��� �� ����."); }
        bulletGo = Resources.Load<GameObject>("Prefabs/Bullet");
        if (bulletGo == null ) { Hack.Err("ItemBase.InitGameObject() : bulletGo�� �ε��� �� ����."); }
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
        // �������� ������ �ٸ� �������� ���Խ�ų �� ������, Ÿ�Կ� ���� � �������� ���ԵǾ�� �ϴ����� ����
        // ���ø������� ���������� ���ڴ�.
        // ���� ��� ������ �ִٸ�, ���� ���ο��� ź������ ź��, �׸��� �׿� ���� ��ǰ�� ������ �ڸ��� ����־�� �ϰ�
        // �ٸ� ���鿡�� ������ ��Ģ ��� �������� ���� �ϰ�, �G�� �ʴ� �������� ���� �� �ȴ�.

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
