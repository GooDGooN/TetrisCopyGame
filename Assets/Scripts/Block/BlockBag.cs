using System.Collections.Generic;
using UnityEngine;

public class BlockBag : MonoBehaviour
{
    private List<BlockEnums.BlockType> myBlockBag = new List<BlockEnums.BlockType>();

    private void Awake()
    {
        RefillBag();
    }

    public BlockEnums.BlockType PopBlockFromBag()
    {
        var result = myBlockBag[Random.Range(0, myBlockBag.Count)];
        myBlockBag.Remove(result);
        if(myBlockBag.Count == 0)
        {
            RefillBag();
        }
        return result;
    }


    private void RefillBag()
    {
        for (int i = 0; i < 7; i++)
        {
            myBlockBag.Add((BlockEnums.BlockType)i);
        }
    }
}
