using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private RectTransform arrow;

    [SerializeField]
    private TextMeshPro f, g, h, p;


    public RectTransform MyArrow { get => arrow; set => arrow = value; }
    public TextMeshPro F { get => f; set => f = value; }
    public TextMeshPro G { get => g; set => g = value; }
    public TextMeshPro H { get => h; set => h = value; }
    public TextMeshPro P { get => p; set => p = value; }
    
}
