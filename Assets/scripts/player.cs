using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed = 200f;
    float xMove;    
    float yMove;

    bool interactiing = false;
    bool haveTriangle = false;
    public Transform carryPoint;
    float staleDeath = 0.6f;
    float staleTimer = 0;
    public GameObject grabbedTriangle;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        staleTimer += Time.deltaTime;
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            interactiing = true;
            staleTimer = 0;
        }
        if (Input.GetKeyUp(KeyCode.Space) && interactiing || staleTimer > staleDeath)
        {
            interactiing = false;
        }
        print("interacting? : " + interactiing);
        //print(staleTimer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            speed = 100;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            speed = 200;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Table")
        {
            grabbedTriangle.transform.SetParent(collision.transform, false);
        } 

        if (collision.tag == "Item" && !interactiing)
        {
            collision.transform.SetParent(null);
            Debug.Log("yes?");
        }

        if ( collision.tag == "Item" && interactiing)
        {
            haveTriangle = true;
            
            collision.transform.SetParent(transform, false);
            collision.transform.localPosition = carryPoint.localPosition;
            grabbedTriangle = collision.GameObject();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xMove, yMove) * speed * Time.deltaTime;
    }
}
