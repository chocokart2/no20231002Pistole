using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    static public GameObject fistGo;
    static public GameObject bulletGo;

    // 게임매니저 전용 함수
    static public void InitGameObject()
    {
        fistGo = Resources.Load<GameObject>("Prefabs/Fist");
        bulletGo = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    public class Item
    {
        public string name;
        public ItemList subItems;

        public virtual void Use(GameObject user, Vector3 direction) { }
        public virtual void Reload(GameObject user, Vector3 direction) { }
    }

    public class ItemList
    {
        public Item[] items;
    }
    public class Inventory : ItemList
    {
        public GameObject user;
        public int itemCursor;

        public void Use(Vector3 direction) => items[itemCursor].Use(user, direction);
        public void Reload(Vector3 direction) => items[itemCursor].Reload(user, direction);
    }

}
