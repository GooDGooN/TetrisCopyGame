using UnityEngine;

public class Block : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetBlock(Vector3 position, BlockEnums.BlockType type)
    {
        transform.localPosition = position;
        spriteRenderer.sprite = BlockSprites.GetSingleBlockSprite(type);
    }
}
