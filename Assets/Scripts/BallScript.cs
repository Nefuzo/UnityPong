using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class BallScript : NetworkBehaviour
{
    private Rigidbody2D rigid;

    float rfloat;
    bool Exploded;
    bool CorutineStarted;
    Vector2 Force;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        DefBallValuesSet();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "EndBox")
        {
            transform.position = new Vector2(0, 0);
            rigid.velocity = Vector2.zero;
            DefBallValuesSet();
            CorutineStarted = false;
            Exploded = false;
        }
    }
    void Update()
    {
        if (!Exploded) rigid.AddForce(Force);
        if (!CorutineStarted) StartCoroutine(StopForce());
        CorutineStarted = true;
    }

    void DefBallValuesSet()
    {
        rfloat = Random.Range(-1.5f, 1.5f);
        Force = new Vector2(1, rfloat) * 1.5f;
    }

    IEnumerator StopForce()
    {
        yield return new WaitForSeconds(0.5f);
        Exploded = true;
    }
}
