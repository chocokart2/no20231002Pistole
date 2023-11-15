using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // 방향을 나타내는 클래스입니다.
    // 방향이나 벡터를 통해 값을 저장하며, 해당 값을 저장할때마다, 각도와 방향을 동시에 저장합니다.
    public class Direction
    {
        public float AngleDegree // other direction
        {
            get => mAngleDegree;
            set // SUS
            {
                mAngleDegree = value;
                mForward = mGetForward(value);
                mRight = mGetRight(value);
            }
        }
        public Vector3 Forward // W, S key
        {
            get => mForward;
            set // SUS
            {
                mAngleDegree = mGetAngle(value.z, value.x);
                mForward = value;
                mRight = mGetRight(mAngleDegree);
            }
        }
        public Vector3 Right // A, D key
        {
            get => mRight;
            set // SUS
            {
                mAngleDegree = 90.0f + mGetAngle(value.z, value.x);
                mForward = mGetForward(mAngleDegree);
                mRight = value;
            }
        }

        private float mAngleDegree;
        private Vector3 mForward;
        private Vector3 mRight;

        private float mGetAngle(float z, float x) => // SUS
            Mathf.Atan2(z, x) * Mathf.Rad2Deg;
        private Vector3 mGetForward(float angle) // SUS
        {
            return new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle),
                0,
                Mathf.Sin(Mathf.Deg2Rad * angle));
        }
        private Vector3 mGetRight(float angle) // SUS
        {
            return new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * angle),
                0,
                -Mathf.Cos(Mathf.Deg2Rad * angle));
        }
    }
    public enum Direction90
    {
        forward,
        back,
        left,
        right
    }

    float speed = 3.0f;
    Direction unitViewDirection;
    //component
    Rigidbody myRigidbody;

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

    // Start is called before the first frame update
    void Start()
    {
        unitViewDirection = new Direction();
        unitViewDirection.Forward = Vector3.forward;
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move(Direction90.forward);
        //unitViewDirection.AngleDegree += Time.deltaTime * 60;
    }
}
