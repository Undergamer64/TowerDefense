using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<CardUI> OnEnter;
    public UnityEvent<CardUI> OnExit;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit.Invoke(this);
    }
}
