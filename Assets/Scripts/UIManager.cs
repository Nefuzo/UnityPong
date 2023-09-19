using Mirror;
using System.Collections;
using Telepathy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    bool GameStarted;
    private string ip_adress;
    private NetworkManager networkManager;
    private bool CanJoinGame;
    [SerializeField] private GameObject NewGameBtn, JoinBtn, IPBtn;
    [SerializeField] private GameObject ClickAnyText, WaitingForText, WelcomeText, NetworkAdressText, ErrorText, TypeText;

    public GameObject TypeIPBtn;
    public ImportantSettings settings;
    string text;
    void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
        WaitingForText.SetActive(false);
        ErrorText.SetActive(false);
        NetworkAdressText.SetActive(false);
    }

    void Update()
    {
        text = TypeIPBtn.GetComponent<TMP_InputField>().text;
        ReadInput(text);
        OnSecondPlayerDisconnect();
        if (WaitingForText.activeInHierarchy == true && GameObject.FindGameObjectsWithTag("PlayerPrefab").Length == 2)
        {
            WaitingForText.SetActive(false);
        }
    }

    public void CreateNewGame()
    {
        networkManager.StartHost();
        UITextManager(false);
        WaitingForText.SetActive(true);
        NetworkAdressManager();
    }

    public void JoinToExistingGame()
    {
        if (CanJoinGame)
        {
            if (!settings.IsDebugMode) networkManager.networkAddress = ip_adress;
            networkManager.StartClient();
            UITextManager(false);
            StartCoroutine(IsConnectedCheck());
            ErrorText.SetActive(false);
            NetworkAdressManager();
        }
        else
        {
            ErrorText.SetActive(true);
            ErrorText.GetComponent<TextMeshProUGUI>().text = "Error: Incorrect ip adress!";
        }
    }

    void UITextManager(bool value)
    {
        NewGameBtn.SetActive(value);
        JoinBtn.SetActive(value);
        ClickAnyText.SetActive(value);
        NetworkAdressText.SetActive(value);
        WelcomeText.SetActive(false);
        TypeIPBtn.SetActive(value);
        IPBtn.SetActive(value);
        TypeText.SetActive(value);
    }

    IEnumerator IsConnectedCheck()
    {
        yield return new WaitForSeconds(2);
        if (!NetworkClient.isConnected)
        {
            UITextManager(true);
            ClickAnyText.GetComponent<TextMeshProUGUI>().text = "Could not find any server!";
            NetworkAdressText.SetActive(false);
        }
    }

    void ReadInput(string s)
    {
        ip_adress = s;
        CanJoinGame = IPWriteCorrectly(s);
    }

    bool IPWriteCorrectly(string s)
    {
        if (settings.IsDebugMode) return true;
        if (s == string.Empty) return false;
        if (s.Length <= 12) return false;
        return true;
    }

    void NetworkAdressManager()
    {
        NetworkAdressText.SetActive(true);
        NetworkAdressText.GetComponent<TextMeshProUGUI>().text = "Your IP adress: " + networkManager.networkAddress;
    }
    void OnSecondPlayerDisconnect()
    {
        if(GameObject.FindGameObjectsWithTag("PlayerPrefab").Length == 2) GameStarted = true;
        if(GameStarted && GameObject.FindGameObjectsWithTag("PlayerPrefab").Length < 2)
        {
            GameStarted = false;
            UITextManager(true);
            if(NetworkServer.activeHost) networkManager.StopHost();
            ErrorText.SetActive(true);
            ErrorText.GetComponent<TextMeshProUGUI>().text = "Second player disconnected from the server!";
        }
    }
}
