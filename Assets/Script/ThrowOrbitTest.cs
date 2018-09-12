using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowOrbitTest : MonoBehaviour {
    public GameObject orbitObj;
    GameObject[] orbits;
    public int orbitsNum = 20;
    public float interval = 0.2f;
    public float reachTime = 1;
    public float throwPower = 5;
    public GameObject sample;

    public GameObject target;
    public GameObject point;

    public float speed = 4;
    // Physics Simulate
    Vector2 vec = Vector2.zero;
    [SerializeField]Vector2 v0 = new Vector2(100, 100);
	// Use this for initialization
	void Start () {
        Physics2D.autoSimulation = false;

        orbits = new GameObject[orbitsNum];
        for (int i = 0; i < orbitsNum; i++)
        {
            orbits[i] = Instantiate(orbitObj);
        }



    }
	
	// Update is called once per frame
	void Update () {




		//if(Input.GetButtonDown(KeyConfig.Fire1))
        {
            vec = TargetVector2(point.transform.position, target.transform.position, v0.magnitude);
            Debug.Log(vec);
            ShowOrbit();
        }
        if (Input.GetButtonDown(KeyConfig.Jump))
        {
            Text text;
            text = GameObject.Find("Text").GetComponent<Text>();
            if (text.text == "Max")
            {
                text.text = "Min";
            }
            else
            {
                text.text = "Max";
            }
        }

        if (Input.GetButtonDown(KeyConfig.Fire1))
        {
            Shoot(m_target.position);
            transform.position = point.transform.position;
            GetComponent<Rigidbody2D>().velocity = vec;
            Physics2D.autoSimulation = true;
        }
    }

    void FixedUpdate()
    {

    }


    Vector2 TargetVector2(Vector2 point, Vector2 target, float V)
    {

        //  テスト
        bool reversal = false;
        float S = target.x - point.x;
        float H = (target.y - point.y);
        float G = 9.8f;

        float b = -1 * (2 * V * V * S) / (G * S * S);
        float c = 1 + (2 * V * V * H) / (G * S * S);

        float D = b * b - 4 * c;

        if (S == 0 && H == 0)
        {
            return Vector2.zero;
        }
        if (S == 0 && H > 0)
        {
            return Vector2.up;
        }
        if (S == 0 && H < 0)
        {
            return Vector2.down;
        }
        if (S < 0)
        {
            reversal = true;
            S *= -1;
        }
        if(D < 0)
        {
            Debug.Log("届かない");
            return Vector2.zero;
        }

        float angle0 = Mathf.Atan(-b - Mathf.Sqrt(D)) / 2;
        float angle1 = Mathf.Atan(-b + Mathf.Sqrt(D)) / 2;

        if (reversal)
        {
            angle0 = Mathf.PI - angle0;
            angle1 = Mathf.PI - angle1;
        }

        float theta = 0;
        string text = "";

            text = GameObject.Find("Text").GetComponent<Text>().text;
        if (text == "Max")
        {
            theta = Mathf.Max(angle0, angle1);
        } else
        {
            theta = Mathf.Min(angle0, angle1);
        }
            return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * V;

        // テスト終わり
    }

    void ShowOrbit()
    {


        float speedVec = ComputeVectorFromTime(target.transform.position, 1);
        float angle = ComputeAngleFromTime(target.transform.position, 1);

        if (speedVec <= 0.0f)
        {
            // その位置に着地させることは不可能のようだ！
            Debug.LogWarning("!!");
            return;
        }

        Vector3 vec = ConvertVectorToVector3(speedVec, angle, target.transform.position);
        Vector2 calculatedPosition = Vector2.zero;

        float time = 0;
        for (int i = 0; i < orbitsNum; i++)
        {
            time = i * 0.1f;

            calculatedPosition.x = vec.x * time + Mathf.Pow(time, 2) / 2;
            calculatedPosition.y = vec.y * time - 9.8f * Mathf.Pow(time, 2) / 2;
            orbits[i].transform.position = calculatedPosition + (Vector2)point.transform.position;
        }
    }



    [SerializeField]
    private Transform m_shootPoint = null;
    [SerializeField]
    private Transform m_target = null;
    [SerializeField]
    private GameObject m_shootObject = null;


    private void Shoot(Vector3 i_targetPosition)
    {
        // 角度固定
        //ShootFixedAngle(i_targetPosition, 60.0f);

        // 速さ固定
        //ShootFixedSpeedInPlaneDirection(i_targetPosition, 3.0f);

        // 時間固定
        ShootFixedTime(i_targetPosition, 1.0f);
    }

    private void ShootFixedAngle(Vector3 i_targetPosition, float i_angle)
    {
        float speedVec = ComputeVectorFromAngle(i_targetPosition, i_angle);
        if (speedVec <= 0.0f)
        {
            Debug.LogWarning("届かない");
            return;
        }

        Vector3 vec = ConvertVectorToVector3(speedVec, i_angle, i_targetPosition);
        InstantiateShootObject(vec);
    }

    private float ComputeVectorFromAngle(Vector3 i_targetPosition, float i_angle)
    {
        // xz平面の距離を計算。
        Vector2 startPos = new Vector2(m_shootPoint.transform.position.x, m_shootPoint.transform.position.z);
        Vector2 targetPos = new Vector2(i_targetPosition.x, i_targetPosition.z);
        float distance = Vector2.Distance(targetPos, startPos);

        float x = distance;
        float g = Physics.gravity.y;
        float y0 = m_shootPoint.transform.position.y;
        float y = i_targetPosition.y;

        float rad = i_angle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(rad);
        float tan = Mathf.Tan(rad);

        float v0Square = g * x * x / (2 * cos * cos * (y - y0 - x * tan));

        // 届かない
        if (v0Square <= 0.0f)
        {
            return 0.0f;
        }

        float v0 = Mathf.Sqrt(v0Square);
        return v0;
    }

    private Vector3 ConvertVectorToVector3(float i_v0, float i_angle, Vector3 i_targetPosition)
    {
        Vector3 startPos = m_shootPoint.transform.position;
        Vector3 targetPos = i_targetPosition;
        startPos.y = 0.0f;
        targetPos.y = 0.0f;

        Vector3 dir = (targetPos - startPos).normalized;
        Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 vec = i_v0 * Vector3.right;

        vec = yawRot * Quaternion.AngleAxis(i_angle, Vector3.forward) * vec;

        return vec;
    }

    private void InstantiateShootObject(Vector3 i_shootVector)
    {
        if (m_shootObject == null)
        {
            throw new System.NullReferenceException("m_shootObject");
        }

        if (m_shootPoint == null)
        {
            throw new System.NullReferenceException("m_shootPoint");
        }

        var obj = Instantiate<GameObject>(m_shootObject, m_shootPoint.position, Quaternion.identity);
        var rigidbody = obj.GetComponent<Rigidbody2D>();

        // 速さベクトルのままAddForce()を渡してはいけないぞ。力(速さ×重さ)に変換するんだ
        Vector3 force = i_shootVector * rigidbody.mass;

        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }


    private void ShootFixedSpeedInPlaneDirection(Vector3 i_targetPosition, float i_speed)
    {
        if (i_speed <= 0.0f)
        {
            // その位置に着地させることは不可能のようだ！
            Debug.LogWarning("!!");
            return;
        }

        // xz平面の距離を計算。
        Vector2 startPos = new Vector2(m_shootPoint.transform.position.x, m_shootPoint.transform.position.z);
        Vector2 targetPos = new Vector2(i_targetPosition.x, i_targetPosition.z);
        float distance = Vector2.Distance(targetPos, startPos);

        float time = distance / i_speed;

        ShootFixedTime(i_targetPosition, time);
    }

    private void ShootFixedTime(Vector3 i_targetPosition, float i_time)
    {
        float speedVec = ComputeVectorFromTime(i_targetPosition, i_time);
        float angle = ComputeAngleFromTime(i_targetPosition, i_time);

        if (speedVec <= 0.0f)
        {
            // その位置に着地させることは不可能のようだ！
            Debug.LogWarning("!!");
            return;
        }

        Vector3 vec = ConvertVectorToVector3(speedVec, angle, i_targetPosition);
        InstantiateShootObject(vec);
    }

    private float ComputeVectorFromTime(Vector3 i_targetPosition, float i_time)
    {
        Vector2 vec = ComputeVectorXYFromTime(i_targetPosition, i_time);

        float v_x = vec.x;
        float v_y = vec.y;

        float v0Square = v_x * v_x + v_y * v_y;
        // 負数を平方根計算すると虚数になってしまう。
        // 虚数はfloatでは表現できない。
        // こういう場合はこれ以上の計算は打ち切ろう。
        if (v0Square <= 0.0f)
        {
            return 0.0f;
        }

        float v0 = Mathf.Sqrt(v0Square);

        return v0;
    }

    private float ComputeAngleFromTime(Vector3 i_targetPosition, float i_time)
    {
        Vector2 vec = ComputeVectorXYFromTime(i_targetPosition, i_time);

        float v_x = vec.x;
        float v_y = vec.y;

        float rad = Mathf.Atan2(v_y, v_x);
        float angle = rad * Mathf.Rad2Deg;

        return angle;
    }

    private Vector2 ComputeVectorXYFromTime(Vector3 i_targetPosition, float i_time)
    {
        // 瞬間移動はちょっと……。
        if (i_time <= 0.0f)
        {
            return Vector2.zero;
        }


        // xz平面の距離を計算。
        Vector2 startPos = new Vector2(m_shootPoint.transform.position.x, m_shootPoint.transform.position.z);
        Vector2 targetPos = new Vector2(i_targetPosition.x, i_targetPosition.z);
        float distance = Vector2.Distance(targetPos, startPos);

        float x = distance;
        // な、なぜ重力を反転せねばならないのだ...
        float g = -Physics.gravity.y;
        float y0 = m_shootPoint.transform.position.y;
        float y = i_targetPosition.y;
        float t = i_time;

        float v_x = x / t;
        float v_y = (y - y0) / t + (g * t) / 2;

        return new Vector2(v_x, v_y);
    }
}
