using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonUnderlineHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText;

    private FontStyles originalStyle;

    void Start()
    {
        if (buttonText != null)
        {
            originalStyle = buttonText.fontStyle;
        }
    }

    // When mouse hovers over button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.fontStyle |= FontStyles.Underline;
        }
    }

    // When mouse leaves button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.fontStyle = originalStyle;
        }
    }
}