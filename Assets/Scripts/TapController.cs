using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float TapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    Rigidbody2D rigidbody1;
    Quaternion downRotation;
    Quaternion forwardRotation;

    void Start()
    {
        rigidbody1 = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 45);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forwardRotation;
            rigidbody1.velocity = Vector3.zero;
            rigidbody1.AddForce(Vector2.up * TapForce, ForceMode2D.Force);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ScoreZone")
        {
            OnPlayerScored();
        }
        if(collision.gameObject.tag == "DeadZone")
        {
            rigidbody1.simulated = false;

            OnPlayerDied();
        }
    }
}
