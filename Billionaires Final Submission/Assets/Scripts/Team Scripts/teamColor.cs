using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamColor : MonoBehaviour
{
    public enum ColorChoice
    {
        Red,
        Yellow,
        Green,
        Blue
    }

    [Header("Select a Color")]
    public ColorChoice selectedColor;
}
