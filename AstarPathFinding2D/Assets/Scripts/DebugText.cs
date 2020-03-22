using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private RectTransform arrow;

    public RectTransform MyArrow { get => arrow; set => arrow = value; }
}
