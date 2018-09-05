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
    FreeVector,
    AutoCursor
}
public class PlayerController : BaseCharacterController
{

    // TODO 後で処理をManagerに移す
    public bool isStarted = true;

    public float slidePower = 1;
    public GameObject snowParticle;
    float resurrectionTime = 2f;
    bool canWalkWhileThrowing = false;


    //外部パラメータ
    [System.NonSerialized] public bool pushedRightB = false;
    [System.NonSerialized] public float chargeTime = 0.5f;
    [System.NonSerialized] public int NumberOfCheckPoint = 10;
    public float maxThrowPower = 12f;
    public float initHpMax = 4.0f;
    [Range(0.1f, 100.0f)] public float initSpeed = 12.0f;
    public GameObject defaultThrowObj;   //投げた雪玉
    [System.NonSerialized] public float groundY = 0.0f;
    [System.NonSerialized] public static int coin = 5000;
    [SerializeField] Vector2 cameraOffset = Vector2.zero;
    public MessageWindowController_old msgWindow;
    public static int CheckPointNumber = 0;
    public static bool enterDoor = false;

    public GameObject damageEffect;
    public Transform throwPoint;
    

    ThrowOrbitController orbits;


    //鼻照準
    Vector3 oldThrowEuler = Vector3.zero;
    GameObject spriteObj;
    Vector2 throwDirection = Vector2.zero;

    //内部パラメータ
    Transform frontPoint;
    [System.NonSerialized] public float throwPower = 0;
    bool breakEnabled = true;
    float groundFriction = 0.0f;
    [SerializeField]float jumpUpPower = 0;
    bool canJumpUp = true;
    float invincibleStartTime;
    float invincibleTime;
    bool throwReservation = false;
    float touchGroundAndJumpingTime = 0;
    float autoCursorTime = 0;
    [SerializeField] float freeVectorCursorSpeed = 15;
    bool IsAutoCursorUp = true;
    int jumpCount = 0;
    PlayerThrowPointCollider throwPointCollider;


    [NonSerialized]public GameObject throwObj;

    [SerializeField, Header("ジャンプ前の一瞬の踏ん張りを入れるかどうか")] bool preparationToJump = true;

    [SerializeField, Header("最初のジャンプ力")] float jumpPower = 10f;
    [SerializeField, Header("ジャンプ押し続けで上昇する時間")] float jumpTimeMax = 0.4f;
    [SerializeField, Header("ジャンプ押し続け時の上昇力")] float[] jumpPowers = { 100, 80, 60, 40, 20 };


    [SerializeField, Header("投げ操作の設定")] THROWTYPE throwType = THROWTYPE.Parabola2Way;
    [SerializeField, Header("自動カーソルが90度動くまでの秒数")] float autoCursorTimeToVertical = 1;
    [SerializeField, Header("自動カーソルを往復させるか")] bool autoCursorBothWays = false;


    [SerializeField, Header("オートカーソルのスピードを自動調節するか")] bool autoCursorSpeedAdjust;

    [SerializeField, Header("投げたものの最高到達点")] float maxThrowHeight = 7.5f;
    [SerializeField, Header("雪玉投げられるか")] bool canThrowSnowBall = false;

    bool IsPreThrow {
        get { return IsCurrentAnimation("Base Layer.Player_PreThrow"); }
    }

    bool IsThrow
    {
        get { return IsCurrentAnimation("Base Layer.Player_Throw"); }
    }

    bool IsJump
    {
        get { return IsCurrentAnimation("Base Layer.Player_Jump") || IsCurrentAnimation("Base Layer.Player_JumpNoPreparation")  ; }
    }

    bool IsJumpFall
    {
        get { return IsCurrentAnimation("Base Layer.Player_JumpFall"); }
    }

    bool IsJumpLanding
    {
        get { return IsCurrentAnimation("Base Layer.Player_JumpLanding"); }
    }

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
        anime.SetTrigger("EndInvincible");

        targetTalkTo = transform.Find("TargetTalkTo");
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        throwPoint = transform.Find("PlayerSprite/ThrowPoint");
        frontPoint = transform.Find("FrontPoint");
        throwPointCollider = throwPoint.gameObject.GetComponent<PlayerThrowPointCollider>();
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

