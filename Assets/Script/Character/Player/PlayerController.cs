using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum PLYSTS
{
    NORMAL,
    THROW,
}
public enum THROWTYPE
{
    Parabola,
    Parabola2Way,
    FreeVector
}
public class PlayerController : BaseCharacterController
{

    // TODO 後で処理をManagerに移す
    public bool isStarted = true;


    public float slidePower = 1;
    public GameObject snowParticle;
    public PLYSTS state;
    public THROWTYPE throwType = THROWTYPE.Parabola2Way;
    public bool canbeSnowBall = false;
    public bool canThrowSnowBall = true;
    public float resurrectionTime = 2f;
    public bool canWalkWhileThrowing = false;
    public bool pushedRightB = false;

    //外部パラメータ
    public float chargeTime = 0.5f;
    public int NumberOfCheckPoint = 10;
    public float maxThrowPower = 12f;
    public float jumpPower = 10f;
    public float initHpMax = 4.0f;
    [Range(0.1f, 100.0f)] public float initSpeed = 12.0f;
    public float jumpTimeMax = 30f;
    public GameObject defaultThrowObj;   //投げた雪玉
    [System.NonSerialized] public float groundY = 0.0f;
    [System.NonSerialized] public static int coin = 5000;
    public MessageWindowController msgWindow;
    public static int CheckPointNumber = 0;
    public static bool enterDoor = false;

    public GameObject damageEffect;
    public Transform throwPoint;

    ThrowOrbitController orbits;
    //鼻照準
    Vector3 oldThrowEuler = Vector3.zero;
    bool readyToThrow = false;
    GameObject spriteObj;
    Vector2 throwDirection = Vector2.zero;

    //内部パラメータ
    bool isCharging = false;
    public float throwPower = 0;
    bool breakEnabled = true;
    float groundFriction = 0.0f;
    float jumpTime = 0;
    bool canJumpUp = true;
    float invincibleStartTime;
    float invincibleTime;
    bool throwReservation = false;
    bool preThrowStart = false;
    public GameObject throwObj;
    [SerializeField] float throwObjSpeed = 1;
    //効果音
    SoundManager soundManager;
    [System.NonSerialized] public Transform targetTalkTo;

    [System.NonSerialized] public bool isWarping = false;

    Vector2 ThrowPoint
    {
        get { return throwPoint.position; }
    }

    Vector2 StickDirection
    {
        get { return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized; }
    }

    Vector2 DefaultThrowDirection
    {
        get { return new Vector2(dir, 0.3f).normalized; }
    }


