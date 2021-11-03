using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mydebug : MonoBehaviour
{
    public static Mydebug mydebug;
    public Text textbox;
    // Start is called before the first frame update
    void Start()
    {
        mydebug = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MyPrint(string txt)
    {
        textbox.text = txt;
    }
}
