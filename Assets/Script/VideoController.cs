﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VideoController : MonoBehaviour
{
    private static int numInstances = 0;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Vector3 defaultRot;
    public float maxSpeed = 50f;
    RawImage screen;


    // Use this for initialization : define statics and dynamics properties
    void Start()
    {
        defaultRot = transform.eulerAngles;
        rb = GetComponent<Rigidbody2D>();
        float dirX = (float)Random.Range(0, 1);
        dirX = 2f * dirX - 1f;
        float dirY = (float)Random.Range(0, 1);
        dirY = 2f * dirY - 1f;
        rb.AddForce(new Vector2(dirX * Random.Range(0f, maxSpeed * 50), dirY * Random.Range(0f, maxSpeed * 50)));

        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta -= new Vector2(20, 20);
        numInstances += 1;
    }

    public void SetUpFilm(string imgPath)
    {
        screen = GetComponent<RawImage>();
        screen.enabled = true;

        Vector3 scale;
        if (screen.texture.width > screen.texture.height)
        {
            scale = new Vector3(1f, (float)screen.texture.height / (float)screen.texture.width, 1f);
        }
        else
        {
            scale = new Vector3((float)screen.texture.width / (float)screen.texture.height, 1f, 1f);
        }
        screen.transform.localScale = scale;
        bc = GetComponent<BoxCollider2D>();
        bc.transform.localScale = scale;
    }

    public void SetSize(float width, float height)
    {
        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(width, height);
        GetComponent<BoxCollider2D>().size = new Vector2(width, height);
    }


    public void SetPos(Vector3 pos)
    {
        RectTransform r = GetComponent<RectTransform>();
        r.transform.position = pos;
    }

   // void FixedUpdate()
   // {
   //     rb.AddForce (new Vector3 (dir, 0, 0));
   // }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = defaultRot;
        if (rb.velocity.x + rb.velocity.y < 0.0001)
        {
            float dirX = (float)Random.Range(0, 1);
            dirX = 2f * dirX - 1f;
            float dirY = (float)Random.Range(0, 1);
            dirY = 2f * dirY - 1f;
            rb.AddForce(new Vector2(dirX * Random.Range(0f, maxSpeed * 0.05f), dirY * Random.Range(0f, maxSpeed * 0.05f)));
        }
        rb.velocity = new Vector2(Mathf.Min(rb.velocity.x, maxSpeed), Mathf.Min(rb.velocity.y, maxSpeed));
    }

}
