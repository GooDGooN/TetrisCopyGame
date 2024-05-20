using UnityEngine;
using UnityEngine.SceneManagement;
using static GlobalEnums;

public class BlockParent : MonoBehaviour
{
    [SerializeField] private BlockStackManager blockStackManager;
    [SerializeField] private BlockList blockList;
    [SerializeField] private BlockPreview blockPreview;

    private BlockEnums.BlockType myBlockType;
    private Vector3 resetPosition;
    private Block[] myBlocks = new Block[4];
    private GameObject[] myGuideBlocks = new GameObject[4];
    private GameObject[] myWarningBlocks = new GameObject[4];
    private int blockDirectionNum;

    private float fallDelay = 0;
    private float fallDelaySpeed = 0;
    private float fallDelayLimit = 0.45f;
    private float fallDelayResetValue = 0.45f;
    private float moveDelay = 0;
    private float moveDelayLimit = 0.25f;
    private bool isUseHold = false;
    private int remainDrop = 0;


    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            myBlocks[i] = Instantiate(Resources.Load<Block>("Prefabs/Block"), transform);
            myBlocks[i].name = $"Block[{i}]";

            myGuideBlocks[i] = new GameObject($"GuideBlock[{i}]");
            myGuideBlocks[i].transform.parent = transform;
            myGuideBlocks[i].AddComponent<SpriteRenderer>();

