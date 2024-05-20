using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockStackManager : MonoBehaviour
{
    private List<GameObject[]> stackList = new List<GameObject[]>();
    private GameObject[] stackBlockObjectPool;
    private int fieldWidth = (int)GlobalEnums.MapScale.MapWidth;
    private int fieldHeight = (int)GlobalEnums.MapScale.MapHeight;
    private int fieldMaxHeight = (int)GlobalEnums.MapScale.MapHeight + 4;
    public int listLength { get => stackList.Count; }

    private void Awake()
    {
        for(int i = 0; i < fieldMaxHeight; i++)
        {
            stackList.Add(new GameObject[fieldWidth]);
        }
        stackBlockObjectPool = new GameObject[fieldWidth * (fieldMaxHeight)];
        for(int i = 0; i < stackBlockObjectPool.Length; i++)
        {
            stackBlockObjectPool[i] = new GameObject($"stackBlock[{i}]");
            stackBlockObjectPool[i].AddComponent<SpriteRenderer>();
            stackBlockObjectPool[i].transform.parent = transform;
            stackBlockObjectPool[i].SetActive(false);
        }
    }

    private GameObject PopStackBlockObjectFromPool()
    { 
        for (int i = 0; i < stackBlockObjectPool.Length; i++)
        {
            if (!stackBlockObjectPool[i].activeSelf)
            {
                stackBlockObjectPool[i].SetActive(true);
                return stackBlockObjectPool[i];
            }
        }
        return null;
    }

    private void PushStackBlockObjectToPool(GameObject stackBlock)
    {
        stackBlock.SetActive(false);
    }

    public void AddBlock(Vector3 globalBlockPos, BlockEnums.BlockType blockType)
    {
        var listIndex = (int)(globalBlockPos.y + fieldHeight * 0.5f);
        var gameObjIndex = (int)(globalBlockPos.x + fieldWidth * 0.5f);

        var obj = stackList[listIndex][gameObjIndex] = PopStackBlockObjectFromPool();
        obj.transform.localPosition = globalBlockPos;
        obj.GetComponent<SpriteRenderer>().sprite = BlockSprites.GetSingleBlockSprite(blockType);
    }


    public bool IsBlockCanMoveToPosition(Vector3 globalBlockPos) => stackList[(int)(globalBlockPos.y + fieldHeight * 0.5f)][(int)(globalBlockPos.x + fieldWidth * 0.5f)] != null;

    public int GetRemoveBlockAmountAndReposition()
    {
        var lineCount = 0;
        // RemoveLine
        for(int listIndex = 0; listIndex < fieldMaxHeight; listIndex++)
        {
            if (stackList[listIndex].Count(i => (i == null)) == 0)
            {
                for (int i = 0; i < fieldWidth; i++)
                {
                    PushStackBlockObjectToPool(stackList[listIndex][i]);
                }
                lineCount++;
                stackList.RemoveAt(listIndex--);
                stackList.Add(new GameObject[fieldWidth]);
            }
        }

        // Reposition
        for (int yi = 0; yi < fieldMaxHeight; yi++)
        {
            for (int xi = 0; xi < fieldWidth; xi++)
            {
                if (stackList[yi][xi] != null)
                {
                    stackList[yi][xi].transform.position = new Vector3(xi - fieldWidth * 0.5f, yi - fieldHeight * 0.5f);
                }
            }
        }
        return lineCount;
    }

    public bool IsLineClear(int line) => (stackList[line].Count(obj => obj != null) > 0) ? false : true;
}
