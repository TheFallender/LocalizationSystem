/*************************************
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 * LocalizeText.cs                   *
 * Created by: TheFallender          *
 * Created on: 27/02/2021 (dd/mm/yy) *
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 *************************************/

using UnityEngine;
using UnityEngine.UI;

public class LocalizeText : MonoBehaviour {
    [SerializeField]
    private Text textToLocalize;
    [SerializeField]
    private string keyLocalization;

    private void Start () {
        if (textToLocalize == null)
            textToLocalize = GetComponent<Text>();
        SimpleLocalization.Instance.OnLanguageChanged += Localize;
        Localize(SimpleLocalization.Instance.CurrentLang);
    }

    private void OnDestroy () {
        SimpleLocalization.Instance.OnLanguageChanged -= Localize;
    }

    private void Localize (SimpleLocalizationLangs language) {
        LocalizationKey tempKey =
            SimpleLocalization.Instance.LocAsset.localizationKeys.Find(
                elem => elem.key == keyLocalization
            );

        if (tempKey != null) {
            LangText tempLangText =
                tempKey.value.Find(
                    lang => lang.key == language
                );

            if (tempLangText != null)
                textToLocalize.text = tempLangText.value;
            else
                Debug.LogWarning(string.Format("WRN - Could not find language {0} in the key {1}.", language, tempKey.key));
        } else
            Debug.LogWarning(string.Format("WRN - Could not find key {0} in the asset.", keyLocalization));
    }
}