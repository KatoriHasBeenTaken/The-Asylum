using UnityEngine;

public class buttonsetting : MonoBehaviour
{

    public GameObject graphicsetting;
    public GameObject audiosetting;
    public GameObject pannel;
    public GameObject menusetting;
    private void Start()
    {
        DontDestroyOnLoad(pannel);
    }
    public void graphics()
    {
        graphicsetting.SetActive(true);
        audiosetting.SetActive(false);
    }
    public void audio()
    {
        graphicsetting.SetActive(false);
        audiosetting.SetActive(true);
    }
    public void exit()
    {
        graphicsetting.SetActive(false);
        audiosetting.SetActive(false);
        pannel.SetActive(false);
        menusetting.SetActive(true);
    }
}
