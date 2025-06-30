using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public float waitTime = 2f;
    public Renderer arrowRenderer;

    void Awake()
    {
        arrowRenderer = transform.GetChild(0).GetComponent<Renderer>();
    }
    public void Hide()
    {
        arrowRenderer.enabled=false;
    }
    public void Show()
    {
        arrowRenderer.enabled=true;
    }
}
