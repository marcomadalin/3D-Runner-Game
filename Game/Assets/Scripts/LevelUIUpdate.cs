using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelUIUpdate : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioSource hover = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable) hover.Play();
    }

}
