using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthOverlay;

    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private FloatingHealthChange healthChangePrefab;
    [SerializeField]
    private Transform healthChangeParent;

    private float originalWidth = 250.0f;

    private Stack<DifferenceToSpawn> differenceStack = new Stack<DifferenceToSpawn>();
    private float timeBetweenDiffSpawns = 0.3f;
    private float timeSinceSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        originalWidth = GetComponent<RectTransform>().sizeDelta.x;
    }

    public void UpdateBar(int current, int max, int difference = 0)
    {
        if (this == null) return;
        healthOverlay.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((float)current / (float)max) * originalWidth);
        healthText.text = current + " / " + max;
    }

    protected struct DifferenceToSpawn
    {
        public int number;
        public StatEffectData effect;
    }

    public void AddChangeToQueue(int diff, StatEffectData data)
    {
        DifferenceToSpawn info = new DifferenceToSpawn() { number = diff, effect = data };
        differenceStack.Push(info);
    }

    private void SpawnChange(DifferenceToSpawn diff)
    {
        if (diff.number != 0)
        {
            FloatingHealthChange change = Instantiate(healthChangePrefab, healthChangeParent);
            change.Initialise(diff.number);
        }
        else if(!diff.effect.Equals(default(StatEffectData)))
        {
            FloatingHealthChange change = Instantiate(healthChangePrefab, healthChangeParent);
            change.Initialise(diff.effect);
        }
    }

    private void Update()
    {
        if(differenceStack.Count > 0)
        {
            if(timeSinceSpawn >= timeBetweenDiffSpawns)
            {
                timeSinceSpawn = 0.0f;
                DifferenceToSpawn info = differenceStack.Pop();
                SpawnChange(info);
            }
        }
        timeSinceSpawn += Time.deltaTime;
    }
}
