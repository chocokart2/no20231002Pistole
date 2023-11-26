using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack : MonoBehaviour
{
    
    



    static public bool isDebugging = true;




    static public void Err(object message)
    {
        Debug.Log($"<!!!> ERROR : {message}");
    }
    static public void Say(object message)
    {
        if (isDebugging)
        {
            Debug.Log($">> DEBUG : {message}");
        }
    }







    // Start is called before the first frame update
    void Start()
    {
        ItemBase.InitGameObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
