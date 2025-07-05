using UnityEngine;

public static class A_Curve
{
    public static AnimationCurve f_GetCurve(float _start, float _done)
    {
        Keyframe[] _key = new Keyframe[2];
        _key[0] = new Keyframe(0, _start);
        _key[1] = new Keyframe(1, _done);
        AnimationCurve _curve = new AnimationCurve(_key);
        return _curve;
    }

    public static AnimationCurve f_GetCurve(float _start, float _centr, float _done)
    {
        Keyframe[] _key = new Keyframe[3];
        _key[0] = new Keyframe(0, _start);
        _key[1] = new Keyframe(0.5f, _centr);
        _key[2] = new Keyframe(1, _done);
        AnimationCurve _curve = new AnimationCurve(_key);
        return _curve;
    }
}
