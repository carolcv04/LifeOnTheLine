using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool scrollLeft;

    private float singleSpriteWidth;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        SetUpSprite();
        initialPosition = transform.position;
        if (scrollLeft) moveSpeed = -Mathf.Abs(moveSpeed);
        else moveSpeed = Mathf.Abs(moveSpeed);
    }

    private void SetUpSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            singleSpriteWidth = spriteRenderer.sprite.bounds.size.x;
        }
        else
        {
            Debug.LogError("Sprite or SpriteRenderer component is missing!");
        }
    }

    private void Scroll()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void CheckReset()
    {
        if (scrollLeft && transform.position.x < initialPosition.x - singleSpriteWidth)
        {
            transform.position += new Vector3(singleSpriteWidth * 2, 0f, 0f);
        }
        else if (!scrollLeft && transform.position.x > initialPosition.x + singleSpriteWidth)
        {
            transform.position -= new Vector3(singleSpriteWidth * 2, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
        CheckReset();
    }
}
