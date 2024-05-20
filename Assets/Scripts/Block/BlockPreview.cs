using UnityEngine;

public class BlockPreview : MonoBehaviour
{
    [SerializeField] private BlockList blockList;
    private GameObject[] previewBlocks = new GameObject[5];
    private GameObject holdBlockImage;

    private float zValue = 5.0f;
    private void OnEnable()
    {
        holdBlockImage = new GameObject("HoldBlock");
        holdBlockImage.AddComponent<SpriteRenderer>();
        holdBlockImage.transform.parent = transform;
        holdBlockImage.transform.localPosition = transform.position + new Vector3(-10.0f, 10.5f, zValue);

        for (int i = 0; i < previewBlocks.Length; i++) 
        {
            previewBlocks[i] = new GameObject($"PreviewBlock[{i}]");
            previewBlocks[i].AddComponent<SpriteRenderer>();
            previewBlocks[i].transform.parent = transform;
            previewBlocks[i].transform.localPosition = transform.position + new Vector3(9.0f, 10.5f - (i * 3.0f), zValue);
        }
    }

    public void UpdateBlocksSprite()
    {
        holdBlockImage.GetComponent<SpriteRenderer>().sprite = BlockSprites.GetBlockCombinedSprite(blockList.HoldingBlock);
        for (int i = 0; i < previewBlocks.Length; i++)
        {
            previewBlocks[i].GetComponent<SpriteRenderer>().sprite = BlockSprites.GetBlockCombinedSprite(blockList.CurrentBlockList[i]);
        }
    }

}
