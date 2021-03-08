/*************************************
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 * LocalizationSystem.cs             *
 * Created by: TheFallender          *
 * Created on: 27/02/2021 (dd/mm/yy) *
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 *************************************/

using UnityEngine;

namespace SimpleLocalization {
    public class LocalizationSystem : MonoBehaviour {
        //Instance
        private static LocalizationSystem instance = null;
        public static LocalizationSystem Instance {
            get {
                return instance;
            }
        }

        //Event
        public delegate void LanguageChangeEvent (SimpleLocalizationLangs language);
        public LanguageChangeEvent OnLanguageChanged;

        //Language Asset
        [SerializeField]
        private SimpleLocAsset locAsset;
        public SimpleLocAsset LocAsset {
            get {
                return Instance.locAsset;
            }
        }

        //Current language
        private SimpleLocalizationLangs currentLang = SimpleLocalizationLangs.English;
        public SimpleLocalizationLangs CurrentLang {
            get {
                return Instance.currentLang;
            }
        }

        //Singleton
        private void Awake () {
            if (instance != null) {
                Destroy(this);
            } else {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }

        //Event call: Language Enum
        public virtual void ChangeLanguage (SimpleLocalizationLangs language) {
            currentLang = language;
            OnLanguageChanged?.Invoke(currentLang);
        }

        //Event call: Language String
        public virtual void ChangeLanguage (string language) {
            currentLang = (SimpleLocalizationLangs) System.Enum.Parse(
                typeof(SimpleLocalizationLangs),
                language
            );
            OnLanguageChanged?.Invoke(currentLang);
        }

        //Event call: Language Index Position in available langs
        public virtual void ChangeLanguage (int languageIndex) {
            currentLang = LocAsset.availableLangs[languageIndex];

            OnLanguageChanged?.Invoke(currentLang);
        }
    }
}