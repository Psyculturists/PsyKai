using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSkillScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject root;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private CurrentSkillCard skillCardPrefab;
    [SerializeField]
    private Transform skillCardParent;
    [SerializeField]
    private SkillPreviewScreen skillPreviewPanel;
    [SerializeField]
    private NewSkillChoiceCard newSkillCardPrefab;
    [SerializeField]
    private Transform newSkillParent;

    private List<CurrentSkillCard> spawnedSkillCards = new List<CurrentSkillCard>();
    private List<NewSkillChoiceCard> spawnedNewSkillCards = new List<NewSkillChoiceCard>();

    private CurrentSkillCard selectedCard;
    private NewSkillChoiceCard selectedNewCard;

    private void Awake()
    {
        exitButton.onClick.AddListener(CloseMenu);
        skillPreviewPanel.SetAcceptFunctionality(OverrideSkillCardEntry);
    }

    // two separate functions as probably need diff handling later than just toggle
    public void OpenMenu()
    {
        root.SetActive(true);
        SpawnCurrentCards();
    }

    public void CloseMenu()
    {
        CleanupCurrentCards();
        CleanupNewCards();
        selectedCard = null;
        selectedNewCard = null;
        root.SetActive(false);
    }

    private void SpawnCurrentCards()
    {
        for(int i = 0; i < PlayerDataManager.Instance.MaxSkills; i++)
        {
            CurrentSkillCard card = Instantiate(skillCardPrefab, skillCardParent);
            card.Init(i + 1, PlayerDataManager.Instance.CurrentSkillLoadout[i], SkillCardSelected);
            spawnedSkillCards.Add(card);
        }
    }

    private void SpawnNewSkillCards()
    {
        if (spawnedNewSkillCards.Count > 0) return;
        foreach(Skill skill in PlayerDataManager.Instance.UnlockableSkills)
        {
            if (skill.LevelRequired > PlayerDataManager.Instance.PlayerLevel) continue;
            NewSkillChoiceCard card = Instantiate(newSkillCardPrefab, newSkillParent);
            card.Initialise(skill, NewSkillCardSelected);
            spawnedNewSkillCards.Add(card);
        }
    }

    private void CleanupCurrentCards()
    {
        skillCardParent.DestroyAllChildren();
        spawnedSkillCards.Clear();
    }

    private void CleanupNewCards()
    {
        newSkillParent.DestroyAllChildren();
        spawnedNewSkillCards.Clear();
        skillPreviewPanel.AssignSkill(null);
    }

    private void SkillCardSelected(CurrentSkillCard card)
    {
        if(selectedCard == card)
        {
            card.SetHighlight(false);
            skillPreviewPanel.AssignSkill(null);
            selectedCard = null;
            CleanupNewCards();
            return;
        }
        else
        {
            selectedCard?.SetHighlight(false);
            selectedCard = card;
            selectedCard.SetHighlight(true);
            skillPreviewPanel.AssignSkill(selectedCard.HousedSkill);
            SpawnNewSkillCards();
        }
    }

    private void NewSkillCardSelected(NewSkillChoiceCard card)
    {
        if(card == selectedNewCard)
        {
            selectedNewCard.SetHighlight(false);
            skillPreviewPanel.AssignSkill(null);
            selectedNewCard = null;
            return;
        }
        skillPreviewPanel.AssignSkill(card.HousedSkill);
        card.SetHighlight(true);
        selectedNewCard?.SetHighlight(false);
        selectedNewCard = card;
    }

    private void OverrideSkillCardEntry()
    {
        selectedCard.Init(spawnedSkillCards.IndexOf(selectedCard), selectedNewCard.HousedSkill, SkillCardSelected);
        UpdatePlayerSkills();
    }

    private void UpdatePlayerSkills()
    {
        List<Skill> selectedSkills = new List<Skill>();
        foreach(CurrentSkillCard card in spawnedSkillCards)
        {
            selectedSkills.Add(card.HousedSkill);
        }
        PlayerDataManager.Instance.UpdateSkillLoadout(selectedSkills);
    }
}
