using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;

    private void Update()
    {
        float horizontalInput = Mathf.Max(0, Input.GetAxis("Horizontal"));
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
    }
}
