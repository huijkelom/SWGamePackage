using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCurves : MonoBehaviour
{
    public static LerpCurves Instance;

    [SerializeField] private AnimationCurve _Linear;
    [SerializeField] private AnimationCurve _EaseIn;
    [SerializeField] private AnimationCurve _EaseOut;
    [SerializeField] private AnimationCurve _EaseInOut;
    [SerializeField] private AnimationCurve _Bounce;

    private void Awake()
    {
        Instance = this;
    }

    public float Linear(float time)
    {
        return _Linear.Evaluate(time);
    }

    public float EaseIn(float time)
    {
        return _EaseIn.Evaluate(time);
    }

    public float EaseOut(float time)
    {
        return _EaseOut.Evaluate(time);
    }

    public float EaseInOut(float time)
    {
        return _EaseInOut.Evaluate(time);
    }

    public float Bounce(float time)
    {
        return _Bounce.Evaluate(time);
    }
}