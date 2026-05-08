using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public struct AvatarData
{
    public string userName;
    public int avatarIndex;
    public Sprite avatarSprite;
}

public class AvatarChanger: MonoBehaviour
{
    [SerializeField] Sprite[] avatars;
    [SerializeField] GameObject avatarObj;
    [SerializeField] GameObject userNameObj; // empty
    [SerializeField] GameObject fieldPrefab;
    [SerializeField] Transform contentParent;
    public UnityEvent<AvatarData> onAvatarChanged;

    public void Init(string userName, int avatarIndex)
    {
        userNameObj.GetComponent<InputField>().text = userName;
        avatarObj.GetComponent<Image>().sprite = avatars[avatarIndex];
        InstantiatePrefab();

    }

    private void InstantiatePrefab()
    {
        for (int i = 0; i < avatars.Length; i++)
        {
            GameObject field = Instantiate(fieldPrefab, contentParent);
            field.GetComponent<Image>().sprite = avatars[i];
            field.GetComponent<Field>().onClick.AddListener(SetPicture);
        }
    }

    protected void SetPicture(Sprite sprite)
    {
        avatarObj.GetComponent<Image>().sprite = sprite;
    }
    public void OnApplyChanges()
    {
        int index = System.Array.IndexOf(avatars, avatarObj.GetComponent<Image>().sprite);
        string userName = userNameObj.GetComponent<InputField>().text;
        onAvatarChanged?.Invoke(new AvatarData {
            userName = userName, 
            avatarIndex = index,
            avatarSprite = avatars[index]
            });
    }
}