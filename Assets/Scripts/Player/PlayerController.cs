using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool CanMove { get; set; }

    private float startingMoveSpeed;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform slashAnimSpawnPoint;

    private PlayerControls playerControls;
    private Knockback knockback;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer sr;

    public bool FacingLeft { get { return facingLeft; }  }
    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        CanMove = true;
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;
        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        PlayerInput();
        AdjustPlayerFacingDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    public Transform GetSlashAnimSpawnPoint()
    {
        return slashAnimSpawnPoint;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        //setting the movementso that it can detect movement
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || CanMove == false)
        {
            return;
        }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            sr.flipX = true;
            facingLeft = true;
        }
        else
        {
            sr.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = 0.2f;
        float dashCD = 0.25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
