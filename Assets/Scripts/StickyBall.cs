using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBall : MonoBehaviour
{
    public float facingAngle = 0;

    public float horizontalSpeed = 100;
    public float verticalSpeed = 500;

    private float x = 0, z = 0;
    private Rigidbody ballRigidbody;
    Vector2 movment;

    public Transform cameraTransform;
    float distanceToCamera = 5;

    float ballSize = 1;

    //touch input
    [Header("Touch input")]
    public GameObject outerCircle;
    public GameObject innerDot;
    private Touch oneTouch;
    private Vector2 touchPosition, touchDirection;
    private RectTransform innerTransform, outerTransorm;
    private Camera mainCamera;

    [Space(10)]
    public GameObject category1;
    bool category1Unlocked = false;
    public GameObject category2;
    bool category2Unlocked = false;
    public GameObject category3;
    bool category3Unlocked = false;

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();

        innerTransform = innerDot.GetComponent<RectTransform>();
        outerTransorm = outerCircle.GetComponent<RectTransform>();
        outerCircle.SetActive(false);
        innerDot.SetActive(false);       

        mainCamera = Camera.main;
    }

    private void Update()
    {
#if UNITY_EDITOR
        x = Input.GetAxis("Horizontal") * Time.deltaTime * -horizontalSpeed;
        z = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed;
#else
         TouchInput();
        x = touchDirection.x;
        z = touchDirection.y;
#endif


        facingAngle += x;
        movment = new Vector2(Mathf.Cos(facingAngle * Mathf.Deg2Rad), Mathf.Sin(facingAngle * Mathf.Deg2Rad));
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            //Debug.Log("touch");
            oneTouch = Input.GetTouch(0);
            touchPosition = mainCamera.ScreenToViewportPoint(oneTouch.position);
            touchPosition = new Vector2(touchPosition.x * Screen.width, touchPosition.y * Screen.height);

            switch (oneTouch.phase)
            {
                case TouchPhase.Began:
                    outerCircle.SetActive(true);
                    innerDot.SetActive(true);
                    Debug.Log(touchPosition);
                    outerTransorm.position = touchPosition;
                    innerTransform.position = touchPosition;
                    break;
                case TouchPhase.Moved:
                    TouchMovment();
                    break;
                case TouchPhase.Stationary:
                    TouchMovment();
                    break;
                case TouchPhase.Ended:
                    outerCircle.SetActive(false);
                    innerDot.SetActive(false);
                    touchDirection = Vector2.zero;
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }

    void TouchMovment()
    {
        innerDot.transform.position = touchPosition;

        innerDot.transform.position = new Vector2(
            Mathf.Clamp(innerTransform.position.x, outerTransorm.position.x - 60, outerTransorm.position.x + 60),
            Mathf.Clamp(innerTransform.position.y, outerTransorm.position.y - 60, outerTransorm.position.y + 60));

        touchDirection = (innerTransform.position - outerTransorm.position).normalized;

    }

    private void FixedUpdate()
    {
       ballRigidbody.AddForce(new Vector3(movment.x, 0, movment.y) * z * 3);

       cameraTransform.position = new Vector3(-movment.x * distanceToCamera, distanceToCamera, -movment.y * distanceToCamera) + this.transform.position;

        //unlockPickupCategories();
    }

    private void unlockPickupCategories()
    {
        if (category1Unlocked == false)
        {
            if (ballSize >= 1)
            {
                category1Unlocked = true;
                for (int i = 0; i < category1.transform.childCount; i++)
                {
                    category1.transform.GetChild(i).GetComponent<Collider>().isTrigger = true;
                }
            }
        }
        else if (category2Unlocked == false)
        {
            if (ballSize >= 1.5f)
            {
                category2Unlocked = true;
                for (int i = 0; i < category2.transform.childCount; i++)
                {
                    category2.transform.GetChild(i).GetComponent<Collider>().isTrigger = true;
                }
            }
        }
        else if (category3Unlocked == false)
        {
            if (ballSize >= 2f)
            {
                category3Unlocked = true;
                for (int i = 0; i < category3.transform.childCount; i++)
                {
                    category3.transform.GetChild(i).GetComponent<Collider>().isTrigger = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sticky"))
        {
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            ballSize += 0.02f;
            distanceToCamera += 0.08f;
            other.enabled = false; // diasable collider

            other.transform.SetParent(this.transform);
        }
    }
}
