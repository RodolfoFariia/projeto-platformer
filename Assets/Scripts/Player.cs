using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D _playerRB;
    private Animator _playerAnim;

    bool isRunning = false;

    private float x_dir;
    [SerializeField] private float x_speed;
    
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private int _jumpsLeft;
    private bool _isGrounded;

    void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        Movimentar();
    }

    // evento disparado pelo PlayerInput
    void OnMove(InputValue inputValue)
    {
        x_dir = inputValue.Get<Vector2>().x;
    }

    // evento disparado pelo PlayerInput JUMP
    void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed && _jumpsLeft > 0)
        {
            _playerRB.linearVelocityY = jumpForce;
            _jumpsLeft--;
        }
    }

    void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(
            groundCheck.position, groundCheckRadius, groundLayer);

        // só reseta os pulos se estiver no chão e não estiver subindo
        if (_isGrounded && _playerRB.linearVelocityY <= 0.01f)
        {
            _jumpsLeft = maxJumps;
        }
    }

    void Movimentar()
    {
        _playerRB.linearVelocityX = x_dir * x_speed;
        isRunning = Math.Abs(_playerRB.linearVelocityX) > Mathf.Epsilon;

        _playerAnim.SetBool("isRunning", isRunning);

        if (isRunning)
        {
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        float sinal = Mathf.Sign(_playerRB.linearVelocityX);
        transform.localScale = new Vector3(sinal, 1, 1);
    }
    
}