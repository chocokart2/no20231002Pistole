using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DeepCopyExtension
{
    public static T DeepCopy<T>(this T source) where T : new()
    {
        if (!typeof(T).IsSerializable)
        {
            throw new NotSerializableTypeException($"{typeof(T).Name} 타입은 직렬화할 수 없습니다.");
        }
        object result = null;
        using (var ms = new System.IO.MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, source);
            ms.Position = 0;
            result = (T)formatter.Deserialize(ms);
            ms.Close();
        }
        return (T)result;
    }
}

[System.Serializable]
public class NotSerializableTypeException : Exception
{
    public NotSerializableTypeException() { }
    public NotSerializableTypeException(string message) : base(message) { }
    public NotSerializableTypeException(string message, Exception inner) : base(message, inner) { }
    protected NotSerializableTypeException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class ExtensionMethod : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
