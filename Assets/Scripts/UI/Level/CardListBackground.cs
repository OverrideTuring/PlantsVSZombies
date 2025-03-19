using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardListBackground : MonoBehaviour
{
    [Header("Cards Settings")]
    [SerializeField] private List<Transform> cardSlots;
    [SerializeField] private int maxLength;
    private List<Card> cards = new List<Card>();
    private int currentLength;
    [Header("UI Movement")]
    [SerializeField] private float topY;
    [SerializeField] private float downY;

    private void Start()
    {
        List<PlantType> plantTypes = PlayerData.Plants;

        currentLength = Mathf.Min(plantTypes.Count, maxLength);
        for (int i = 0; i < currentLength; i++)
        {
            Card cardPrefab = PlantFactory.Instance.GetCardPrefab(plantTypes[i]);
            cards.Add(Instantiate(cardPrefab, cardSlots[i].transform, false));
        }
    }

    public void MoveDown()
    {
        GetComponent<RectTransform>().DOAnchorPosY(downY, 1.0f);
    }

    public void DisableCards()
    {
        for(int i = 0; i < currentLength; i++)
        {
            cards[i].Disable();
        }
    }

    public void EnableCards()
    {
        for (int i = 0; i < currentLength; i++)
        {
            cards[i].Enable();
        }
    }
}
