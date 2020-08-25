using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Gerbil : MonoBehaviour
{

    public Sprite Idle;
    public Sprite Hug;
    public float HumpRate = 0.5f;
    public float minAllowed = 0.02f;
    private SpriteRenderer sr;
    private static Gerbil Instance;
    private float _t = 0;

    public bool hasWon = false;
    private bool _hasAttached = false;
    private bool hasAttached
    {
        get
        {
            return _hasAttached;
        }
        set
        {
            if (_hasAttached != value)
                UpdateSpriteImage(value);
            _hasAttached = value;
        }
    }

    private void UpdateSpriteImage(bool Attached)
    {
        sr.sprite = Attached ? Hug : Idle;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Idle == null)
            Debug.LogError("Missing Idle Sprite!");
        if (Hug == null)
            Debug.LogError("Missing Hug Sprite!");

        if (Idle == null || Hug == null)
            this.enabled = false;

        sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = Idle;
        Instance = this;
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasAttached) return;
        if( collision.gameObject.GetComponent<MouseGrab>() != null )
        {
            hasAttached = true;
            sr.sprite = Hug;
        }
    }

    public static void HasWon()
    {
        Instance.hasWon = true;
    }

    // show the gerbil the humping animation
    private void Update()
    {
        if (hasWon)
        {
            _t += Time.deltaTime;
            if (_t > HumpRate)
            {
                if (sr.sprite == Hug)
                    sr.sprite = Idle;
                else
                    sr.sprite = Hug;
                _t = 0;
                HumpRate = Mathf.Clamp(HumpRate - 0.005f, minAllowed, HumpRate);
            }
        }
        else
        {
            hasAttached = this.transform.parent != null;
        }
    }
}
