using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeDefinition : MonoBehaviour
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

    // 최대 10자까지 입력 가능
    // 대문자 26개, 소문자 26개, 숫자 10개, 공백' ' 1개, 짝대기 문자'-' 1개 = 64개
    // 각 칸마다 +- 26하면 대문자, 소문자 변환이 가능
    // + 해당 문자의 길이는 마지막 남는 4개의 비트로 표시할 수 있음.
    // 문자 하나당 64(비트 6개)로 판단하여 long(64비트)로 문자 10개를 담을 수 있음
    // 기능:
    // 문자열을 매개변수로 받으면 내부의 ulong 숫자로 변환함,
    // 내부의 ulong숫자를 문자열로 리턴해줌
    // 빠른 equals를 제공함.
    // 문자열 더하기를 제공
    // 문자열 길이를 제공
    // 인덱서 기능 제공
    // 해당 문자열의 임의의 원소에 더하기, 빼기 기능 제공
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

            //10칸까지만 입력함

            return 0;

            // -1 : 사이즈가 10을 벗어남
            // -2 : 입력할 수 없는 문자가 있음.
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