using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Pauser))]
public class Throwable : MonoBehaviour
{
    [SerializeField] float gravity = 4;
    [SerializeField] List<string> attributes;

    public float carryMultiplier = 1;
    Rigidbody2D rigid;
    Collider2D collider2D;
    GameObject playerObj;
    [SerializeField]GameObject holdObj = null;

    public bool hasBeThrew = false;
    GameObject outlineObj;

    bool isTaken = false;
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
        gameObject.AddComponent<Pauser>();
    }

    void Initialize()
    {
        outlineObj = Resources.Load<GameObject>("PresentOutline");
        GameObject obj = Instantiate(outlineObj, transform);
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        obj.SetActive(false);
        outlineObj = obj;
        collider2D = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravity;

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

            rigid.velocity = Vector2.zero;
            transform.position = holdObj.transform.position + Vector3.up * 0.8f;
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

        if(attributes.Contains("Fragile"))
        {
            if(c.transform.CompareTag("Road")  && hasBeThrew && !IsTaken)
            {
                Debug.Log("Fragile Destroy");
                GameObject.Find("PresentManager").GetComponent<PresentManager>().HidePresentFromView(gameObject);
                Destroy(gameObject);
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
