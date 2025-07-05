using System.Collections;
using UnityEngine;

public class MuveBall : ActivObj
{
    [SerializeField] private Collider mc_collider;
    [SerializeField] private MeshRenderer mm_mesh;
    private LineRenderer ml_lineRenderer;
    private float m_speed;
    private int m_currentPointIndex = 0;
    private bool m_muve;
    public int m_id;
    private Ball c_ballStop;

    public MuveBall f_Init(float _yPos, S_BallInfo _info)
    {
        mm_mesh.material = _info.m_mat;
        m_id = _info.m_id;
        mc_collider.enabled = false;
        m_muve = false;
        transform.localPosition = new Vector3(0, _yPos, 0);
        transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(true);
        StartCoroutine("IEActiv", true);
        return this;
    }

    public override IEnumerator IEActiv(bool _activ)
    {
        AnimationCurve _scale = A_Curve.f_GetCurve(0, 1);
        float _step = 0;
        while (_step < 1)
        {
            _step += 3 * Time.deltaTime;
            transform.localScale = new Vector3(_scale.Evaluate(_step), _scale.Evaluate(_step), _scale.Evaluate(_step));
            yield return null;
        }
    }

    public void f_Muve(LineRenderer _line, float _speed, Ball _ballStop)
    {
        c_ballStop = _ballStop;
        c_ballStop.f_ActivBallsAround(true);
        transform.parent = BallController._get.transform;
        ml_lineRenderer = _line;
        mc_collider.enabled = true;
        m_speed = _speed;
        m_muve = true;
    }

    void Update()
    {
        if (!m_muve) return;

        if (m_currentPointIndex < ml_lineRenderer.positionCount)
        {
            Vector3 targetPosition = ml_lineRenderer.GetPosition(m_currentPointIndex);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, m_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                m_currentPointIndex++;
                if (m_currentPointIndex >= ml_lineRenderer.positionCount)
                {
                    c_ballStop.f_ActivBallsAround(false);
                    DestroyImmediate(gameObject);
                    m_currentPointIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>())
        {
            if (other.GetComponent<Ball>().f_CheckBall())
            {
                other.GetComponent<Ball>().mc_collider.enabled = false;
            }
            other.GetComponent<Ball>().f_SetNewBall(new S_BallInfo(mm_mesh.material, m_id));
            c_ballStop.f_ActivBallsAround(false);
            other.GetComponent<Ball>().f_React(true);
            mc_collider.enabled = false;
        }
    }
}
