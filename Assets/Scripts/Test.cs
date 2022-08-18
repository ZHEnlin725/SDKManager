using SDK.Core;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SDKManager.sharedInst.Demo.Init();
        
        SDKManager.sharedInst.Demo.Login();
        SDKManager.sharedInst.Demo.Pay("123", 1, 1, "", "", "", "", "");
    }

    // Update is called once per frame
    void Update()
    {
    }
}