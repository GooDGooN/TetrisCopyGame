using UnityEngine;

class BlockData
{    
    public static int[,] GetBlockMatrix(BlockEnums.BlockType type)
    {
        int[,] blockMatrix = null;
        switch (type)
        {
            case BlockEnums.BlockType.I:
                blockMatrix = new int[1,4]
                {
                    {1,1,1,1, },
                };
                break;
            case BlockEnums.BlockType.J:
                blockMatrix = new int[2, 3]
                {
                    {1,0,0, },
                    {1,1,1, },
                };
                break;
            case BlockEnums.BlockType.L:
                blockMatrix = new int[2, 3]
                {
                    {0,0,1, },
                    {1,1,1, },
                };
                break;
            case BlockEnums.BlockType.O:
                blockMatrix = new int[2, 2]
                {
                    {1,1, },
                    {1,1, },
                };
                break;
            case BlockEnums.BlockType.S:
                blockMatrix = new int[2, 3]
                {
                    {0,1,1, },
                    {1,1,0, },
                };
                break;
            case BlockEnums.BlockType.T:
                blockMatrix = new int[2, 3]
                {
                    {0,1,0, },
                    {1,1,1, },
                };
                break;
            case BlockEnums.BlockType.Z:
                blockMatrix = new int[2, 3]
                {
                    {1,1,0, },
                    {0,1,1, },
                };
                break;
        }
        return blockMatrix;
    }


    /// <summary>
    /// Return additional vector3 to check the block position when block is rotated.
    /// </summary>
    public static Vector3 GetKickPosition(int testNumber, BlockEnums.BlockType blockType, BlockEnums.BlockDirection nowDir, BlockEnums.BlockRotateType rotateType)
    {
        var isFlip = (rotateType != BlockEnums.BlockRotateType.TurnRight && rotateType != BlockEnums.BlockRotateType.TurnLeft);
        if (blockType == BlockEnums.BlockType.I)
        {
            return isFlip ? WallKickFlipVector_I[(int)nowDir, testNumber] : WallKickRotateVector_I[(int)rotateType, (int)nowDir, testNumber];
        }
        else
        {
            return isFlip ? WallKickFlipVector_JLSTZ[(int)nowDir, testNumber] : WallKickRotateVector_JLSTZ[(int)rotateType, (int)nowDir, testNumber];
        }
    }

    /// <summary>
    /// [CWorCCW, RotateType, testnum] - [0]:0, [1]:R, [2]:2, [3]:L
    /// </summary>
    public static Vector3[,,] WallKickRotateVector_JLSTZ =
    {
        //CW
        {
            // 0->R
            { Vector3.zero, new Vector3(-1.0f, 0.0f), new Vector3(-1.0f, 1.0f), new Vector3(0.0f, -2.0f), new Vector3(-1.0f, -2.0f)},
            // R->2
            { Vector3.zero, new Vector3(1.0f, 0.0f), new Vector3(1.0f, -1.0f), new Vector3(0.0f, 2.0f), new Vector3(1.0f, 2.0f)},
            // 2->L
            { Vector3.zero, new Vector3(1.0f, 0.0f), new Vector3(1.0f, 1.0f), new Vector3(0.0f, -2.0f), new Vector3(1.0f, -2.0f)},
            // L->0
            { Vector3.zero, new Vector3(-1.0f, 0.0f), new Vector3(-1.0f, -1.0f), new Vector3(0.0f, 2.0f), new Vector3(-1.0f, 2.0f)},
        },
        //CCW
        {
            // 0->L
            { Vector3.zero, new Vector3(1.0f, 0.0f), new Vector3(1.0f, 1.0f), new Vector3(0.0f, -2.0f), new Vector3(1.0f, -2.0f)},
            // R->0
            { Vector3.zero, new Vector3(1.0f, 0.0f), new Vector3(1.0f, -1.0f), new Vector3(0.0f, 2.0f), new Vector3(1.0f, 2.0f)},
            // 2->R
            { Vector3.zero, new Vector3(-1.0f, 0.0f), new Vector3(-1.0f, 1.0f), new Vector3(0.0f, -2.0f), new Vector3(-1.0f, -2.0f)},
            // L->2
            { Vector3.zero, new Vector3(-1.0f, 0.0f), new Vector3(-1.0f, -1.0f), new Vector3(0.0f, 2.0f), new Vector3(-1.0f, 2.0f)},
        },
    };