            myWarningBlocks[i] = new GameObject($"WarningBlock[{i}]");
            myWarningBlocks[i].transform.parent = transform.parent;
            myWarningBlocks[i].AddComponent<SpriteRenderer>();

        }
    }

    private void Start()
    {
        resetPosition = transform.parent.position + new Vector3(-1.0f, ((float)MapScale.MapHeight / 2.0f) - 3.0f);
        blockDirectionNum = 0;
        myBlockType = blockList.PopBlockFromList();
        ResetBlock(myBlockType);
        for (int i = 0; i < 4; i++)
        {
            myGuideBlocks[i].GetComponent<SpriteRenderer>().sprite = BlockSprites.GetSingleBlockSprite(myBlockType);
            myGuideBlocks[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            myWarningBlocks[i].GetComponent<SpriteRenderer>().sprite = BlockSprites.GetSingleBlockSprite(BlockEnums.BlockType.Warning);
            myWarningBlocks[i].GetComponent<SpriteRenderer>().color = Color.clear;
        }
        fallDelay = 1.0f;
    }

    private void LateUpdate()
    {
        UpdateGuideBlock();
    }

    private void Update() 
    {
        fallDelayResetValue = 0.45f - (0.02f * (GameManager.Instance.GameLevel - 1));
        BlockFalling();
        //restart for test
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    #region PublicMethod
    public void RotateBlocks(BlockEnums.BlockRotateType rotateType)
    {
        if(GetBlockDropPosition() == Vector3.zero)
        {
            fallDelay = fallDelayLimit * 2.0f;
            fallDelayLimit = fallDelayLimit * 0.75f;
        }
        var maxTestNum = 0;
        Vector3[] testBlockPos = new Vector3[4];
        for (int i = 0; i < myBlocks.Length; i++)
        {
            if (rotateType == BlockEnums.BlockRotateType.TurnRight)
            {
                testBlockPos[i] = new Vector3(myBlocks[i].transform.localPosition.y, -myBlocks[i].transform.localPosition.x);
            }
            else if (rotateType == BlockEnums.BlockRotateType.TurnLeft)
            {
                testBlockPos[i] = new Vector3(-myBlocks[i].transform.localPosition.y, myBlocks[i].transform.localPosition.x);
            }
            else
            {
                testBlockPos[i] = new Vector3(-myBlocks[i].transform.localPosition.x, -myBlocks[i].transform.localPosition.y);
            }
        }
        switch (rotateType)
        {
            case BlockEnums.BlockRotateType.TurnRight:
            case BlockEnums.BlockRotateType.TurnLeft:
                if(myBlockType == BlockEnums.BlockType.I)
                {
                    maxTestNum = BlockData.WallKickRotateVector_I.GetLength(2);
                }
                else
                {
                    maxTestNum = BlockData.WallKickRotateVector_JLSTZ.GetLength(2);
                }
                break;
            case BlockEnums.BlockRotateType.Flip:
                if (myBlockType == BlockEnums.BlockType.I) 
                {
                    maxTestNum = BlockData.WallKickFlipVector_I.GetLength(1); 
                }
                else 
                { 
                    maxTestNum = BlockData.WallKickFlipVector_JLSTZ.GetLength(1); 
                }
                break;
            default: Debug.LogError("ERROR_RotateBlocks : rotateType not match"); 
                break;
        }
        var testPos = Vector3.zero;
        var isSkip = true;
        for (int testNum = 0; testNum < maxTestNum; testNum++)
        {
            isSkip = true;
            for (int i = 0; i < myBlocks.Length; i++)
            {
                testPos = BlockData.GetKickPosition(testNum, myBlockType, (BlockEnums.BlockDirection)blockDirectionNum, rotateType);
                if (!IsCanPlaceBlock(testPos + testBlockPos[i], true))
                {
                    isSkip = false;
                    break;
                }
            }
            if(isSkip) 
            {
                transform.position += testPos;
                for (int i = 0; i < myBlocks.Length; i++)
                {
                    myBlocks[i].SetBlock(testBlockPos[i], myBlockType);
                }
                if(rotateType == BlockEnums.BlockRotateType.TurnRight)
                {
                    blockDirectionNum += 1;
                }
                else if (rotateType == BlockEnums.BlockRotateType.TurnLeft)
                {
                    blockDirectionNum -= 1;
                }
                else
                {
                    blockDirectionNum += 2;
                }

                if (blockDirectionNum > 3)
                {
                    blockDirectionNum = blockDirectionNum - 4;
                }
                else if (blockDirectionNum < 0)
                {
                    blockDirectionNum = blockDirectionNum + 4;
                }
                if (GetScoreType() != ScoreType.Normal)
                {
                    Debug.Log(GetScoreType());
                }

                return; 
            }
        }
    }

    public void MoveBlocks(Vector3 dir, bool isPressed)
    {
        if (isPressed) 
        {
            if (IsCanPlaceBlock(dir, false))
            {
                moveDelay = moveDelayLimit;
                transform.position += dir;
            }
        }
        else
        {
            if(moveDelay < 0)
            {
                if (IsCanPlaceBlock(dir, false))
                {
                    transform.position += dir;
                }
                moveDelay = moveDelayLimit * 0.15f;
            }
        }
    }

    public void DropBlocks(bool isHardDrop)
    {
        if(isHardDrop)
        {
            var dropPosition = GetBlockDropPosition();
            remainDrop = (int)Mathf.Abs(dropPosition.y);
            GameManager.Instance.AddScore(remainDrop * 2);
            transform.position += dropPosition;
            fallDelay = 0;
        }
        else
        {
            if (GetBlockDropPosition() != Vector3.zero)
            {
                fallDelaySpeed = Time.deltaTime * 20.0f;
            }
        }
    }

    public void DecreaseMoveDelay() => moveDelay -= Time.deltaTime;

    public void HoldBlocks()
    {
        if(!isUseHold)
        {
            isUseHold = true;
            ResetBlock(blockList.SwapBlockFromHold(myBlockType));
        }
    }
    #endregion PublicMethod

    #region PrivateMethod
    private void UpdateGuideBlock()
    {
        for (int i = 0; i < myGuideBlocks.Length; i++)
        {
            myGuideBlocks[i].transform.localPosition = GetBlockDropPosition() + myBlocks[i].transform.localPosition;
            myGuideBlocks[i].GetComponent<SpriteRenderer>().sprite = BlockSprites.GetSingleBlockSprite(myBlockType);
        }
    }

    private void SetBlocks(BlockEnums.BlockType type)
    {
        if (myBlockType == BlockEnums.BlockType.I || myBlockType == BlockEnums.BlockType.O)
        {
            transform.position += new Vector3(1.0f, 1.0f) * 0.5f;
        }
        for (int i = 0; i < 4; i++)
        {
            myBlocks[i].SetBlock(GetBlockPiecePositions(type)[i], type);
            myGuideBlocks[i].GetComponent<SpriteRenderer>().sprite = BlockSprites.GetSingleBlockSprite(myBlockType);

            myWarningBlocks[i].transform.localPosition = resetPosition + GetBlockPiecePositions(blockList.CurrentBlockList[0])[i];
            if (blockList.CurrentBlockList[0] == BlockEnums.BlockType.I || blockList.CurrentBlockList[0] == BlockEnums.BlockType.O)
            {
                myWarningBlocks[i].transform.localPosition += new Vector3(1.0f, 1.0f) * 0.5f;
            }
        }
    }

    private Vector3[] GetBlockPiecePositions(BlockEnums.BlockType type)
    {
        Vector3[] result = new Vector3[4];
        var vectorLength = new Vector3(BlockData.GetBlockMatrix(type).GetLength(1), BlockData.GetBlockMatrix(type).GetLength(0));
        int blockindex = 0;
        for (int iy = 0; iy < vectorLength.y; iy++)
        {
            for (int ix = 0; ix < vectorLength.x; ix++)
            {
                if (BlockData.GetBlockMatrix(type)[iy, ix] == 1)
                {
                    var vecterDelta = new Vector3((vectorLength.x - 1.0f) * 0.5f, (Mathf.Abs(vectorLength.x - 3.0f) * 0.5f));
                    result[blockindex++] = new Vector3(ix, -iy + 1.0f) - (vecterDelta);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Will stack the block when hit bottom
    /// </summary>
    private void BlockFalling()
    {

        if (fallDelay < 0)
        {
            fallDelay = fallDelayLimit;
            if (IsCanPlaceBlock(Vector3.down, false))
            {
                transform.position += Vector3.down;
                if(GetBlockDropPosition() == Vector3.zero)
                {
                    fallDelay = fallDelayLimit * 1.5f; 
                }
            }
            else
            {
                StackBlocks();
                ResetBlock(blockList.PopBlockFromList());
                isUseHold = false;
                fallDelayLimit = fallDelayResetValue;
            }
        }
        else
        {
            fallDelay -= fallDelaySpeed;
        }
        fallDelaySpeed = Time.deltaTime;
    }

    private void ResetBlock(BlockEnums.BlockType targetType)
    {
        transform.position = resetPosition;
        blockDirectionNum = 0;
        myBlockType = targetType;
        SetBlocks(myBlockType);
        fallDelay = 0.1f;
        blockPreview.UpdateBlocksSprite();
    }

    private Vector3 GetBlockDropPosition()
    {
        var deltaVector = Vector3.down;
        while(true)
        {
            if (!IsCanPlaceBlock(deltaVector, false))
            {
                break;
            }
            deltaVector += Vector3.down;
        }
        return deltaVector - Vector3.down;
    }

    private void StackBlocks()
    {
        foreach(var block in myBlocks)
        {
            var pos = new Vector3Int(Mathf.FloorToInt(block.transform.position.x), Mathf.FloorToInt(block.transform.position.y));
            blockStackManager.AddBlock(pos, myBlockType);
        }
        var removedLineCount = blockStackManager.GetRemoveBlockAmountAndReposition();
        remainDrop = 0;

        // check gameover
        for (int blocki = 0; blocki < myWarningBlocks.Length; blocki++)
        {
            if (myBlocks[blocki].transform.position == myWarningBlocks[blocki].transform.position)
            {
                GameManager.Instance.GameOver();
            }
        }

        // show warningblock or not
        for (int i = 17; i < blockStackManager.listLength; i++)
        {
            if(!blockStackManager.IsLineClear(i))
            {
                for (int wblocki = 0; wblocki < myWarningBlocks.Length; wblocki++)
                {
                    myWarningBlocks[wblocki].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.25f);
                }
                return;
            }
        }

        for(int wblocki = 0; wblocki < myWarningBlocks.Length; wblocki++)
        {
            myWarningBlocks[wblocki].GetComponent<SpriteRenderer>().color = Color.clear;
        }

        GameManager.Instance.AddScore(GetScoreType(), removedLineCount);
        GameManager.Instance.StackTotalRemovedLine(removedLineCount);

    }

    private bool IsCanPlaceBlock(Vector3 testPosition, bool isBlockParentCheck)
    {
        bool result = true;
        for (int i = 0; i < myBlocks.Length; i++)
        {
            var pos = (isBlockParentCheck) ? transform.position + testPosition : (myBlocks[i].transform.position + testPosition);
            var witdhLimit = (int)MapScale.MapWidth / 2;
            var heightLimit = -(((int)MapScale.MapHeight / 2) - 2);
            if (pos.x < -witdhLimit || pos.x > witdhLimit - 1)
            {
                return false;
            }
            if (blockStackManager.IsBlockCanMoveToPosition(new Vector3(pos.x, pos.y)))
            {
                return false;
            }
            if (pos.y < heightLimit)
            {
                return false;
            }
            if(isBlockParentCheck)
            {
                break;
            }
        }
        return result;
    }

    private GlobalEnums.ScoreType GetScoreType()
    {
        if(myBlockType == BlockEnums.BlockType.T && GetBlockDropPosition() == Vector3.zero)
        {
            int sum = 0;
            int count = 0;
            int pownum = 0;
            for (int xx = -1; xx <= 1; xx += 2)
            {
                for (int yy = -1; yy <= 1; yy += 2)
                {
                    if (!IsCanPlaceBlock(new Vector3(xx, yy), true))
                    {
                        sum += (int)Mathf.Pow(2, pownum);
                        count++;
                    }
                    pownum++;
                }   
            }
            if(count >= 3)
            {
                switch(blockDirectionNum)
                {
                    case 0:
                        return (sum ==  11 || sum == 14) ? 
                            ScoreType.Tspin : ScoreType.MiniTspin;
                    case 1:
                        return (sum == 14 || sum == 13) ?
                            ScoreType.Tspin : ScoreType.MiniTspin;
                    case 2:
                        return (sum == 13 || sum == 7) ?
                            ScoreType.Tspin : ScoreType.MiniTspin;
                    case 3:
                        return (sum == 7 || sum == 11) ?
                            ScoreType.Tspin : ScoreType.MiniTspin;
                }
            }
        }
        return ScoreType.Normal;
    }

    #endregion PrivateMethod
}

