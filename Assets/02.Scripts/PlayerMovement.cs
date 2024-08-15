using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;
    PlayerInput c_playerInput;
    Rigidbody rb;
    Animator ani;

    readonly int hashMove = Animator.StringToHash("move");

    void Start()
    {
        c_playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Rotate();
        Move();

        ani.SetFloat(hashMove, c_playerInput.move);
    }

    private void Move()
    {
        Vector3 moveDist = moveSpeed * c_playerInput.move * Time.deltaTime * transform.forward; //이동거리 계산
        Vector3 destination = rb.position + moveDist;   //현재 위치 + 이동 거리 => 캐릭터가 위치해야 할 좌표 계산
        rb.MovePosition(destination);
    }

    void Rotate()
    {
        float turn = c_playerInput.rotate * rotateSpeed * Time.deltaTime;
        rb.rotation *= Quaternion.Euler(0f, turn, 0f);
    }
}
