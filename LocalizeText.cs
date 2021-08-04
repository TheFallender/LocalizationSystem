/*************************************
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 * LocalizeText.cs                   *
 * Created by: TheFallender          *
 * Created on: 27/02/2021 (dd/mm/yy) *
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 *************************************/

using UnityEngine;
using UnityEngine.UI;

namespace UniLoc {
    public class LocalizeText : MonoBehaviour {
        [SerializeField]
        private Text textToLocalize;
        [SerializeField]
        private string keyLocalization;

        private void Start () {
            if (textToLocalize == null)
                textToLocalize = GetComponent<Text>();
            LocSystem.Instance.OnLanguageChanged += Localize;
            Localize(LocSystem.Instance.CurrentLang);
        }

        private void OnDestroy () {
            LocSystem.Instance.OnLanguageChanged -= Localize;
        }

        private void Localize (UniLocLangs language) {
            LocKey tempKey =
                LocSystem.Instance.LocAsset.localizationKeys.Find(
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
}