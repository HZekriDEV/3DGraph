using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);

    static Function[] functions = { Wave, MultiWave, Ripple, Sphere, Torus };

    public static string[] funcNames = { "F(u, v, t) = Sin(π·(u + v + t)",
                                         "F(u, v, t) = (2/5)(Sin(π·(u + t)) + (1/2)(Sin(2·π·(v + t))) + (Sin(π·(u + v + (1/4)·t)))",
                                         "F(u, v, t) = Sin(π·(4·(Sqrt((u·u) + (v·v))) - t))/(1 + 10·(Sqrt((u·u) + (v·v))))",
                                         "",
                                         "" };
    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus }
    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }
    public static FunctionName GetNextFunction(FunctionName name)
    {
        if ((int)name < functions.Length - 1)
            return name + 1;
        else 
            return 0;
    }
    public static FunctionName GetRandomFunctionName(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }
    public static Vector3 Morph(float u, float v, float t, Function from , Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }
    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }   
    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + t)) + (Sin(2 * PI * (v + t)) * (1f / 2f)) + (Sin(PI * (u + v + 0.25f * t)));
        p.y *= (1f / 2.5f);
        p.z = v;

        return p;
    }
    public static Vector3 Ripple(float u, float v, float t)
    {
        float d = Sqrt(u * u + v * v);

        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t));
        p.y /= (1f + 10f * d);
        p.z = v;
        return p; 
    }
    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(PI * 0.5f * v);
        
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    public static Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        float r1 = (7 + Sin(PI * (6 * u + (t / 2)))) / 10;
        float r2 = (3 + Sin(PI * (8 * u + 4 * v + 2 * t))) / 20; ;
        float s = r1 + r2 * Cos(PI * v);

        p.x = s * Sin(PI * u); ;
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
}
