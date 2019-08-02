using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool vertical; //true = vertical moving, false = horizontal moving
    public float space; //the amount of space to allow moving
    public float speed;

    private float start;
    private float moveTo;
    private float step;
    private Vector2 moveToVector;
    private bool movement; // true for positive, false for negative

    void Start() {
        if (vertical)
        {
            start = transform.position.y;
        } else 
        {
            start = transform.position.x;
        }
        moveTo = start + space;
        step = (start - moveTo) / speed;
        movement = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (vertical) 
        {
            moveToVector = new Vector2 (transform.position.x, Move(transform.position.y));
            transform.position = moveToVector;
        } else 
        {
            moveToVector = new Vector2 (Move(transform.position.x), transform.position.y);
            transform.position = moveToVector;
        }
    }

    private float Move(float a) {
        if (movement && a < start + space) {
            return a - step;
        }
        else if (movement && a >= start + space) {
            movement = false;
            if (!vertical)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            return a + step;
        }
        else if (!movement && a < start - space) {
            movement = true;
            if (!vertical)
                transform.Rotate(0.0f, -180.0f, 0.0f);
            return a - step;
        }
        return a + step;
    }
}
