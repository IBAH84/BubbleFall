using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private LineRenderer ml_lineGameOver;
    [SerializeField] private Ball c_ball;
    public List<Ball> l_ball = new List<Ball>();
    [SerializeField] private Material[] ma_mat;
    [SerializeField] [Range(20, 50)] private int l_length;
    public static BallController _get;

    public void f_Init()
    {
        if (_get == null)
        {
            _get = this;
        }
        f_SetColor(Color.green);
        transform.localPosition = new Vector3(0, -3, 70);
        f_GenerateField();
    }

    public void f_Activ()
    {
        StartCoroutine("IEActiv");
    }

    private IEnumerator IEActiv()
    {
        AnimationCurve _muveZ = A_Curve.f_GetCurve(transform.localPosition.z, 35);
        float _step = 0;
        while (_step < 1)
        {
            _step += 0.4f * Time.deltaTime;
            transform.localPosition = new Vector3(0, -3, _muveZ.Evaluate(_step));
            yield return null;
        }

        GameMananger._get.f_DoneBallController();
    }

    public void f_GenerateField()
    {
        if (l_ball.Count > 0)
        {
            for (int i = 0; i < l_ball.Count; i++)
            {
                DestroyImmediate(l_ball[i]);
            }
        }

        l_ball.Clear();
        Vector2 _pos = new Vector2(0, 0);
        int _length;
        int _index = 0;
        for (int i = 0; i < 40; i++)
        {
            _length = i % 2 == 0 ? 9 : 10;
            _pos.x = i % 2 == 0 ? -4 : -4.5f;
            for (int j = 0; j < _length; j++)
            {
                l_ball.Add(Instantiate(c_ball.f_SetNewBall(_pos, i < 30, f_GetBallInfo(), "Ball_" + _index), transform));
                _pos.x += 1;
                _index++;
            }
            _pos.y -= 1;
        }

        _index = 0;
        for (int i = 0; i < 40; i++)
        {
            _length = i % 2 == 0 ? 9 : 10;
            for (int j = 0; j < _length; j++)
            {
                Ball[] _ballAround = new Ball[6];
                for (int c = 0; c < _ballAround.Length; c++)
                {
                    switch (c)
                    {
                        case 0: { _ballAround[c] = (i == 0 || (_length == 10 && j == _length - 1)) ? null : l_ball[_index - 9]; break; }
                        case 1: { _ballAround[c] = j == _length - 1 ? null : l_ball[_index + 1]; break; }
                        case 2: { _ballAround[c] = (i == 39 || (_length == 10 && j == _length - 1)) ? null : l_ball[_index + 10]; break; }
                        case 3: { _ballAround[c] = (i == 39 || (_length == 10 && j == 0)) ? null : l_ball[_index + 9]; break; }
                        case 4: { _ballAround[c] = j == 0 ? null : l_ball[_index - 1]; break; }
                        case 5: { _ballAround[c] = (i == 0 || (_length == 10 && j == 0)) ? null : l_ball[_index - 10]; break; }
                    }
                }
                l_ball[_index].f_SetBallAround(_ballAround);
                _index++;
            }

        }
    }

    public S_BallInfo f_GetBallInfo()
    {
        int _random = Random.Range(0, ma_mat.Length);
        return new S_BallInfo(ma_mat[_random], _random);
    }

    public void f_SetNewBall(Ball _ball, int _id)
    {
        HashSet<Ball> _ba = new HashSet<Ball>(_ball.f_GetAround(_id, new List<Ball>()));
        List<Ball> _b = new List<Ball>(_ba);
        int _count = 0;
        if (_b.Count > 2)
        {
            for (int i = 0; i < _b.Count; i++)
            {
                _count++;
                _b[i].f_SetNewBall();
            }
            _b.Clear();
            for (int i = 0; i < l_ball.Count; i++)
            {
                if (l_ball[i].f_CheckBallEmpty() != null)
                {
                    _b.Add(l_ball[i]);
                }
            }
            for (int i = 0; i < _b.Count; i++)
            {
                _count++;
                _b[i].f_SetNewBall();
            }
        }
        StartCoroutine("IEMuve");
    }

    private IEnumerator IEMuve()
    {
        PointTouch._get.m_clik = true;
        AnimationCurve _muveZ = A_Curve.f_GetCurve(transform.localPosition.z, transform.localPosition.z - 0.5f);
        float _step = 0;
        while (_step < 1)
        {
            _step += 2 * Time.deltaTime;
            transform.localPosition = new Vector3(0, transform.localPosition.y, _muveZ.Evaluate(_step));
            yield return null;
        }
        PointTouch._get.m_clik = false;

        for (int i = 0; i < l_ball.Count; i++)
        {
            if (!l_ball[i].f_CheckDistance(ml_lineGameOver.transform))
            {
                GameMananger._get.f_GameOver();
                f_SetColor(Color.red);
                break;
            }
        }
    }

    private void f_SetColor(Color _color)
    {
        ml_lineGameOver.endColor = _color;
        ml_lineGameOver.startColor = _color;
    }
}

public struct S_BallInfo
{
    public Material m_mat;
    public int m_id;

    public S_BallInfo(Material _mat, int _id)
    {
        m_mat = _mat;
        m_id = _id;
    }
}
