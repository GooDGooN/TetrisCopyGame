using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockList : MonoBehaviour
{
    private List<BlockEnums.BlockType> blockBag = new List<BlockEnums.BlockType>();
    private List<BlockEnums.BlockType> currentBlockList = new List<BlockEnums.BlockType>();
    public List<BlockEnums.BlockType> CurrentBlockList { get => currentBlockList; }
    private BlockEnums.BlockType holdingBlock = BlockEnums.BlockType.None;
    public BlockEnums.BlockType HoldingBlock { get => holdingBlock; }

    private void Awake()
    {
        RefillBag();
        while (currentBlockList.Count < 5)
        {
            currentBlockList.Add(PopBlockFromBag());
        }
    }

    public BlockEnums.BlockType PopBlockFromList()
    {
        var block = currentBlockList[0];
        currentBlockList.RemoveAt(0);
        while (currentBlockList.Count < 5)
        {
            currentBlockList.Add(PopBlockFromBag());
        }
        return block;
    }

    public BlockEnums.BlockType SwapBlockFromHold(BlockEnums.BlockType targetBlock)
    {
        var holdBlock = holdingBlock;
        holdingBlock = targetBlock;
        return holdBlock != BlockEnums.BlockType.None ? holdBlock : PopBlockFromList();
    }

    private BlockEnums.BlockType PopBlockFromBag()
    {
        var result = blockBag[Random.Range(0, blockBag.Count)];
        blockBag.Remove(result);
        if (blockBag.Count == 0)
        {
            RefillBag();
        }
        return result;
    }

    private void RefillBag()
    {
        for (int i = 0; i < 7; i++)
        {
            blockBag.Add((BlockEnums.BlockType)i);
        }
    }
}
