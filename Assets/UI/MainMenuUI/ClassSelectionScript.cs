using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassSelectionScript : MonoBehaviour, IPointerEnterHandler
{
    GameObject ClassText;
    public Text ClassDesc;
    public string ClassString;
    public Image ClassBird;
    public Sprite BirdSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ClassDesc.text = ClassString;
        ClassBird.sprite = BirdSprite;
    }
    public void ClassSelect(int num)
    {
        PlayerPrefs.SetInt("classNum", num);
        SceneManager.LoadScene("Combat");
    }

    public void RandomClass()
    {
        ClassSelect(UnityEngine.Random.Range(1, 7));
    }
}

