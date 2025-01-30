using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Input : MonoBehaviour
{
    private InputDevice targetDevice;

    public Vector2 Axis2D;


    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightConChar = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightConChar, devices);

        targetDevice = devices[index: 0];
    }

    private void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        Axis2D = primary2DAxisValue;

    }
}