    Vector2 oldVelocity = Vector2.zero;
    protected override void FixedUpdateCharacter()
    {
        throwPower = maxThrowPower;

        //落下開始

        if (grounded && IsJump)
        {
            touchGroundAndJumpingTime += Time.fixedDeltaTime;
        } else
        {
            touchGroundAndJumpingTime = 0;
        }

        if (IsPreThrow)
        {
            soundManager.PlaySEIfNotPlaying("ThrowChargeMax");
            orbits.SetOrbitsAppearance(Color.blue, 1.3f);
            orbits.SetOrbitsActive(true);

            ShowOrbit();
            ThrowRotate();

            if (!Input.GetButton(KeyConfig.Fire1))
            {
                Throw();
            }
        }
        else
        {
            orbits.SetOrbitsActive(false);
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

        
        switch (GetCurrentAnimation())
        {   //接地判定ラインの設定
            default:
                centerY = cameraOffset.y;
                break;
        }

        if (grounded)
        {
            anime.ResetTrigger("JumpFall");
            anime.ResetTrigger("JumpLanding");
            if (IsJumpFall)
            {
                anime.SetTrigger("JumpLanding");

            }
            if(IsJump)
            {
                anime.SetTrigger("jumpFall");

            }
        }
        if (rbody2D.velocity.y < 0 && !grounded)
        {
            anime.ResetTrigger("JumpLanding");
            if (!IsJumpFall)
            {
                anime.SetTrigger("JumpFall");
            }
        }

        //着地しているのに上への力が働いているとジャンプが解除されないため
        if (touchGroundAndJumpingTime >= 0.1f)
        {
            anime.SetTrigger("JumpLanding");
            touchGroundAndJumpingTime = 0;
        }

        //　着地チェック
        if (jumped)
        {
            if ((grounded && !groundedPrev) ||
                (grounded && Time.fixedTime > jumpStartTime + 1.0f))
            {
                if(!IsCurrentAnimation("Base Layer.Player_Idle") &&
                    !IsCurrentAnimation("Base Layer.Player_PreThrow") &&
                    !IsCurrentAnimation("Base Layer.Player_Throw"))
                {

                }

                if(IsJump)
                {
                    anime.SetTrigger("JumpLanding");
                }
                jumped = false;

            }
        }



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


        oldVelocity = rbody2D.velocity;
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
        if (throwPointCollider.FoundInterrupter)
        {
            orbits.SetOrbitsActive(false);
            return;
        }

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
            case THROWTYPE.AutoCursor:
                ShowOrbitAutoCursor();
                break;
        }

    }

