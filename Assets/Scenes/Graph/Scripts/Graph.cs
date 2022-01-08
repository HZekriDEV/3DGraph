using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Graph : MonoBehaviour
{
    [SerializeField] 
    Transform pointPrefab;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField, Range(10, 200)]
    int resolution = 50;

    [SerializeField]
    FunctionLibrary.FunctionName function = default;

    public enum TransitionMode { Cycle, Random, None }

    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    float duration;
    bool transitioning;

    FunctionLibrary.FunctionName transitionFunction;

    Transform[] points;

    private void Awake()
    {
        float step = 2f / resolution;
        var scale = Vector3.one * step;

        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }
    private void Update()
    {
        duration += Time.deltaTime;
        if(transitioning)
        {
            if (duration >= functionDuration)
            {
                duration -= functionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }
        if (transitioning)
            UpdateFunctionTransition();
        else
            UpdateFunction();
    }
    void PickNextFunction()
    {
        if (transitionMode == TransitionMode.Cycle)
            function = FunctionLibrary.GetNextFunction(function);
        else if (transitionMode == TransitionMode.Random)
            function = FunctionLibrary.GetRandomFunctionName(function);  
    }    
    void UpdateFunction()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);

        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, Time.time);
        }
        text.SetText(FunctionLibrary.funcNames[(int)function]);
    }
    void UpdateFunctionTransition()
    {
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction), to = FunctionLibrary.GetFunction(function);
        float progress = duration / transitionDuration;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.Morph(u, v, Time.time, from, to, progress);
        }
        text.SetText(FunctionLibrary.funcNames[(int)function]);
    }
}