    //コード（サポート関数）
    public static GameObject GetGameObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }
    public static Transform GetTransform()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }
    public static PlayerController GetController()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public static Animator GetAnimator()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }


    protected override void Awake()
    {

        base.Awake();
        rbody2D = GetComponent<Rigidbody2D>();



        //鼻照準　初期化
        spriteObj = transform.Find("PlayerSprite").gameObject;
        oldThrowEuler = spriteObj.transform.eulerAngles;
        orbits = GetComponent<ThrowOrbitController>();

        //パラメータ初期化
        speed = initSpeed;
        setHP(initHpMax, initHpMax);
        state = PLYSTS.NORMAL;
        anime.SetTrigger("EndInvincible");

        targetTalkTo = transform.Find("TargetTalkTo");
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        throwPoint = transform.Find("PlayerSprite/ThrowPoint");
    }


    public void ReadyToThrow()
    {
        readyToThrow = true;
    }


    
    private int GetCurrentAnimation()
    {
        AnimatorStateInfo asi = anime.GetCurrentAnimatorStateInfo(0);
        if (asi.fullPathHash == Animator.StringToHash("Base Layer.Player_Idle"))
        {
        }
        return asi.fullPathHash;
    }

    private bool IsCurrentAnimation(string path)
    {
        return Animator.StringToHash(path) == GetCurrentAnimation();
    }

    protected override void FixedUpdateCharacter()
    {
        // #Progress
        //defaultThrowObj.GetComponent<Rigidbody2D>().gravityScale = throwObjSpeed;
        throwPower = maxThrowPower * throwObjSpeed;// *throwObjSpeed;

        ChargeManage();
        if(preThrowStart)
        {
            ShowOrbit();
            ThrowRotate();
        }

        //無敵時間かくにん 
        if (IsInvincible)
        {
            if (Time.fixedTime - invincibleStartTime >= invincibleTime)
            {
                IsInvincible = false;
                anime.SetTrigger("EndInvincible");
            }
        }


        switch (state)
        {   //接地判定ラインの設定
            case PLYSTS.NORMAL:
            default:
                centerY = 0.8f;
                break;
        }
        //　着地チェック
        if (jumped)
        {
            if ((grounded && !groundedPrev) ||
                (grounded && Time.fixedTime > jumpStartTime + 1.0f))
            {


                switch (state)
                {
                    case PLYSTS.NORMAL:

                        break;
                }
                if(!IsCurrentAnimation("Base Layer.Player_Idle") &&
                    !IsCurrentAnimation("Base Layer.Player_PreThrow") &&
                    !IsCurrentAnimation("Base Layer.Player_Throw"))
                {
                    anime.SetTrigger("Idle");
                }
                jumped = false;

            }
        }
        if (grounded)
        {
            //前のジャンプからジャンプボタン押しっぱなしならジャンプ発動しない
            if (!Input.GetButton("Jump"))
            {
                jumpTime = 0;

                canJumpUp = true;
            }
        }
        //	Debug.Log ("　設置：" + grounded + "ジャンプ："+ jumped + "　摩擦：" + breakEnabled + "　アーマー：" + IsInvincible);
        //　ジャンプ中の横移動減速
        if (jumped && !grounded)
        {
            if (breakEnabled)
            {
                breakEnabled = false;
                speedVx *= 0.9f;
            }
        }
        //減速処理
        if (breakEnabled)
        {
            speedVx *= groundFriction;
        }
        // キャラの向き
        transform.localScale = new Vector3(
            basScaleX * dir, transform.localScale.y, transform.localScale.z);


        //	CameraTranslate ();

    }


    private void ChargeManage()
    {
        //チャージ
        if (isCharging)
        {
            // #Progress
            throwPower = maxThrowPower;
        //    throwPower += maxThrowPower * (1 / chargeTime) * Time.fixedDeltaTime * throwObjSpeed;
        //    throwPower = Mathf.Clamp(throwPower, 0, maxThrowPower * throwObjSpeed);
            if (throwPower == maxThrowPower)
            {
                soundManager.StopSE("ThrowCharge");
                soundManager.PlaySEIfNotPlaying("ThrowChargeMax");
                orbits.SetOrbitsAppearance(Color.blue, 1.3f);
            }
            orbits.SetOrbitsActive(true);

            if (readyToThrow)
            {

            }
        }
        else
        {
            orbits.SetOrbitsAppearance(Color.white, 1f);
            orbits.SetOrbitsActive(false);
            orbits.ResetAnime();
            // throwPower = 0;
        }
    }
    // 投げる方向に鼻が向く
    private void ThrowRotate()
    {
        float vx = Input.GetAxis("Horizontal");
        float vy = Input.GetAxis("Vertical");

        if (vx == 0 && vy == 0)
        {
            vx = 1 * dir;
            vy = 0.2f;
        }

        float radian = Mathf.Atan2(vy, vx);
        {
            if (dir < 0)
            {
                radian = radian - 180 / Mathf.PI;
            }
            radian -= 0.7f;
        }

        Vector3 rotateValue = new Vector3(0, 0, radian * 45);

        //  プレイヤーの角度を変更
        spriteObj.transform.eulerAngles = rotateValue;
        //滑らかに角度を変える
        //if (spriteObj.transform.eulerAngles.z < oldThrowEuler.z)
        //{
        //    spriteObj.transform.eulerAngles = Vector3.Lerp(oldThrowEuler, rotateValue, 0.1f);
        //} else
        //{
        //    spriteObj.transform.eulerAngles = Vector3.Lerp(rotateValue, oldThrowEuler,  0.1f);

        //}
        //Debug.Log("oldThrowEuler : " + spriteObj.transform.eulerAngles);
        //oldThrowEuler = spriteObj.transform.eulerAngles;
    }

    void ShowOrbit()
    {
        switch (throwType)
        {
            case THROWTYPE.Parabola:
                ShowOrbitSimple();
                break;
            case THROWTYPE.Parabola2Way:
                ShowOrbitInclude2Way();
                break;
            case THROWTYPE.FreeVector:
                ShowOrbitFreeVector();
                break;
        }

    }


    void ShowOrbitFreeVector()
    {
        orbits.CaptureObjPosition += StickDirection * Time.deltaTime * 10f;
        Vector2 distVec = (orbits.CaptureObjPosition - ThrowPoint);
        float time = 0.1f * distVec.magnitude;
        throwVec.x = distVec.x / time;
        throwVec.y = ((orbits.CaptureObjPosition - ThrowPoint).y + 9.8f * time * time * 0.5f) / time;
        //throwVec = orbits.ShowOrbit(ThrowPoint,orbits.CaptureObjPosition, throwPower, dir);
    }

    void ShowOrbitInclude2Way()
    {
        //Vector2 targetPos = orbits.CalcOrbit(throwPower, dir);
        GameObject target = orbits.GetNearTargetByOrbit(throwPower, dir);


        if (target != null)
        {
            orbits.CaptureObjPosition = target.transform.position;
            throwVec = orbits.ShowOrbit(ThrowPoint, target.transform.position, throwPower, dir);
            orbits.SetOrbitsHit();
        } else
        {
            orbits.SetOrbitsDefault();
            orbits.HideCaptureObj();
            if (StickDirection != Vector2.zero)
            {
                orbits.ShowOrbitByVector(ThrowPoint, StickDirection, throwPower, dir);
                throwVec = StickDirection * throwPower;
                throwVec = orbits.ShowOrbitByVector(ThrowPoint, StickDirection, throwPower, dir);
            }
            else
            {
                orbits.ShowOrbitByVector(ThrowPoint, DefaultThrowDirection, throwPower, dir);
                throwVec = DefaultThrowDirection * throwPower;
            }

        }
    }



    void ShowOrbitSimple()
    {

        Vector2 targetPos = SearchTarget();



        if (targetPos == Vector2.zero || (Input.GetButton("LB")))
        {
            throwVec = StickDirection * throwPower;
            if (throwVec == Vector2.zero)
            {
                throwVec = DefaultThrowDirection * throwPower * dir;
            }
            orbits.SetOrbitsAppearance(Color.blue, 1f);
            return;
        }

        if (orbits.canReachToTarget(ThrowPoint, targetPos, throwPower))
        {
            orbits.SetOrbitsAppearance(Color.red, 1.3f);
        }
        else
        {
            orbits.SetOrbitsAppearance(Color.blue, 1f);
        }

    }

    public override void Move(float n)
    {

        if (state == PLYSTS.THROW)
        {
            if (canWalkWhileThrowing && !pushedRightB)
            {
                n /= 2;
            }
        }
        if (IsCurrentAnimation("Base Layer.Player_Throw"))
        {
        //    n /= 5;
        }
            if (pushedRightB)
        {
            if (n != 0)
            {
                dir = Mathf.Sign(n);
            }
            speedVx *= 0.3f;
            anime.SetFloat("MovSpeed", 0);
            return;
        }

        breakEnabled = false;
        if (!activeSts || isWarping)
        {
            return;
        }
        float dirOld = dir;
        float moveSpeed = Mathf.Clamp(Mathf.Abs(n), -1.0f, +1.0f);


            anime.SetFloat("MovSpeed", moveSpeed);

        //移動チェック
        if (n != 0.0f)
        {

            dir = Mathf.Sign(n);


            moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;
            speedVx = initSpeed * moveSpeed * dir;

        }
        else
        {
            breakEnabled = true;
        }

        if (dirOld != dir)
        {
            breakEnabled = true;
        }
    }

    //ジャンプ
    public void Jump()
    {


        if (canJumpUp)
        {
            if (jumpTime == 0)
            {
                rbody2D.velocity += Vector2.up * jumpPower;
                jumpStartTime = Time.fixedTime;
                jumped = true;
                if (!IsCurrentAnimation("Base Layer.Player_PreThrow") &&
                    !IsCurrentAnimation("Base Layer.Player_Throw"))
                {
                    anime.SetTrigger("Jump");
                }
            }
            rbody2D.velocity += Vector2.up * (jumpTime * jumpTime / 10);
        }
        jumpTime++;
        if (jumpTime >= jumpTimeMax)
        {
            endJumpUp();

        }


    }
    public void JumpUp()
    {
        if (jumped)
        {
            if (canJumpUp)
            {
                if (jumpTime == 0)
                {
                    rbody2D.velocity += Vector2.up * jumpPower;
                    jumpStartTime = Time.fixedTime;
                    jumped = true;
                   // anime.SetTrigger("Jump");
                }
                rbody2D.velocity += Vector2.up * (jumpTime * jumpTime / 10);
            }
            jumpTime++;
            if (jumpTime >= jumpTimeMax)
            {
                endJumpUp();

            }


        }
    }
    public void endJumpUp()
    {
        canJumpUp = false;
    }

    //	ダメージ
    public void Damage(float damage)
    {
        if (!activeSts || IsInvincible)
        {
            return;
        }
        speedVx = 0;

        Instantiate(damageEffect, transform.position, transform.rotation);
        if (setHP(hp - damage, hpMax))
        {
            Dead(true);
        }
    }

    public override void Dead(bool gameOver)
    {
        if (!activeSts)
        {
            return;
        }
        base.Dead(gameOver);
        setHP(0, hpMax);
        Invoke("GameOver", 0.0f);

    }

    public void GameOver()
    {

        anime.SetTrigger("EndInvincible");
        anime.SetTrigger("Dead");
        Invoke("Resurrection", resurrectionTime);
    }

    public void PreThrow()
    {
        anime.SetTrigger("PreThrow");
        if (state == PLYSTS.THROW)
        {
            return;
        }

        state = PLYSTS.THROW;
        anime.ResetTrigger("Throw");

        ShowOrbit();
        preThrowStart = true;
    }

    public void SetThrowObj()
    {
        throwObj = defaultThrowObj;
    }

    public void SetThrowObj(GameObject gameObject)
    {
        throwObj = gameObject;
    }

    public void ThrowChargeSE()
    {
        soundManager.PlaySE("ThrowCharge");
    }
    public void Throw()
    {
        anime.ResetTrigger("PreThrow");
        if (IsCurrentAnimation("Base Layer.Player_PreThrow"))
        {
            anime.SetTrigger("Throw");
        }
    }
    public void StartCharge()
    {
        isCharging = true;
    }
    public void EndCharge()
    {
        isCharging = false;
    }
    public void ThrowEnd()
    {
        Transform point = transform.Find("PlayerSprite/ThrowPoint");

        Vector2 transformX = transform.position;

        if (Input.GetAxis("Horizontal") > 0)
        {
            dir = (0 <= Input.GetAxis("Horizontal")) ? 1 : -1;
        }
        soundManager.PlaySEOneShot("ThrowSnowBall");
        soundManager.StopSE("ThrowCharge");
        soundManager.StopSE("ThrowChargeMax");
        readyToThrow = false;
        preThrowStart = false;
        //Vector2 targetPos = SearchTarget();
        //   tempDirection = orbits.ShowOrbit(ThrowPoint, SearchTarget(), throwPower, dir);

        //if (targetPos == Vector2.zero)
        //{
        //    tempDirection = StickDirection * throwPower;
        //}

        //if (tempDirection == Vector2.zero)
        //{
        //    tempDirection = DefaultThrowDirection * throwPower;
        //}

        ShowOrbit();
        // #Progress
        //if (chargeTime == 0)
        //{
        //    throwPower = maxThrowPower;
        //}
        throwPower = maxThrowPower * throwObjSpeed;
        if (throwObj == null)
        {
            throwObj = Instantiate(defaultThrowObj, ThrowPoint, transform.rotation);
            throwObj.transform.position = throwPoint.position;
            throwObj.GetComponent<Throwable>().SetMovement(transform.position, throwVec, throwPower, dir);

        }
        else
        {
            throwObj.transform.position = throwPoint.position;
            throwObj.GetComponent<Throwable>().SetMovement(transform.position, throwVec, throwPower, dir);
        }
        throwObj = null;


        //GameObject sbThrown = Instantiate(defaultThrowObj, ThrowPoint, transform.rotation);
        //sbThrown.GetComponent<SnowBallNormal>().SetMovement(transform.position, throwVec, throwPower, dir);
    }


    Vector2 throwVec = Vector2.zero;
    public Vector2 SearchTarget()
    {
        float g = 9.8f;
        Vector2 targetPos = Vector2.zero;
        float v0 = throwPower;

        float radius = v0 * v0 / g;
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        RaycastHit2D[] rays = Physics2D.CircleCastAll(transform.position, 1.5f, input, radius - 1.5f);
        float distance = 999;
        foreach(RaycastHit2D ray in rays)
        {
            if (ray.transform.tag != "EnemyBody")
            {
                continue;
            }
            if (ray.transform.GetComponent<EnemyTarget>().isDead)
            {
                continue;
            }
            //  到達可能
            if (targetPos.y < -g / 2 / v0 / v0 * targetPos.x * targetPos.x + v0 * v0 / 2 * g)
            {
                if (Vector2.Distance(targetPos, ThrowPoint) < distance)
                {
                    distance = Vector2.Distance(targetPos, ThrowPoint);
                    targetPos = ray.transform.position;
                }
            }
        }
        return targetPos;

        Collider2D[] collider2dList = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in collider2dList)
        {
            if(collider.tag != "EnemyBody")
            {
                continue;
            }
            bool throwDirAndTargetDir = ((collider.transform.position.x - transform.position.x) < 0 && dir < 0) ||
                        ((collider.transform.position.x - transform.position.x) > 0 && dir > 0);
            if (!throwDirAndTargetDir)
            {
                continue;
            }

            //tempDirection = targetPos - (Vector2)point.transform.position;
            //tempDirection.Normalize();
            //  到達可能
            if (targetPos.y < -g / 2 / v0 / v0 * targetPos.x * targetPos.x + v0 * v0 / 2 * g)
            {
                targetPos = collider.transform.position;
            }
        }
        return targetPos;
    }


    public void ThrowAnimeEnd()
    {
        spriteObj.transform.eulerAngles = Vector3.zero;
        state = PLYSTS.NORMAL;
        if(throwReservation)
        {
            throwReservation = false;
            isCharging = true;
        }
    }

    //無敵状態でないときだけ攻撃によってノックバックする
    public void DamageNockBack(float x, float y)
    {
        if (!IsInvincible)
        {
            breakEnabled = true;

            AddForceAnimatorVx(x);
            if (GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                AddForceAnimatorVy(y);
            }
            else
            {
                Debug.Log("ジャンプ中だったので大目に見てやろう");
            }
            jumped = true;

        }
    }
    //無敵状態でないときだけ攻撃によってノックバックする
    public void AddForcePC(float x, float y)
    {
        AddForceAnimatorVx(x);
        AddForceAnimatorVy(y);
    }
    //無敵状態
    public void SetInvincible(float time)
    {
        if (IsInvincible || hp <= 0)
        {
            return;
        }
        anime.SetTrigger("Invincible");
        invincibleTime = time;
        invincibleStartTime = Time.fixedTime;
        IsInvincible = true;
    }

    public void CoinGet()
    {
        coin++;
        ;
    }
    public void CoinGet(int num)
    {
        coin += num;

    }

    public void Recovery(int i)
    {

        setHP(hp + i, hpMax);
    }

    public void Resurrection()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Stage1", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}