    void ShowOrbitAutoCursor()
    {

        if(!Input.GetButton("RB"))
        {
            if (IsAutoCursorUp)
            {
                autoCursorTime += Time.deltaTime;
            } else
            {
                autoCursorTime -= Time.deltaTime;
            }
        }

        float radian = 0;
        if (autoCursorSpeedAdjust) {
            radian = -90 * ((autoCursorTime - autoCursorTimeToVertical) * (autoCursorTime - autoCursorTimeToVertical)) + 90;
            radian *= Mathf.Deg2Rad;
        }
        else
        {
            radian = autoCursorTime / autoCursorTimeToVertical * 90 * Mathf.Deg2Rad;
        }
        throwPower = Mathf.Sqrt(2 * 9.8f * maxThrowHeight * throwObj.GetComponent<Throwable>().carryMultiplier * throwObj.GetComponent<Rigidbody2D>().gravityScale);
        //Mathf.Sqrt(2 * 9.8f * 20)
        Vector2 throwDirection = new Vector2(Mathf.Cos(radian) * dir, Mathf.Sin(radian));
        throwVec = orbits.ShowOrbitByVector(ThrowPoint, throwDirection, throwPower, dir);

        if(autoCursorTime / autoCursorTimeToVertical > 1)
        {
            if (autoCursorBothWays)
            {
                IsAutoCursorUp = false;
            } else
            {
                autoCursorTime = 0;
            }
        } else if(autoCursorTime / autoCursorTimeToVertical < 0)
        {
            IsAutoCursorUp = true;
        }
    }
    void ShowOrbitFreeVector()
    {
        orbits.CaptureObjPosition += StickDirection * Time.deltaTime * freeVectorCursorSpeed;
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

        orbits.SetGravity(throwObj.GetComponent<Rigidbody2D>().gravityScale);
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

        if (IsPreThrow || IsThrow)
        {
            if (canWalkWhileThrowing && !pushedRightB)
            {
                n /= 2;
            }
        }

            if (pushedRightB && throwType != THROWTYPE.AutoCursor)
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
    public void JumpButtonDown()
    {
        //if (rbody2D.velocity.y > 0) return;

        if (grounded)
        {

            if (!IsPreThrow && !Input.GetButtonDown(KeyConfig.Fire1))
            {
                spriteObj.transform.rotation = transform.rotation;
                if(preparationToJump)
                {
                    anime.SetTrigger("Jump");
                }
                else
                {
                    anime.SetTrigger("JumpNoPreparation");
                }



            } else
            {
                StartJump();
            }


            //if (preparationToJump) {
            //    spriteObj.transform.rotation = new Quaternion(0, 0, 0, 1);
            //    if (!IsPreThrow)
            //    {
            //        anime.SetTrigger("Jump");

            //    }
            //}
            //else
            //{
            //    StartJump();
            //}
            //rbody2D.velocity += Vector2.up * (jumpUpPower * (Time.fixedTime - jumpStartTime) / jumpTimeMax);
        }

    }

    public void StartJump()
    {
        rbody2D.velocity = new Vector2(rbody2D.velocity.x, 0);
        rbody2D.velocity += Vector2.up * jumpPower;
        jumpStartTime = Time.fixedTime;
        jumped = true;
        canJumpUp = true;
        jumpCount = 0;
    }

    public void JumpButton()
    {
        float jumpTime = Time.fixedTime - jumpStartTime;

        if (jumped)
        {
            if (canJumpUp)
            {

                if (jumpTime > 0.2f / 5 * (jumpCount + 1 ))
                {
                    rbody2D.velocity += Vector2.up * jumpPowers[jumpCount];
                    jumpCount++;
                }
            }

            if (jumpTime >= jumpTimeMax)
            {
                EndJumpUp();
            }


        }
    }
    public void EndJumpUp()
    {
        jumped = false;
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

        GameObject throwObj = FindThrowObj();

        GameObject inputInterrupter = FindInputInterrupter();
        if (inputInterrupter != null && (throwObj == null || throwObj.GetComponent<Throwable>().IsTaken))
        {
            Debug.Log("Putout");
            inputInterrupter.GetComponent<Oven>().PutOut();
            return;
        }

        if (throwObj != null)
        {
            SetThrowObj(throwObj);
            orbits.SetGravity(throwObj.GetComponent<Rigidbody2D>().gravityScale);
        }
        else
        {
            if(!canThrowSnowBall)
            {
                return;
            }
            SetThrowObj();
            orbits.SetGravity(defaultThrowObj.GetComponent<Rigidbody2D>().gravityScale);
        }
        if (IsThrow)
        {
        //    Debug.Log("Reservation");
            throwReservation = true;
        }

        if (IsPreThrow || IsThrow)
        {
            return;
        }

        anime.SetTrigger("PreThrow");
        anime.ResetTrigger("Throw");
        ShowOrbit();
    }

    GameObject FindThrowObj()
    {
        RaycastHit2D[] hit2D = new RaycastHit2D[5];
        int num = Physics2D.BoxCastNonAlloc(throwPoint.transform.position, new Vector2(2f, 2f), 0, Vector2.right * dir, hit2D, 0.1f);

        foreach(RaycastHit2D hit in hit2D)
        {
            if (hit.collider == null) continue;

            if(hit.collider.gameObject.CompareTag("Throwable")) {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
    public void SetThrowObj()
    {
        throwObj = defaultThrowObj;
    }

    public void SetThrowObj(GameObject throwObj)
    {
        this.throwObj = throwObj;
        Throwable throwable = this.throwObj.GetComponent<Throwable>();
        throwable.IgnoreCollision(gameObject, true);
        throwable.OnHeld(throwPoint.gameObject);
    }

    public void ThrowChargeSE()
    {
     //   soundManager.PlaySE("ThrowCharge");
    }
    public void Throw()
    {
        anime.ResetTrigger("PreThrow");
        if (IsPreThrow)
        {
            anime.SetTrigger("Throw");
        }
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


        throwPower = maxThrowPower;

        if (!throwPointCollider.FoundInterrupter)
        {
            if (throwObj == defaultThrowObj)
            {
                throwObj = Instantiate(defaultThrowObj, ThrowPoint, transform.rotation);
                throwObj.transform.position = throwPoint.position;
                throwObj.GetComponent<Rigidbody2D>().gravityScale = defaultThrowObj.GetComponent<Rigidbody2D>().gravityScale;
                throwObj.GetComponent<Throwable>().OnThrew(transform.position, throwVec, throwPower, dir);
            }
            else
            {
                throwObj.transform.position = throwPoint.position;
                throwObj.GetComponent<Throwable>().OnThrew(transform.position, throwVec, throwPower, dir);
            }
            throwObj = null;
        } else
        {
            throwObj.GetComponent<Throwable>().OnRelease();
            GameObject inputInterrupter = throwPointCollider.GetFirstInterrputer();
            inputInterrupter.GetComponent<Oven>().PutIn(throwObj.GetComponent<Present>());

            throwObj = null;
        }

        autoCursorTime = 0;
        orbits.HideCaptureObj();
        //GameObject sbThrown = Instantiate(defaultThrowObj, ThrowPoint, transform.rotation);
        //sbThrown.GetComponent<SnowBallNormal>().SetMovement(transform.position, throwVec, throwPower, dir);
    }

    GameObject FindInputInterrupter()
    {

        Transform point = transform.Find("PlayerSprite/ThrowPoint");
        RaycastHit2D[] rays = Physics2D.CircleCastAll(point.position, 1.5f, Vector2.zero, 0);
        float distance = 999;
        foreach (RaycastHit2D ray in rays)
        {
            if (ray.transform.tag == "InputInterrupter")
            {
                return ray.transform.gameObject;
            }
        }
        return null;
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
        if(throwReservation)
        {
            PreThrow();
            throwReservation = false;
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


