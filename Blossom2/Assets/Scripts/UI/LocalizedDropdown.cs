using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizedDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    [SerializeField] private string _tableName;
    [SerializeField] private string[] _keys;
    void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        UpdateOptions();
    }

    void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        UpdateOptions();
    }

    void UpdateOptions()
    {
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (string key in _keys)
        {
            string translated = LocalizationSettings.StringDatabase.GetLocalizedString(_tableName, key);
            options.Add(new TMP_Dropdown.OptionData(translated));
        }

        int previousValue = _dropdown.value;
        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);
        _dropdown.value = previousValue;
        _dropdown.RefreshShownValue();
    }

    void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }
}
