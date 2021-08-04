using UnityEngine;
using System.Collections.Generic;

public class DropdownLangs : MonoBehaviour {
    UnityEngine.UI.Dropdown dropdownComp;


    private void Start () {
        dropdownComp = GetComponent<UnityEngine.UI.Dropdown>();
        List<string> dropdownOptions = new List<string>();
        foreach (var lang in UniLoc.LocSystem.Instance.LocAsset.availableLangs)
            dropdownOptions.Add(lang.ToString());
        dropdownComp.AddOptions(dropdownOptions);
    }

    public void OnLangSelect () {
        UniLoc.LocSystem.Instance.ChangeLanguage(dropdownComp.value);
    }
}