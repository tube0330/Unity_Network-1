using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class ColorSerial : MonoBehaviour
{
    public static byte[] SerializeColor(object targetObj)
    {
        Color color = (Color)targetObj;
        Quaternion colorQuaternion = new Quaternion(color.r, color.g, color.b, color.a);
    }
}
