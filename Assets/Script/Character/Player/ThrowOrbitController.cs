using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowOrbitController : MonoBehaviour {
    //軌道を描く
    GameObject[] targetOrbits;
    public int orbitsNum = 20;
    public GameObject orbitObj;
    GameObject throwOrbits;
    float orbitAnimTime = 0.01f;

    float orbitTimer = 0;
    public float orbitAnimInterval = 0.1f;
    float orbitAnimSpeed = 0.03f;
    public GameObject oldCapturedObj = null;

    float throwObjGravityScale = 1;
    [SerializeField] SnowBallNormal snowBall;
    GameObject target, point;
    //軌道を描くためのサンプル


    bool hitsEarlier = false;
    //ターゲットチェック
    public float checkTimeInterval = 0.01f;
    public float checkNum = 100;
    float detectedHitTime = 0;
    [SerializeField] GameObject captureObj;

    private void Update()
    {
        // #Progress
        throwObjGravityScale = snowBall.GetComponent<Rigidbody2D>().gravityScale;
    }
    public Vector2 CaptureObjPosition
    {
        get { return captureObj.transform.position; }
        set {
                captureObj.SetActive(true);
                captureObj.transform.position = value;
        }
    }

    public void HideCaptureObj()
    {
        captureObj.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        // 軌道点、初期化
        throwOrbits = GameObject.Find("ThrowOrbits");
        point = GameObject.Find("Player/PlayerSprite/ThrowPoint");
        captureObj = GameObject.Find("Capture");
        target = point;
        targetOrbits = new GameObject[orbitsNum];
        for (int i = 0; i < orbitsNum; i++)
        {
            targetOrbits[i] = Instantiate(orbitObj);
            targetOrbits[i].transform.parent = throwOrbits.transform;
        }
        
    }

    public void SetOrbitsAppearance(Color color, float size)
    {
        foreach (GameObject targetOrbit in targetOrbits)
        {
            targetOrbit.GetComponent<SpriteRenderer>().color = color;
            targetOrbit.transform.localScale = Vector3.one * size;
        }
    }

    public void SetOrbitsHit()
    {
        SetOrbitsAppearance(Color.red, 1.3f);
    }

    public void SetOrbitsDefault()
    {
        SetOrbitsAppearance(Color.blue, 1f);
    }
    public void SetOrbitsActive(bool active)
    {
        throwOrbits.SetActive(active);
    }

    public void ResetAnime()
    {
        orbitAnimTime = 0;
    }


    public GameObject GetNearTargetByOrbit(float throwPower, float dir)
    {
        float x = 0;
        float y = 0;
        float vx = Input.GetAxis("Horizontal");
        float vy = Input.GetAxis("Vertical");
        Transform point = transform.Find("PlayerSprite/ThrowPoint");
        float preY = 0;

        GameObject tempTarget = null;
        for (int i = 0; i < checkNum; i++)
        {
            float time = (i * checkTimeInterval) + orbitAnimTime;
            if (orbitTimer >= orbitAnimInterval)
            {
                //   orbitAnimTime += orbitAnimSpeed;
                //    orbitTimer -= orbitAnimInterval;
            }
            if (orbitAnimTime >= 0.1f)
            {
                orbitAnimTime = orbitAnimSpeed;
            }
            if (vx == 0 && vy == 0)
            {
                vx = 1 * dir;
                vy = 0.2f;
            }
            x = throwPower * Mathf.Cos(Mathf.Atan2(vy, vx)) * time;
            y = throwPower * Mathf.Sin(Mathf.Atan2(vy, vx)) * time - 0.5f * 9.8f * throwObjGravityScale * time * time;


            Vector2 orbitPos = new Vector2(x, y) + (Vector2)point.position;
            GameObject nearTarget = GetNearTargetByPosition(orbitPos, 2f);

            if (nearTarget != null)
            {
                Vector2 vec = new Vector2(vx, vy) * throwPower;
                float distance = (nearTarget.transform.position - point.position).magnitude;
                Debug.Log("vec : " + vec);
                float approvalTime = distance / throwPower;
                float topTime = vec.y / (9.8f * throwObjGravityScale);

                detectedHitTime = time;
                Debug.Log("topTime : " + topTime + ", time : " + time);

                
                return nearTarget;
            }
        }
        return null;
    }
    public Vector2 CalcOrbit(float throwPower, float dir)
    {
        float x = 0;
        float y = 0;
        float vx = Input.GetAxis("Horizontal");
        float vy = Input.GetAxis("Vertical");
        Transform point = transform.Find("PlayerSprite/ThrowPoint");
        float preY = 0;

        GameObject tempTarget = null;
        for (int i = 0; i < checkNum; i++)
        {
            float time = (i * checkTimeInterval) + orbitAnimTime;
            if (orbitTimer >= orbitAnimInterval)
            {
             //   orbitAnimTime += orbitAnimSpeed;
            //    orbitTimer -= orbitAnimInterval;
            }
            if (orbitAnimTime >= 0.1f)
            {
                orbitAnimTime = orbitAnimSpeed;
            }
            if (vx == 0 && vy == 0)
            {
                vx = 1 * dir;
                vy = 0.2f;
            }
            x = throwPower * Mathf.Cos(Mathf.Atan2(vy, vx)) * time;
            y = throwPower * Mathf.Sin(Mathf.Atan2(vy, vx)) * time - 0.5f * 9.8f * throwObjGravityScale * time * time;


            Vector2 orbitPos = new Vector2(x, y) + (Vector2)point.position;
            GameObject nearTarget = GetNearTargetByPosition(orbitPos, 1f);
            Vector2 targetPos = Vector2.zero;
            if (nearTarget == null) { targetPos =new Vector2 (5000, 5000); } else
            {
                targetPos = nearTarget.transform.position;
            }

            //  ターゲットロックオン
            if(tempTarget != null && nearTarget != tempTarget)
            {
                return new Vector2(vx, vy);
            }
            if (nearTarget != null)
            {
                tempTarget = nearTarget;
                target = nearTarget;
                SetOrbitsAppearance(Color.red, 1f);
                //targetOrbits[i].GetComponent<SpriteRenderer>().color = Color.black;
                Vector2 dv = nearTarget.transform.position - point.position;
                Vector2 vec = Vector2.zero;

                vec.x = dv.x / time;
                vec.y = (dv.y + 0.5f * 9.8f * throwObjGravityScale * time * time) / time;
                float speed = 0;
                if (time == 0)
                {
                    vec.x = dv.x;
                    vec.y = dv.y;
                    speed = 12;
                }
                speed = dv.magnitude / time;
                //float A = 9.8f * nearTarget.x * nearTarget.x / 2 / vec.magnitude * vec.magnitude;
                //float X = nearTarget.x * nearTarget.x / A / A / 4 + nearTarget.y / A - 1 + nearTarget.x / A / 2;
                //if(X < 0)
                //{
                //    foreach (GameObject targetOrbit in targetOrbits)
                //    {
                //        targetOrbit.GetComponent<SpriteRenderer>().color = Color.blue;
                //    }
                //}
                vx = vec.x;
                vy = vec.y;

                GameObject.Find("Capture").transform.position = target.transform.position;
                Debug.Log(speed);
               // return TargetVector2(point.transform.position, tempTarget.transform.position, speed);
                return vec;
            }

            //  最高点以降のオブジェクトは無視
            if (y < preY)
            {
            //    break;
            }
            preY = y;

            //if (time > new Vector2(vx, vy).magnitude * Mathf.Sin(Mathf.Atan2(vy, vx)) / 9.8f)
            //{
            //    break;
            //}
        }
        return new Vector2(vx, vy);
    }

    Vector2 TargetVector2(Vector2 point, Vector2 target, float V)
    {

        //  テスト
        bool reversal = false;
        float S = target.x - point.x;
        float H = target.y - point.y;
        float G = 9.8f * throwObjGravityScale;

        float b = -1 * (2 * V * V * S) / (G * S * S);
        float c = 1 + (2 * V * V * H) / (G * S * S);

        float D = b * b - 4 * c;



        if (S == 0 && H == 0)
        {
            return new Vector2(20f, 5f);
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


 
        float angle0 = Mathf.Atan(( -b - Mathf.Sqrt(D)) / 2);
        float angle1 = Mathf.Atan((-b + Mathf.Sqrt(D)) / 2);

        if (reversal)
        {
            //angle0 = Mathf.PI - angle0;
            //angle1 = Mathf.PI - angle1;
        }



        //  届かない判定
        if (D < 0)
        {
            return (target - point).normalized;
        }
        float theta = 0;
        if (hitsEarlier) {
            theta = Mathf.Min(angle0, angle1);
            if (reversal)
            {
                theta = Mathf.Max(angle0, angle1);
            }
        } else
        {
            theta = Mathf.Max(angle0, angle1);
            if (reversal)
            {
                theta = Mathf.Min(angle0, angle1);
            }
        }
        if (reversal)
        {
            return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * -1f;
        } else
        {
            return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
        }
        
        // テスト終わり
    }

    public bool canReachToTarget(Vector2 point, Vector2 target, float V)
    {

        float S = target.x - point.x;
        float H = target.y - point.y;
        float G = 9.8f * throwObjGravityScale;

        float b = -1 * (2 * V * V * S) / (G * S * S);
        float c = 1 + (2 * V * V * H) / (G * S * S);

        float D = b * b - 4 * c;
        //  届かない判定
        if (D < 0)
        {
            return false;
        }
        return true;
    }


    private float ComputeVectorFromAngle(Vector3 i_targetPosition, float i_angle)
    {
        // xz平面の距離を計算
        Vector2 startPos = new Vector2(point.transform.position.x, point.transform.position.z);
        Vector2 targetPos = new Vector2(i_targetPosition.x, i_targetPosition.z);
        float distance = Vector2.Distance(targetPos, startPos);

        float x = distance;
        float g = Physics.gravity.y;
        float y0 = point.transform.position.y;
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
        Vector3 startPos = point.transform.position;
        Vector3 targetPos = i_targetPosition;
        startPos.y = 0.0f;
        targetPos.y = 0.0f;

        Vector3 dir = (targetPos - startPos).normalized;
        Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 vec = i_v0 * Vector3.right;

        vec = yawRot * Quaternion.AngleAxis(i_angle, Vector3.forward) * vec;

        return vec;
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
        if (i_time <= 0.0f)
        {
            return Vector2.zero;
        }


        // xz平面の距離を計算
        Vector2 startPos = new Vector2(point.transform.position.x, point.transform.position.z);
        Vector2 targetPos = new Vector2(i_targetPosition.x, i_targetPosition.z);
        float distance = Vector2.Distance(targetPos, startPos);

        float x = distance;
        float g = -Physics.gravity.y;
        float y0 = point.transform.position.y;
        float y = i_targetPosition.y;
        float t = i_time;

        float v_x = x / t;
        float v_y = (y - y0) / t + (g * t) / 2;

        return new Vector2(v_x, v_y);
    }

    //  
    public Vector2 ShowOrbit(Vector2 startPos, Vector2 targetPos, float throwPower, float dir)
    {
        // #Progress

        //float distance = Vector2.Distance(targetPos, startPos);

        float distance = Vector2.Distance(targetPos, startPos);

        float approvalTime = distance / throwPower;
        Debug.Log("approvalTime : " + approvalTime);
        float speedVec = ComputeVectorFromTime(targetPos, approvalTime);
        float angle = ComputeAngleFromTime(targetPos, approvalTime);

        if (speedVec <= 0.0f)
        {
            return Vector2.zero;
        }

        Vector2 vec = (targetPos - startPos).normalized * speedVec; //ConvertVectorToVector3(speedVec, angle, targetPos);
        Vector2 calculatedPosition = Vector2.zero;


        if (vec.magnitude > throwPower)
        {
            vec = vec / vec.magnitude * throwPower;
            //vec.Normalize();
            //vec *= throwPower;
        }

        if (Mathf.Abs(detectedHitTime - approvalTime) < 0.5f)
        {
            hitsEarlier = true;
        }
        else
        {
            hitsEarlier = false;
        }


        vec = TargetVector2(startPos, targetPos, throwPower) * throwPower;

        //Debug.Log(Vector2.Angle(Vector2.left, vec));
        float time = 0;

        if (targetPos == Vector2.zero || (Input.GetButton("LB")))
        {
            vec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * throwPower;
        }

        if (vec == Vector2.zero)
        {
            vec = new Vector2(dir, 0.3f).normalized * throwPower;
        }

        for (int i = 0; i < orbitsNum; i++)
        {
            time = i * 0.1f;
            calculatedPosition.x = vec.x * time;
            calculatedPosition.y = vec.y * time - 9.8f * throwObjGravityScale * time * time / 2;
            targetOrbits[i].transform.position = calculatedPosition + startPos;
        }
        return vec;
    }

    public Vector2 ShowOrbitCertainly(Vector2 startPos, Vector2 targetPos, float throwPower, float dir)
    {

        //float distance = Vector2.Distance(targetPos, startPos);

        float distance = Vector2.Distance(targetPos, startPos);

        float approvalTime = distance / throwPower;
        Debug.Log("approvalTime : " + approvalTime);
        float speedVec = ComputeVectorFromTime(targetPos, approvalTime);
        float angle = ComputeAngleFromTime(targetPos, approvalTime);

        if (speedVec <= 0.0f)
        {
            return Vector2.zero;
        }

        Vector2 vec = (targetPos - startPos).normalized * speedVec; //ConvertVectorToVector3(speedVec, angle, targetPos);
        Vector2 calculatedPosition = Vector2.zero;


        if (vec.magnitude > throwPower)
        {
            vec = vec / vec.magnitude * throwPower;
            //vec.Normalize();
            //vec *= throwPower;
        }

        if (Mathf.Abs(detectedHitTime - approvalTime) < 0.5f)
        {
            hitsEarlier = true;
        }
        else
        {
            hitsEarlier = false;
        }


        vec = TargetVector2(startPos, targetPos, throwPower) * throwPower;

        //Debug.Log(Vector2.Angle(Vector2.left, vec));
        float time = 0;

        if (targetPos == Vector2.zero || (Input.GetButton("LB")))
        {
            vec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * throwPower;
        }

        if (vec == Vector2.zero)
        {
            vec = new Vector2(dir, 0.3f).normalized * throwPower;
        }

        for (int i = 0; i < orbitsNum; i++)
        {
            time = i * 0.1f;
            calculatedPosition.x = vec.x * time;
            calculatedPosition.y = vec.y * time - 9.8f * throwObjGravityScale * time * time / 2;
            targetOrbits[i].transform.position = calculatedPosition + startPos;
        }
        return vec;
    }
    public Vector2 ShowOrbitByVector(Vector2 startPos, Vector2 throwDirection, float throwPower, float dir)
    {
        Vector2 calculatedPosition = Vector2.zero;
        //#Progress
        Vector2 vec = throwDirection * throwPower;
        float time = 0;
        for (int i = 0; i < orbitsNum; i++)
        {
            time = i * 0.1f;
            calculatedPosition.x = vec.x * time;
            //#Progress
            calculatedPosition.y = vec.y * time - 9.8f * throwObjGravityScale * time * time / 2;
            targetOrbits[i].transform.position = calculatedPosition + startPos;
        }

        return vec;
    }

    public GameObject GetNearTargetByPosition(Vector2 pos, float distance)
    {
        RaycastHit2D[] nearTargets = new RaycastHit2D[5];
        Physics2D.CircleCastNonAlloc(pos, distance, Vector2.zero, nearTargets, 0);

        GameObject target = null;
        Vector2 targetPos = pos;
        float targetDistance = 999;
        foreach (RaycastHit2D hit in nearTargets)
        {
            if (!hit)
            {
                continue;
            }
            if (hit.transform.tag != "EnemyBody")
            {
                continue;
            }
            float tmpDistance = Vector2.SqrMagnitude((Vector2)hit.transform.position - pos);
            if (tmpDistance < targetDistance)
            {
                targetDistance = tmpDistance;
                targetPos = hit.transform.position;
                target = hit.transform.gameObject;
                oldCapturedObj = hit.transform.gameObject;
            }

        }

        return target;
    }
}
