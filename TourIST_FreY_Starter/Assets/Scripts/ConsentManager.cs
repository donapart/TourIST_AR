using UnityEngine;
using UnityEngine.UI;

public class ConsentManager : MonoBehaviour
{
    public GameObject consentPanel;
    public Toggle analyticsToggle;
    public Toggle locationToggle;
    public Button acceptButton;
    public Button deleteDataButton;

    private const string ConsentKey = "UserConsentGiven";
    private const string AnalyticsOptOutKey = "UserAnalyticsOptOut";
    private const string LocationOptInKey = "UserLocationOptIn";

    void Start()
    {
        if (!PlayerPrefs.HasKey(ConsentKey))
        {
            consentPanel.SetActive(true);
        }
        else
        {
            consentPanel.SetActive(false);
        }

        acceptButton.onClick.AddListener(AcceptConsent);
        deleteDataButton.onClick.AddListener(DeleteAllData);
    }

    void AcceptConsent()
    {
        PlayerPrefs.SetInt(ConsentKey, 1);
        PlayerPrefs.SetInt(AnalyticsOptOutKey, analyticsToggle.isOn ? 0 : 1);
        PlayerPrefs.SetInt(LocationOptInKey, locationToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        consentPanel.SetActive(false);
    }

    void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
        consentPanel.SetActive(true);
    }

    public static bool HasConsent() => PlayerPrefs.GetInt(ConsentKey, 0) == 1;
    public static bool IsAnalyticsOptOut() => PlayerPrefs.GetInt(AnalyticsOptOutKey, 1) == 1;
    public static bool IsLocationOptedIn() => PlayerPrefs.GetInt(LocationOptInKey, 0) == 1;
}
