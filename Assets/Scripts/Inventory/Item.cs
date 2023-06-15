using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    protected ItemData data;
    public ItemData Data => data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item()
    {

    }

    public Item(ItemData startingData)
    {
        data = startingData;
    }
}
