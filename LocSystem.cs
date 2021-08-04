/*************************************
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 * LocSystem.cs                      *
 * Created by: TheFallender          *
 * Created on: 27/02/2021 (dd/mm/yy) *
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 *************************************/

using UnityEngine;

namespace UniLoc {
    public class LocSystem : MonoBehaviour {
        //Instance
        private static LocSystem instance = null;
        public static LocSystem Instance {
            get {
                return instance;
            }
        }

        //Event
        public delegate void LanguageChangeEvent (UniLocLangs language);
        public LanguageChangeEvent OnLanguageChanged;

        //Language Asset
        [SerializeField]
        private LocAsset locAsset;
        public LocAsset LocAsset {
            get {
                return Instance.locAsset;
            }
        }
        public void SetLocAsset (LocAsset asset) {
            locAsset = asset;
        }

        //Current language
        private UniLocLangs currentLang = UniLocLangs.English;
        public UniLocLangs CurrentLang {
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
        public virtual void ChangeLanguage (UniLocLangs language) {
            currentLang = language;
            OnLanguageChanged?.Invoke(currentLang);
        }

        //Event call: Language String
        public virtual void ChangeLanguage (string language) {
            currentLang = String2Lang(language);
            OnLanguageChanged?.Invoke(currentLang);
        }

        //Event call: Language Index Position in available langs
        public virtual void ChangeLanguage (int languageIndex) {
            currentLang = LocAsset.availableLangs[languageIndex];
            OnLanguageChanged?.Invoke(currentLang);
        }


        //String to UniLocLangs
        public static UniLocLangs String2Lang (string lang) {
            return (UniLocLangs) System.Enum.Parse(
                    typeof(UniLocLangs),
                    lang,
                    true
            );
        }
    }
}