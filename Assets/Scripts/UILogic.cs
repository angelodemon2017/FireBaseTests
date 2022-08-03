using UnityEngine;
using UnityEngine.UI;

public class UILogic : MonoBehaviour
{
    public static UILogic instance;

    public InputField linkIF;
    public Text linkUIText;

    public db _db;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateText(string message) 
    {
        linkUIText.text = message;
    }

    public void ButtonSaveFBd()
    {
    //    _db.SaveData(linkIF.text);
    }

    public void ButtonSaveFSD() 
    {
        _db.SaveToCloudStore();
    }

    public void BuittonLoadFSB() 
    {
    //    _db.ButtonC();
    }
}
