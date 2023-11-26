using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeDefinition : MonoBehaviour
{

    // ������ ��Ÿ���� Ŭ�����Դϴ�.
    // �����̳� ���͸� ���� ���� �����ϸ�, �ش� ���� �����Ҷ�����, ������ ������ ���ÿ� �����մϴ�.
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

    // �ִ� 10�ڱ��� �Է� ����
    // �빮�� 26��, �ҹ��� 26��, ���� 10��, ����' ' 1��, ¦��� ����'-' 1�� = 64��
    // �� ĭ���� +- 26�ϸ� �빮��, �ҹ��� ��ȯ�� ����
    // + �ش� ������ ���̴� ������ ���� 4���� ��Ʈ�� ǥ���� �� ����.
    // ���� �ϳ��� 64(��Ʈ 6��)�� �Ǵ��Ͽ� long(64��Ʈ)�� ���� 10���� ���� �� ����
    // ���:
    // ���ڿ��� �Ű������� ������ ������ ulong ���ڷ� ��ȯ��,
    // ������ ulong���ڸ� ���ڿ��� ��������
    // ���� equals�� ������.
    // ���ڿ� ���ϱ⸦ ����
    // ���ڿ� ���̸� ����
    // �ε��� ��� ����
    // �ش� ���ڿ��� ������ ���ҿ� ���ϱ�, ���� ��� ����
    struct StringNum
    {

    }
}
