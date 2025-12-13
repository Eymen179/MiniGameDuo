using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SubmarineController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 10f;
    public float turnSpeed = 10f;

    [Header("Environment Limits")]
    public float surfaceLevel = 52.5f;

    //Denizalti Icerikleri
    private Rigidbody rb;
    private Vector3 movementInput;
    private Animator animator;

    //Hareket Input
    [SerializeField] private InputActionReference movement;
    private void OnEnable()
    {
        if (movement != null) movement.action.Enable();
    }

    private void OnDisable()
    {
        if (movement != null) movement.action.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //---------Hareket Yon, Animasyon ve Aci Ayarlari-------------
        //Yon
        Vector2 inputVector = movement.action.ReadValue<Vector2>();

        float moveX = inputVector.x;
        float moveY = inputVector.y;

        movementInput = new Vector3(moveX, moveY, 0f).normalized;

        //Animasyon
        if (animator != null)
        {
            bool isMoving = movementInput.sqrMagnitude > 0.01f;

            animator.SetBool("isMoving", isMoving);
        }

        //Aci
        if (moveX != 0)
        {
            float targetAngle = (moveX > 0) ? 90f : 270f;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        //Hareketi Uygulama Komutu
        rb.velocity = movementInput * moveSpeed;

        //Yükseklik Sinirlamasi
        if (transform.position.y > surfaceLevel)
        {
            Vector3 clampedPosition = transform.position;
            clampedPosition.y = surfaceLevel;
            transform.position = clampedPosition;

            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }
        }
    }

    //Denizalti Acma - Kapama Metodu
    public void SetSubmarineState(bool state)
    {
        this.enabled = state;

        /*if (state) movement.action.Enable();
        else movement.action.Disable();*/
    }
}