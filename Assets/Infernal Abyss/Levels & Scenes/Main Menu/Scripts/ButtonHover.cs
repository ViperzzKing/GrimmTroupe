using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverLine;
    public Text hoverIncrease;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverLine.SetActive(true);
        hoverIncrease.fontSize = 50;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverLine.SetActive(false);
        hoverIncrease.fontSize = 45;
    }
}
