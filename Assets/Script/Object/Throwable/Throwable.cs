using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

[RequireComponent(typeof(Pauser))]
public class Throwable : MonoBehaviour
{
    [SerializeField] float initialGravity = 6.15f;
    [SerializeField] List<string> attributes;

    public float carryMultiplier = 1;
    Rigidbody2D rigid;
    Collider2D collider2D;
    GameObject playerObj;
    [SerializeField]GameObject holdObj = null;

    float flashTime;
    float disappearTime;
    float leftTime = 0;
    float gravity;
    public bool hasBeThrew = false;
    GameObject outlineObj;

    [SerializeField] GameObject brokenObj;
    [Inject]
    PresentManager presentManager;

    Coroutine flashCorutine;
    bool isFlashing = false;
    bool isTaken = false;
    [SerializeField]bool isInParachute = false;

    int thrown;

    public bool IsTaken {
        get {
            return holdObj != null;
        }
    }
    int maxBoundCount = 0;

    int boundCount = -99;

    Subject<Unit> threwSubject = new Subject<Unit>();
    public IObservable<Unit> OnThrewEvent
    {
        get
        {
            return threwSubject;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    void Start()
    {
        thrown = LayerMask.NameToLayer("ThrowableThrown");
        Pauser pauser = gameObject.AddComponent<Pauser>();
        pauser.pauseType = PauseType.KeepRigidbody;
    }

    void Initialize()
    {
        gravity = initialGravity;
        outlineObj = Resources.Load<GameObject>("PresentOutline");
        GameObject obj = Instantiate(outlineObj, transform);
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        obj.SetActive(false);
        outlineObj = obj;
        collider2D = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = initialGravity;
        disappearTime   = presentManager.presentDisappearTime;
        flashTime       = presentManager.flashTime;

        foreach (string attribute in attributes)
        {
            string[] elements = attribute.Split(' ');
            switch (elements[0])
            {
                case "Bound":
                GetComponent<Collider2D>().sharedMaterial.bounciness = 0;
                GetComponent<Collider2D>().sharedMaterial.friction = 1;
                    rigid.drag = 1;
                    rigid.angularDrag = 10;
                    break;
                case "MaxBoundCount":
                    maxBoundCount = int.Parse(elements[1]);
                    break;
                case "Fragile":

                    break;
            }
        }
    }

    public void SetOutline(bool f)
    {
        outlineObj.SetActive(f);
    }

    public void IgnoreCollision(GameObject obj, bool flag)
    {
        foreach (Collider2D c in obj.GetComponentsInChildren<Collider2D>())
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), c, flag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInParachute)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;

            float y = Mathf.Clamp(rigid.velocity.y, -1f, 2f);
            rigid.velocity = new Vector2(rigid.velocity.x, y);

        }
        else
        {
            gravity = initialGravity;
        }
        if (attributes.Contains("Bound"))
        {
            Debug.Log("bounciness : " + GetComponent<Collider2D>().sharedMaterial.bounciness);
            Debug.Log("friction : " + GetComponent<Collider2D>().sharedMaterial.friction);
            Debug.Log("Gravity : " + GetComponent<Rigidbody2D>().gravityScale);
            Debug.Log("Drag : " + GetComponent<Rigidbody2D>().drag);
            if (boundCount >= maxBoundCount)
            {

                // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                // GetComponent<Rigidbody2D>().angularVelocity = 0;

                GetComponent<Collider2D>().sharedMaterial.bounciness = 0;
                GetComponent<Collider2D>().sharedMaterial.friction = 1;
                GetComponent<Rigidbody2D>().gravityScale = gravity;
                //rigid.drag = 10;
                boundCount++;
                if(rigid.velocity.y > 0)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x / 2, rigid.velocity.y / 2);
                }
                if(rigid.velocity.magnitude < 10)
                {
                    rigid.velocity = Vector2.zero;
                }
            }
        }
        if (holdObj != null)
        {
                        GetComponent<SpriteRenderer>().color = Color.white;
            rigid.velocity = Vector2.zero;
            transform.position = holdObj.transform.position + Vector3.up * 0.8f;
        } else if(!isInParachute)
        {
            leftTime += Time.deltaTime;
        }

        if (presentManager.autoDisappearPresent)
        {
            if (leftTime >= disappearTime - flashTime)
            {
                if (flashCorutine == null)
                {
                    flashCorutine = StartCoroutine(Flash());
                }
            }
            if (leftTime >= disappearTime)
            {
                presentManager.GetComponent<PresentManager>().HidePresentFromView(gameObject);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Flash()
    {
        isFlashing = true;
        float flashTime = 0.1f;
        while(true)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            yield return new WaitForSeconds(flashTime);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(flashTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (attributes.Contains("Bound"))
        {
            if (boundCount >= 0 && boundCount < maxBoundCount)
            {
                //GetComponent<Collider2D>().sharedMaterial.bounciness = 1;
                //GetComponent<Collider2D>().sharedMaterial.friction = 0;
                //GetComponent<Rigidbody2D>().gravityScale = 0;
                //rigid.drag = 0;
                boundCount++;
            }

        }

        if (attributes.Contains("Fragile"))
        {
            if (c.transform.CompareTag("Road") && hasBeThrew && !IsTaken)
            {
                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySEOneShot("CakeBreak");
                Instantiate(brokenObj, transform.position, transform.rotation);
                presentManager.HidePresentFromView(gameObject);
                Destroy(gameObject);
            }
        }
        if (c.transform.CompareTag("Road")) { 
        gameObject.layer = LayerMask.NameToLayer("Throwable");
        if (isInParachute)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            isInParachute = false;
        }
    }
    }

    private void OnCollisionExit2D(Collision2D c)
    {

    }

    IEnumerator CollisionControl()
    {
        yield return new WaitForSeconds(0.2f);
    }

    public void OnHeld(GameObject holdObj)
    {
        this.holdObj = holdObj;
        isInParachute = false;
        leftTime = 0;
        if (flashCorutine != null)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            StopCoroutine(flashCorutine);
            flashCorutine = null;
        }
        isFlashing = false;
        rigid.Sleep();
    }

    public void OnRelease()
    {

        Debug.Log("On Release");
        rigid.WakeUp();
        isTaken = false;
        holdObj = null;
    }

    

    public void OnThrew(Vector2 destPos, Vector2 throwDirection, float power, float dir)
    {
        hasBeThrew = true;

        gameObject.layer = thrown;

        if (attributes.Contains("Bound"))
        {
            GetComponent<Collider2D>().sharedMaterial.bounciness = 1;
            GetComponent<Collider2D>().sharedMaterial.friction = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            rigid.drag = 0;
            boundCount = 0;
        }
       // Initialize();
        isTaken = false;
        if(collider2D == null)
        {
            collider2D = GetComponent<Collider2D>();
            rigid = GetComponent<Rigidbody2D>();
        }
        collider2D.isTrigger = false;
        Vector2 vec = throwDirection;
        vec.Normalize();

        if (vec == Vector2.zero)
        {
            vec = new Vector2(1 * dir, 0.2f);
        }

        vec = throwDirection;
        //Vector2 force = vec * rigid.mass;
        //StartCoroutine(CollisionControl());
        holdObj = null;
        rigid.velocity = vec;
        //rigid.AddForce(force, ForceMode2D.Impulse);
        threwSubject.OnNext(Unit.Default);
    }
}
