using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;
    PlayerInput c_playerInput;
    Rigidbody rb;
    Animator aniPlayer;

    readonly int hashMove = Animator.StringToHash("move");

    void Start()
    {
        c_playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        aniPlayer = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Rotate();
        Move();

        aniPlayer.SetFloat(hashMove, c_playerInput.move);
    }

     private void Move()
    {
        Vector3 moveDist = moveSpeed * c_playerInput.move * Time.deltaTime * transform.forward;
        Vector3 newPosition = rb.position + moveDist;
        rb.MovePosition(newPosition);
    }

    void Rotate()
    {
        float turn = c_playerInput.rotate * rotateSpeed * Time.deltaTime;
        rb.rotation = rb.rotation * Quaternion.Euler(0f, turn, 0f);
    }
}