    /// <summary>
    /// [type, testnum] - [0]:0, [1]:R, [2]:2, [3]:L
    /// </summary>
    public static Vector3[,] WallKickFlipVector_JLSTZ =
    {
        // 0->2
        { Vector3.zero, new Vector3(0.0f, 1.0f), new Vector3(1.0f, 1.0f), new Vector3(-1.0f, 1.0f), new Vector3(1.0f, 0.0f), new Vector3(-1.0f, 0.0f), },
        // R->L
        { Vector3.zero, new Vector3(1.0f, 0.0f), new Vector3(1.0f, 2.0f), new Vector3(1.0f, 1.0f), new Vector3(0.0f, 2.0f), new Vector3(0.0f, 1.0f), },
        // 2->0
        { Vector3.zero, new Vector3(0.0f, -1.0f), new Vector3(-1.0f, -1.0f), new Vector3(1.0f, -1.0f), new Vector3(-1.0f, 0.0f), new Vector3(1.0f, 0.0f), },
        // L->R
        { Vector3.zero, new Vector3(-1.0f, 0.0f), new Vector3(-1.0f, 2.0f), new Vector3(-1.0f, 1.0f), new Vector3(0.0f, 2.0f), new Vector3(0.0f, 1.0f), },
    };

    /// <summary>
    /// [CWorCCW, RotateType, testnum] - [0]:0, [1]:R, [2]:2, [3]:L
    /// </summary>
    public static Vector3[,,] WallKickRotateVector_I =
    {
        // CW
        {
            // 0->R
            { Vector3.zero, new Vector3(-2.0f, 0.0f), new Vector3(1.0f, 0.0f), new Vector3(-2.0f, -1.0f), new Vector3(1.0f, 2.0f)},
            // R->2
            { Vector3.zero, new Vector3(-1.0f, 0.0f), new Vector3(2.0f, 0.0f), new Vector3(-1.0f, 2.0f), new Vector3(2.0f, -1.0f)},
            // 2->L
            { Vector3.zero, new Vector3(2.0f, 0.0f), new Vector3(-1.0f, 0.0f), new Vector3(2.0f, 1.0f), new Vector3(-1.0f, -2.0f)},
            // L->0
            { Vector3.zero, new Vector3(1.0f, 0.0f), new Vector3(-2.0f, 0.0f), new Vector3(1.0f, -2.0f), new Vector3(-2.0f, 1.0f)},
        },
        // CCW
        {
            // 0->L
            { Vector3.zero, new Vector3(-1.0f, 0.0f), new Vector3(2.0f, 0.0f), new Vector3(-1.0f, 2.0f), new Vector3(2.0f, -1.0f)},
            // R->0
            { Vector3.zero, new Vector3(2.0f, 0.0f), new Vector3(-1.0f, 0.0f), new Vector3(2.0f, 1.0f), new Vector3(-1.0f, -2.0f)},
            // 2->R
            { Vector3.zero, new Vector3(1.0f, 0.0f), new Vector3(-2.0f, 0.0f), new Vector3(1.0f, -2.0f), new Vector3(-2.0f, 1.0f)},
            // L->2
            { Vector3.zero, new Vector3(-2.0f, 0.0f), new Vector3(1.0f, 0.0f), new Vector3(-2.0f, -1.0f), new Vector3(1.0f, 2.0f)},
        },
    };
    

    /// <summary>
    /// [type, testnum] - [0]:0, [1]:R, [2]:2, [3]:L
    /// </summary>
    public static Vector3[,] WallKickFlipVector_I =
    {
        // 0->2
        { Vector3.zero, new Vector3(0.0f, 1.0f), },
        // R->L
        { Vector3.zero, new Vector3(1.0f, 0.0f), },
        // 2->0
        { Vector3.zero, new Vector3(0.0f, -1.0f), },
        // L->R
        { Vector3.zero, new Vector3(-1.0f, 0.0f), },
    };
}

