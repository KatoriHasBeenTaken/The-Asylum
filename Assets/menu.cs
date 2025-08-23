using UnityEngine;

public class menu : MonoBehaviour
{
    public GameObject settingpannel;
    public GameObject graphicsetting;
    public GameObject audiosetting;
    public GameObject menustting;
    public void option()
    {
        settingpannel.SetActive(true);
        graphicsetting.SetActive(false);
        audiosetting.SetActive(false);
        menustting.SetActive(false);
}
}
