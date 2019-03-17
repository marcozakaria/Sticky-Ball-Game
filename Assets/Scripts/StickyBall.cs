using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBall : MonoBehaviour
{
    public float facingAngle = 0;

    private float x = 0, z = 0;
    private Rigidbody ballRigidbody;
    Vector2 movment;

    public Transform cameraTransform;
    float distanceToCamera = 5;

    float ballSize = 1;

    public GameObject category1;
    bool category1Unlocked = false;
    public GameObject category2;
    bool category2Unlocked = false;
    public GameObject category3;
    bool category3Unlocked = false;

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * -100;
        z = Input.GetAxis("Vertical") * Time.deltaTime * 500;

        facingAngle += x;
        movment = new Vector2(Mathf.Cos(facingAngle * Mathf.Deg2Rad), Mathf.Sin(facingAngle * Mathf.Deg2Rad));
    }

    private void FixedUpdate()
    {
       ballRigidbody.AddForce(new Vector3(movment.x, 0, movment.y) * z * 3);

       cameraTransform.position = new Vector3(-movment.x * distanceToCamera, distanceToCamera, -movment.y * distanceToCamera) + this.transform.position;

        unlockPickupCategories();
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
