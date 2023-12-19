using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : TypeDefinition
{
    float speed = 7.0f;
    float lifeTime = 2.0f;
    Direction direction;

    public int damage = 0;

    public void Init()
    {
        direction = new Direction();
        direction.Forward = new Vector3(1, 0, 0);
    }
    public void Init(Vector3 vec)
    {
        Init();
        direction.Forward = vec;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction.Forward * speed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0.0f) Destroy(gameObject);
    }
}
