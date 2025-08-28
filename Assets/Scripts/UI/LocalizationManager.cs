using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Threading.Tasks;
using System.Security.Cryptography;

public class LocalizationManager : Singleton<LocalizationManager>
{
    public int localizationId;
     private void Start()
    {
        CheckLocalization();
    }
    public void SetLanguege(int _id)
    {
        StartCoroutine(CouroutineSetLanguege(_id));
    }
    IEnumerator CouroutineSetLanguege(int _id)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_id];
        localizationId = _id;
        PlayerPrefs.SetInt("localizationID", _id);
      

    }
    public void CheckLocalization()
    {
        if (PlayerPrefs.HasKey("localizationID"))
        {
            int id = PlayerPrefs.GetInt("localizationID");
            localizationId = id;
            return;

        }
        else
        {
            var selectedLocale = LocalizationSettings.SelectedLocale;
            if (selectedLocale != null)
            {
                for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
                {
                    if (LocalizationSettings.AvailableLocales.Locales[i] == selectedLocale)
                    {
                        localizationId = i;

                    }
                }
            }
        }
       
    }
    public async Task<string> GetLocalizedText(string key, string tableName = "Localization Table")
    {
        var handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName);
        var table = await handle.Task;
        var entry = table?.GetEntry(key);
        return entry?.GetLocalizedString();
    }
   
}
