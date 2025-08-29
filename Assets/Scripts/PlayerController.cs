using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    public UIDocument uiDocument; // arraste o PlayerUI aqui no Inspector

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDead = false;

    private Label timeLabel;
    private float elapsedTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (uiDocument != null)
        {
            timeLabel = uiDocument.rootVisualElement.Q<Label>("TimeLabel");
        }
    }

    void Update()
    {
        CheckGround();

        if (!isDead && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // corrigido de linearVelocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Atualiza o tempo apenas se o jogador não morreu
        if (!isDead && timeLabel != null)
        {
            elapsedTime += Time.deltaTime;
            timeLabel.text = "Tempo: " + Mathf.FloorToInt(elapsedTime).ToString();
        }

        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void CheckGround()
    {
        int mask = (groundLayer.value == 0) ? ~0 : groundLayer.value;
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, mask);

        if (hit != null)
        {
            if (groundLayer.value == 0)
                isGrounded = hit.CompareTag("Ground");
            else
                isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Spike") && !isDead)
        {
            isDead = true;
            Time.timeScale = 0f;

            // Mensagem de restart
            if (timeLabel != null)
                timeLabel.text += "\nVocê morreu! Pressione R para reiniciar.";
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
