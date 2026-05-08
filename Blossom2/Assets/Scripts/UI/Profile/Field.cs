using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    private Sprite sprite;
    public UnityEvent<Sprite> onClick;

    public void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    public void OnClicked()
    {
        onClick?.Invoke(sprite);
    }
}