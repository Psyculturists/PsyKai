using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonGenerator : MonoBehaviour
{
    public GameObject buttonPrefab;

    public void CreateChoices(int amount)
    {
        Debug.Log("Create :D");
        DestroyChoices();
        for (int i = 0; i < amount; i++)
        {
            Instantiate(buttonPrefab, transform);
        }
    }

    public void DestroyChoices()
    {
        Debug.Log("Destroy >:D");
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

}
