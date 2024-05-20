using UnityEngine;
using static GlobalEnums;

public class GameKeyInput : MonoBehaviour
{
    [SerializeField] private BlockParent blockParent;

    public static bool GetKeyPressed(InputKeys key)
    {
        switch (key)
        {
            case InputKeys.MoveRight:
                if (Input.GetKeyDown(KeyCode.RightArrow)) { return true; }
                break;
            case InputKeys.MoveLeft:
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { return true; }
                break;
            case InputKeys.RotateRight:
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.X)) { return true; }
                break;
            case InputKeys.RotateLeft:
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z)) { return true; }
                break;
            case InputKeys.Flip:
                if (Input.GetKeyDown(KeyCode.A)) { return true; }
                break;
            case InputKeys.SoftDrop:
                if (Input.GetKeyDown(KeyCode.DownArrow)) { return true; }
                break;
            case InputKeys.HardDrop:
                if (Input.GetKeyDown(KeyCode.Space)) { return true; }
                break;
            case InputKeys.Hold:
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.C)) { return true; }
                break;
        }
        return false;
    }
    public static bool GetKey(InputKeys key)
    {
        switch(key)
        {
            case InputKeys.MoveRight:
                if (Input.GetKey(KeyCode.RightArrow)) { return true; } 
                break;
            case InputKeys.MoveLeft:
                if (Input.GetKey(KeyCode.LeftArrow)) { return true; }
                break;
            case InputKeys.RotateRight:
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.X)) { return true; }
                break;
            case InputKeys.RotateLeft:
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Z)) { return true; }
                break;
            case InputKeys.Flip:
                if (Input.GetKey(KeyCode.A)) { return true; }
                break;
            case InputKeys.SoftDrop:
                if (Input.GetKey(KeyCode.DownArrow)) { return true; }
                break;
            case InputKeys.HardDrop:
                if (Input.GetKey(KeyCode.Space)) { return true; }
                break;
            case InputKeys.Hold:
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.C)) { return true; }
                break;
        }
        return false;
    }

    private void Update()
    {
        if(blockParent != null)
        {
            #region BlockRotation
            if (GetKeyPressed(InputKeys.RotateLeft))
            {
                blockParent.RotateBlocks(BlockEnums.BlockRotateType.TurnLeft);
            }
            else if (GetKeyPressed(InputKeys.RotateRight))
            {
                blockParent.RotateBlocks(BlockEnums.BlockRotateType.TurnRight);
            }
            #endregion

            #region MoveBlockKeyPressed
            if (GetKeyPressed(InputKeys.MoveLeft))
            {
                blockParent.MoveBlocks(Vector3.left, true);
            }
            if (GetKeyPressed(InputKeys.MoveRight))
            {
                blockParent.MoveBlocks(Vector3.right, true);
            }
            #endregion

            #region MoveBlockKeyPressing
            if (GetKey(InputKeys.MoveLeft) ^ GetKey(InputKeys.MoveRight))
            {
                blockParent.DecreaseMoveDelay();
                if (GetKey(InputKeys.MoveLeft))
                {
                    blockParent.MoveBlocks(Vector3.left, false);
                }
                if (GetKey(InputKeys.MoveRight))
                {
                    blockParent.MoveBlocks(Vector3.right, false);
                }
            }
            #endregion

            #region DropBlock
            if (GetKeyPressed(InputKeys.HardDrop))
            {
                blockParent.DropBlocks(true);
            }

            if (GetKey(InputKeys.SoftDrop))
            {
                blockParent.DropBlocks(false);
                GameManager.Instance.AddScore(1);
            }
            #endregion

            #region HoldBlock
            // Block Hold
            if (GetKeyPressed(InputKeys.Hold))
            {
                blockParent.HoldBlocks();
            }
            #endregion

            #region FlipBlock
            if ((GetKeyPressed(InputKeys.Flip)))
            {
                blockParent.RotateBlocks(BlockEnums.BlockRotateType.Flip);
            }
            #endregion
        }
    }

}
