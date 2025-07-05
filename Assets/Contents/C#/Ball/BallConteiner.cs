using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallConteiner : MonoBehaviour
{
    [SerializeField] private MuveBall c_ball;
    [SerializeField] [Range(1, 100)] private float m_speed;
    private List<MuveBall> l_ball = new List<MuveBall>();
    private MuveBall c_balUsed;
    private bool m_rotate;

    public static BallConteiner _get;

    public void f_Init()
    {
        if (_get == null)
        {
            _get = this;
        }
        c_balUsed = null;

        l_ball.Clear();
        l_ball.Add(f_GetBall().f_Init(1.05f, BallController._get.f_GetBallInfo()));
        l_ball.Add(f_GetBall().f_Init(-1.05f, BallController._get.f_GetBallInfo()));

        m_rotate = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void f_Rotate()
    {
        if (m_rotate) return;

        StartCoroutine("IERotate");
    }

    private IEnumerator IERotate()
    {
        if (l_ball.Count > 1)
        {
            MuveBall _ball = l_ball[0];
            l_ball[0] = l_ball[1];
            l_ball[1] = _ball;
        }

        m_rotate = true;
        AnimationCurve _rotate = A_Curve.f_GetCurve(transform.localEulerAngles.z, transform.localEulerAngles.z + 180);

        float _step = 0;
        while (_step < 1)
        {
            _step += 3 * Time.deltaTime;
            transform.localEulerAngles = new Vector3(0, 0, _rotate.Evaluate(_step));
            yield return null;
        }

        if (l_ball.Count == 1)
        {
            l_ball.Add(f_GetBall().f_Init(l_ball[0].transform.localPosition.y > 0 ? -1.05f : 1.05f, BallController._get.f_GetBallInfo()));
        }

        m_rotate = false;
    }

    public void f_SetBall(LineRenderer _line, Ball _stopBall)
    {
        if (!_line.enabled)
        {
            return;
        }
        l_ball[0].f_Muve(_line, m_speed, _stopBall);
        c_balUsed = l_ball[0];
        l_ball.RemoveAt(0);
        f_Rotate();
    }

    private MuveBall f_GetBall()
    {
        return Instantiate(c_ball, transform);
    }

    public bool f_CheckClik() { return c_balUsed == null; }
    public void f_RemuveBall() { c_balUsed = null; }
}
