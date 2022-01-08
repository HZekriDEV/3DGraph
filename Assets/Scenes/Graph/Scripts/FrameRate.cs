using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameRate : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;

    public enum DisplayMode { FPS, MS }

    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    int frames;
    float duration;

    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames++;
        duration += frameDuration;

        if(duration >= sampleDuration)
        {
            if(displayMode == DisplayMode.FPS)
                display.SetText("FPS\n{0:0}", frames / duration);
            else
                display.SetText("MS\n{0:1}", 1000f * duration / frames);

            frames = 0;
            duration = 0f;
        }
        
    }
}
