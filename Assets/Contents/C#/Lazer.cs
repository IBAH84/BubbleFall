using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private LineRenderer ml_line;
    private int m_maxBounces = 5;
    private float m_maxDistance = 100f;
    private Vector3 m_clik;
    private Ball c_ballStop;


    private void Update()
    {
        if (!BallConteiner._get.f_CheckClik() || PointTouch._get.m_clik)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ml_line.enabled = true;
            m_clik = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            m_clik.x -= transform.localEulerAngles.x;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 _clik = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            transform.localEulerAngles = new Vector3(m_clik.x + _clik.x * 1000, 90, 0);
            f_Laser();
        }
        if (Input.GetMouseButtonUp(0))
        {
            BallConteiner._get.f_SetBall(ml_line, c_ballStop);
            transform.localEulerAngles = new Vector3(90, 90, 0);
            ml_line.enabled = false;
        }
    }

    private void f_Laser()
    {
        ml_line.positionCount = 1;
        ml_line.SetPosition(0, transform.position);

        Vector3 _pos = transform.position;
        Vector3 _dir = transform.forward;
        float _distance = m_maxDistance;

        for (int i = 0; i < m_maxBounces; i++)
        {
            RaycastHit _hit;
            if (Physics.Raycast(_pos, _dir, out _hit, _distance))
            {
                ml_line.positionCount++;
                ml_line.SetPosition(ml_line.positionCount - 1, _hit.point);

                _distance -= Vector3.Distance(_pos, _hit.point);
                _pos = _hit.point;
                if (_hit.collider.GetComponent<Ball>())
                {
                    c_ballStop = _hit.collider.GetComponent<Ball>();
                    break;
                }
                _dir = Vector3.Reflect(_dir, _hit.normal);
            }
            else
            {
                ml_line.positionCount++;
                ml_line.SetPosition(ml_line.positionCount - 1, _pos + _dir * _distance);
                break;
            }
        }
    }
}
