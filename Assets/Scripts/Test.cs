using SDK.Core;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SDKManager.sharedInst.Demo_Init();
        
        SDKManager.sharedInst.Demo_Login();
        SDKManager.sharedInst.Demo_Pay("123", 1, 1, "", "", "", "", "");
    }

    // Update is called once per frame
    void Update()
    {
    }
}