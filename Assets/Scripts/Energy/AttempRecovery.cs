using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttempRecovery : MonoBehaviour
{
    [Header("Properties")]
    public int CurrentTime;
    public int PreviousTime;
    [Space]
    public bool IsNeworkError;
    public bool IsHttpError;
    public bool IsLoaded;
    public bool IsCompleteLoaded;
    public bool IsLocalDataFounded;

    [Header("Settings")]
    [SerializeField] private string localLastReceiveBonusTimeKey;
    [Space]
    [SerializeField] private string serverUri;
    [Space]
    [SerializeField] private bool useLocalData;
    [SerializeField] private int localTime;

    [System.Obsolete]
    private IEnumerator SendRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUri);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            IsNeworkError = true;
            IsLoaded = true;
            Debug.Log(request.error);
            yield break;
        }

        if (request.isHttpError)
        {
            IsHttpError = true;
            IsLoaded = true;
            Debug.Log(request.error);
        }

        var json = request.downloadHandler.text;
        ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(json);

        CurrentTime = response.unixtime;
        IsCompleteLoaded = true;
        IsLoaded = true;
    }

    private void LoadLocalData()
    {
        if (PlayerPrefs.HasKey(localLastReceiveBonusTimeKey))
        {
            PreviousTime = PlayerPrefs.GetInt(localLastReceiveBonusTimeKey);
            IsLocalDataFounded = true;
        }
    }

    private void SetPreviousTime()
    {
        PreviousTime = CurrentTime;
        PlayerPrefs.SetInt(localLastReceiveBonusTimeKey, PreviousTime);
    }

    private void RecoverAttemp()
    {
        if (IsLocalDataFounded && CurrentTime - PreviousTime >= 86400)
            MainScript.AttempCount = 5;
    }

    [System.Obsolete]
    private void Awake()
    {
        if (useLocalData)
        {
            LoadLocalData();
            CurrentTime = localTime;
            IsCompleteLoaded = true;
            IsLoaded = true;
        }
        else
        {
            LoadLocalData();
            StartCoroutine(routine: SendRequest());
        }
    }
}
