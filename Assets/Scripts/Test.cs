using System.Collections;
using System.Collections.Generic;
using SDK.Core;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SDKManager.sharedInst.InitDemo();

        SDKManager.sharedInst.DemoInit();
        SDKManager.sharedInst.DemoLogin();
        SDKManager.sharedInst.DemoPay("123", 1, 1, "", "", "", "", "");
    }

    // Update is called once per frame
    void Update()
    {
    }
}