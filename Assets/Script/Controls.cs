using System.Collections;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [Header("Jump Buffer")]
    [SerializeField] private float _jumpBufferDuration = 0.25f;
    private float _jumpBufferTimer = 0f;


    [Header("Horizontal Mvt")]
    public float NormalSpeed;

    [Header("Vertical Mvt")]
    public float jump;
    public float bonusJumpStrength;

    [Header("Attaque")]
    [SerializeField] private GameObject slash;

    [Header("Dash")]
    [SerializeField] private TrailRenderer tr;

    [Header("Grounded")]
    public float deccalageGroundcheckX = -1f;
    public float deccalageGroundcheck = -1f;


    private Rigidbody2D _rb;
    private Collider2D _monColl;
    private SpriteRenderer _skin;
    private Animator anim;
    private WallDetector _detector;

    //OrientX
    private float _orientX = 1;
    private float _moveDirX = 0f;

    //Grounded
    private bool grounded;
    private Collider2D[] colls;

    //Jump 
    private int bonusJump;

    //WallJump
    private bool IsTouchingWallLeft = false;
    private bool IsTouchingWallRight = false;
    [HideInInspector]
    public bool IsWallJumping = false;

    //Dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private float horizontal;

    //Audio
    private AudioSource _audioSource;
    private AudioController _audioData;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _monColl = GetComponent<Collider2D>();
        _skin = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        _detector = GetComponent<WallDetector>();

        //Audio
        _audioSource = GetComponent<AudioSource>();
        _audioData = GetComponent<AudioController>();

        _CancelJumpBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        _UpdateJumpBuffer();

        groundCheck();
        _ApplyWallDetection();

        GetInputMoveX();
        _ChangeOrientFromHorizontalMovement();

        controlCheck();
        //flipCheck();
        animCheck();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        _rb.velocity = new Vector2(horizontal * NormalSpeed, _rb.velocity.y);

    }

    void controlCheck()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (IsSliding || grounded || bonusJump > 0)
            {
                SetUpAudio("jump", true);
                if (IsSliding)
                {
                    IsWallJumping = true;
                    WallJumpStart();
                }
                else if (grounded)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jump);
                }
                else if (bonusJump > 0)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, bonusJumpStrength);
                    bonusJump = 0;
                }
            }  // initialisations jumpBuffer
            else
            {
                _ResetJumpBuffer();
            }
        }

        if (IsJumpBufferActive())
        {
            if (grounded)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jump);
            }
        }

        // if(grounded && Input.GetButtonDown("Jump")){
        // }
        // if(!grounded && Input.GetButtonDown("Jump") && bonusJump>0){
        //     _rb.velocity = new Vector2(_rb.velocity.x, jump*2/3);
        //     bonusJump=0;
        // }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // if(IsSliding && Input.GetKeyDown(KeyCode.Space)){

        // }

        flipCheck();
    }

    //OrientX
    private void _ChangeOrientFromHorizontalMovement()
    {
        if (_moveDirX == 0f) return; // et si pas d'accolades le if execute juste la ligne d'apres 
        _orientX = Mathf.Sign(_moveDirX);
    }

    private void GetInputMoveX()
    {
        _moveDirX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            _moveDirX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Debug.Log(_orientX);
            _moveDirX = 1f;
        }
    }

    void groundCheck()
    {
        grounded = false;

        Vector3 offset = Vector3.right * deccalageGroundcheckX;
        Vector3 circlePosition = transform.position + Vector3.up * deccalageGroundcheck + offset;

        colls = Physics2D.OverlapCircleAll(circlePosition, _monColl.bounds.extents.x * 0.5f);
        foreach (Collider2D coll in colls)
        {
            if (coll != _monColl && !coll.isTrigger)
            {
                grounded = true;
                bonusJump = 1;
                break;
            }
        }
    }

    //Dash
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        SetUpAudio("dash", true);
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(horizontal * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        _rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    //Audio
    private void SetUpAudio(string label, bool play)
    {
        // set up l'audio clip
        _audioSource.clip = _audioData.GetAudioByLabel(label);
        // play ou pas play
        if (play)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
    }


    //Sliding
    private void _ApplyWallDetection()
    {
        IsTouchingWallLeft = _detector.DetectWallNearByLeft();
        IsTouchingWallRight = _detector.DetectWallNearByRight();
    }
    public bool IsSliding => (IsTouchingWallLeft || IsTouchingWallRight) && !grounded;

    //Wall jump01
    //Wall Jump
    private float _wallJumpTimer;

    public void WallJumpStart()
    {
        _wallJumpTimer = 0f;
        _UpdateWallJump();
    }

    private void _UpdateWallJump()
    {
        _wallJumpTimer += Time.deltaTime;

        if (_wallJumpTimer < 0.5f)
        {
            Vector2 velocity = _rb.velocity;

            horizontal = 3 * -_orientX;
            velocity.y = 6;
            //Debug.Log("orientX " + _orientX

            //_rb.velocity = velocity;
            _rb.velocity = new Vector2(horizontal, 6f);
        }
        else if (!grounded)
        {
            //Debug.Log("tombe"); 
        }
        else if (grounded)
        {
            IsWallJumping = false;
        }
    }

    //wall Jump02

    private float jumpForce = 10f; // Force de saut vertical lors du wall jump
    private float wallJumpHorizontalForce = 50f; // Force horizontale lors du wall jump

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("wall"))
    //     // && Input.GetButtonDown("Jump"))
    //     {
    //         Debug.Log("contact");
    //         RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 3f, wallLayer);
    //         RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 3f, wallLayer);
    //         Debug.Log("hit calculé");

    //         if (hitRight.collider != null && hitLeft.collider == null)
    //         {
    //             Debug.Log("contact droit");
    //             WallJump(Vector2.left);
    //             Debug.Log("Wall jump vers la gauche");
    //         }
    //         else if (hitLeft.collider != null && hitRight.collider == null)
    //         {
    //             Debug.Log("contact gauche");
    //             WallJump(Vector2.right);
    //             Debug.Log("Wall jump vers la droite");
    //         }
    //     }
    // }

    // void WallJump(Vector2 direction)
    // {
    //     _rb.velocity = Vector2.zero; // Réinitialise la vélocité
    //     _rb.AddForce(direction * wallJumpHorizontalForce, ForceMode2D.Impulse); // Ajoute la force horizontale
    //     _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Ajoute la force verticale
    // }

    //Jump Buffer
    private void _ResetJumpBuffer()
    {
        _jumpBufferTimer = 0f;
    }

    private bool IsJumpBufferActive()
    {
        return _jumpBufferTimer < _jumpBufferDuration;
    }

    private void _CancelJumpBuffer()
    {
        _jumpBufferTimer = _jumpBufferDuration;
    }

    private void _UpdateJumpBuffer()
    {
        if (!IsJumpBufferActive()) return;
        _jumpBufferTimer += Time.deltaTime;
    }




    //Anim
    void flipCheck()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _skin.flipX = true;
            slash.transform.localPosition = new Vector3(-0.352f, -0.06f, transform.localPosition.z);
            slash.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _skin.flipX = false;
            slash.transform.localPosition = new Vector3(0.352f, -0.06f, transform.localPosition.z);
            slash.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void animCheck()
    {
        anim.SetFloat("velocityX", Mathf.Abs(_rb.velocity.x));
        anim.SetFloat("velocityY", _rb.velocity.y);
        anim.SetBool("grounded", grounded);
        //Attack
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
            SetUpAudio("slash", true);
            slash.GetComponent<Animator>().SetTrigger("slash");
        }
    }

    private void OnDrawGizmos()
    {
        if (_monColl == null)
        {
            _monColl = GetComponent<CapsuleCollider2D>();
        }
        Gizmos.color = Color.red;

        Vector3 offset = Vector3.right * deccalageGroundcheckX;
        Vector3 circlePosition = transform.position + Vector3.up * deccalageGroundcheck + offset;

        Gizmos.DrawWireSphere(circlePosition, _monColl.bounds.extents.x * 0.5f);
    }

    [Header("Debug")]
    [SerializeField] private bool _guiDebug = false;


    private void OnGUI()
    {
        if (!_guiDebug) return;

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(gameObject.name);
        GUILayout.Label($"Jump Buffer Timer = {_jumpBufferTimer}");
        GUILayout.Label($"IsSliding = {IsSliding}");
        GUILayout.Label($"grounded = {grounded}");
        GUILayout.Label($"Horizontal Velocity = {_rb.velocity.x}");
        GUILayout.Label($"Vertical Velocity = {_rb.velocity.y}");
        GUILayout.Label($"IsTouchingWallRight = {IsTouchingWallRight}");
        GUILayout.Label($"IsTouchingWallLeft = {IsTouchingWallLeft}");
        GUILayout.Label($"orientX = {_orientX}");
        GUILayout.EndVertical();
    }
}
