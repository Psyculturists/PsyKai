using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Drop Table", menuName = "Items/Drop Table")]
public class DropTable : ScriptableObject
{
    [SerializeField]
    private ItemOddsDictionary table = new ItemOddsDictionary();

    private bool isBalanced = false;
    private bool isNotBalanced => !isBalanced;

    private bool TableIsBalanced()
    {
        float total = 0;
        foreach(var dataPair in table)
        {
            total += dataPair.Value;
        }
        isBalanced = total == 100.0f;
        return isBalanced;
    }

    public ItemData Evaluate()
    {
        float randomSelector = UnityEngine.Random.Range(0, 100.0f);
        foreach(var pair in table)
        {
            randomSelector -= pair.Value;
            if(randomSelector <= 0.0f)
            {
                return pair.Key;
            }
        }
        Debug.LogError("Somehow did not randomly select an entry. Returning first entry by default.");
        return table.FirstOrDefault().Key;
    }
}
