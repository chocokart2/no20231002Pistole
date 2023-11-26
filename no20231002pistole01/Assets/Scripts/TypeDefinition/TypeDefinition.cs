using System;
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
        public string Words
        {
            get;
            set;
        }
        public int size
        {
            get
            {
                throw new NotImplementedException();
                return -1;
            }
        }

        private const long WORD_ID_ZERO = 0;
        private const long WORD_ID_NINE = 9;
        private const long WORD_ID_UPPER_A = 10;
        private const long WORD_ID_UPPER_Z = 35;
        private const long WORD_ID_LOWER_A = 36;
        private const long WORD_ID_LOWER_Z = 61;
        private const char NOT_CONVERTIBLE = '\0';
        private long m_value; // 0b_SSSS_CCCCCC_CCCCCC_CCCCCC_CCCCCC_CCCCCC_CCCCCC_CCCCCC_CCCCCC_CCCCCC_CCCCCC;

        public static bool operator ==(StringNum left, StringNum right) => left.m_value == right.m_value;
        public static bool operator !=(StringNum left, StringNum right) => left.m_value != right.m_value;

        
        public int Set(string words)
        {
            throw new NotImplementedException();

            //10ĭ������ �Է���

            return 0;

            // -1 : ����� 10�� ���
            // -2 : �Է��� �� ���� ���ڰ� ����.
        }
        public void Set(int number)
        {
            Set(number.ToString());
        }

        private long M_Convert(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'Z') return c - 'A' + WORD_ID_UPPER_A;
            if (c >= 'a' && c <= 'z') return c - 'a' + WORD_ID_LOWER_A;
            if (c == ' ') return 62;
            if (c == '-') return 63;
            return -1;
        }
        private char M_Convert(long l)
        {
            if (l >= WORD_ID_ZERO && l <= WORD_ID_NINE) return (char)('0' + l);
            if (l >= WORD_ID_UPPER_A && l <= WORD_ID_UPPER_Z) return (char)('A' + l - 10);
            if (l >= WORD_ID_LOWER_A && l <= WORD_ID_LOWER_Z) return (char)('a' + l - 36);
            if (l == 62) return ' ';
            if (l == 63) return '-';
            return NOT_CONVERTIBLE;
        }
    }
}