using UnityEngine;

public class BlockSprites : MonoBehaviour
{
    public static Sprite GetSingleBlockSprite(BlockEnums.BlockType type)
    {
        switch(type)
        {
            case BlockEnums.BlockType.I:
                return Resources.Load<Sprite>("Sprites/Single/Block_I_Single");
            case BlockEnums.BlockType.J:
                return Resources.Load<Sprite>("Sprites/Single/Block_J_Single");
            case BlockEnums.BlockType.L:
                return Resources.Load<Sprite>("Sprites/Single/Block_L_Single");
            case BlockEnums.BlockType.O:
                return Resources.Load<Sprite>("Sprites/Single/Block_O_Single");
            case BlockEnums.BlockType.S:
                return Resources.Load<Sprite>("Sprites/Single/Block_S_Single");
            case BlockEnums.BlockType.T:
                return Resources.Load<Sprite>("Sprites/Single/Block_T_Single");
            case BlockEnums.BlockType.Z:
                return Resources.Load<Sprite>("Sprites/Single/Block_Z_Single");
            case BlockEnums.BlockType.Warning:
                return Resources.Load<Sprite>("Sprites/Single/Block_Warning_Single");
            default:    
                return null;
        }
    }
    public static Sprite GetBlockCombinedSprite(BlockEnums.BlockType type)
    {
        switch (type)
        {
            case BlockEnums.BlockType.I:
                return Resources.Load<Sprite>("Sprites/Combined/Block_I");
            case BlockEnums.BlockType.J:
                return Resources.Load<Sprite>("Sprites/Combined/Block_J");
            case BlockEnums.BlockType.L:
                return Resources.Load<Sprite>("Sprites/Combined/Block_L");
            case BlockEnums.BlockType.O:
                return Resources.Load<Sprite>("Sprites/Combined/Block_O");
            case BlockEnums.BlockType.S:
                return Resources.Load<Sprite>("Sprites/Combined/Block_S");
            case BlockEnums.BlockType.T:
                return Resources.Load<Sprite>("Sprites/Combined/Block_T");
            case BlockEnums.BlockType.Z:
                return Resources.Load<Sprite>("Sprites/Combined/Block_Z");
            default:
                return null;
        }
    }
}
