using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private ParticleSystem mp_particle;
    [SerializeField] private ParticleSystemRenderer mr_r;
    public GameObject mg_ball;
    public Collider mc_collider;
    [SerializeField] private MeshRenderer mm_mesh;
    public List<Ball> c_ballAround;
    public int m_id;

    public Ball f_SetNewBall(Vector2 _pos, bool _work, S_BallInfo _info, string _name)
    {
        mp_particle.gameObject.SetActive(false);
        gameObject.name = _name;
        f_ActivCollider(_work);
        mg_ball.SetActive(_work);
        if (_work)
        {
            mm_mesh.material = _info.m_mat;
            m_id = _info.m_id;
        }

        transform.localPosition = new Vector3(_pos.x, 0, _pos.y);
        gameObject.SetActive(true);
        return this;
    }

    public void f_SetNewBall(S_BallInfo _info)
    {
        if (mg_ball.activeSelf)
        {
            return;
        }
        mm_mesh.material = _info.m_mat;
        m_id = _info.m_id;
        mg_ball.SetActive(true);
        f_ActivCollider(true);
    }

    public void f_SetNewBall()
    {
        mr_r.material = mm_mesh.material;
        mp_particle.gameObject.SetActive(true);
        mg_ball.SetActive(false);
        f_ActivCollider(false);
        Progress._get.f_AddProgress();
    }

    public void f_SetBallAround(Ball[] _ball)
    {
        c_ballAround = new List<Ball>();
        for (int i = 0; i < _ball.Length; i++)
        {
            if (_ball[i] != null)
            {
                c_ballAround.Add(_ball[i]);
            }
        }

        for (int i = 0; i < c_ballAround.Count; i++)
        {
            c_ballAround[i].f_ActivCollider(false);
        }
    }

    public void f_ActivBallsAround(bool _activ)
    {
        for (int i = 0; i < c_ballAround.Count; i++)
        {
            if (!c_ballAround[i].f_CheckBall())
            {
                c_ballAround[i].f_ActivCollider(_activ);
            }
            else
            {
                for (int j = 0; j < c_ballAround[i].c_ballAround.Count; j++)
                {
                    if (!c_ballAround[i].c_ballAround[j].f_CheckBall())
                    {
                        c_ballAround[i].c_ballAround[j].f_ActivCollider(_activ);
                    }
                }
            }
        }
    }

    public void f_React(bool _this)
    {
        if (_this)
        {
            BallController._get.f_SetNewBall(this, m_id);
            for (int i = 0; i < c_ballAround.Count; i++)
            {
                c_ballAround[i].f_React(false);
            }
        }

        StartCoroutine("IEReact");
    }

    private IEnumerator IEReact()
    {
        AnimationCurve _muveX = A_Curve.f_GetCurve(0, Random.Range(-0.1f, 0.1f), 0);
        AnimationCurve _muveZ = A_Curve.f_GetCurve(0, Random.Range(-0.1f, 0.1f), 0);

        float _step = 0;
        while (_step < 1)
        {
            _step += 3 * Time.deltaTime;
            mg_ball.transform.localPosition = new Vector3(_muveX.Evaluate(_step), 0, _muveZ.Evaluate(_step));
            yield return null;
        }
    }

    public void f_ActivCollider(bool _activ)
    {
        mc_collider.enabled = mg_ball.activeSelf ? true : _activ;
    }

    public bool f_CheckBall()
    {
        return mg_ball.activeSelf;
    }

    public List<Ball> f_GetAround(int _id, List<Ball> _ballOld)
    {
        if (_id != m_id && !f_CheckBall())
        {
            return _ballOld;
        }
        if (_ballOld.Count > 0)
        {
            for (int i = 0; i < _ballOld.Count; i++)
            {
                if (_ballOld[i] == this)
                {
                    return _ballOld;
                }
            }
        }
        _ballOld.Add(this);
        List<Ball> _b = new List<Ball>(_ballOld);

        for (int i = 0; i < c_ballAround.Count; i++)
        {
            bool _add = true;
            if (c_ballAround[i].f_CheckBall() && c_ballAround[i].m_id == _id)
            {
                for (int j = 0; j < _b.Count; j++)
                {
                    if (c_ballAround[i] == _b[j])
                    {
                        _add = false;
                        break;
                    }
                }
                if (_add)
                {
                    _ballOld.AddRange(c_ballAround[i].f_GetAround(_id, _ballOld));
                }
            }
        }

        return _ballOld;
    }

    public Ball f_CheckBallEmpty()
    {
        if (f_CheckBall())
        {
            for (int i = 0; i < c_ballAround.Count; i++)
            {
                if (c_ballAround[i].f_CheckBall())
                {
                    return null;
                }
            }
            return this;
        }
        return null;
    }

    public bool f_CheckDistance(Transform _point)
    {
        return f_CheckBall() ? Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_point.position.x, _point.position.z)) > 2 : true;
    }
}
