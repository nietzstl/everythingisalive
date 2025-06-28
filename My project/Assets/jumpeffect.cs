using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class jumpeffect : MonoBehaviour
{
    public float bounceHeight = 0.5f; // 弹跳高度
    public float bounceSpeed = 3f;   // 弹跳速度
    public float bouncescale = 0.5f; // 弹跳缩放比例
    private Vector3 originalPos;
    private Vector3 originalScale;
    private float timer;

    void Start()
    {
        originalPos = transform.localPosition;
        originalScale = transform.localScale;
    }
    void Update()
    {
        timer += Time.deltaTime * bounceSpeed;
        float yOffset = Mathf.Sin(timer) * bounceHeight;
        float yScale = Mathf.Sin(timer) * bouncescale;
        transform.localPosition = originalPos + Vector3.up * yOffset;
        transform.localScale= originalScale + Vector3.up * yScale;
    }
}
