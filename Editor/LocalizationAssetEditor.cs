/*************************************
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 * LocalizationAssetEditor.cs        *
 * Created by: TheFallender          *
 * Created on: 27/02/2021 (dd/mm/yy) *
 * ©   ©   ©   ©   ©   ©   ©   ©   © *
 *************************************/

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleLocAsset))]
public class LocalizationAssetEditor : Editor {
    //Asset
    private SimpleLocAsset locAsset;

    //Serialized object
    private SerializedObject locSerialized;

    //Properties
    private SerializedProperty avlLangs;
    private SerializedProperty locKeys;

    //Variables
    int locKeyAddIndexValue = 1;

    //When it gets enabled
    private void OnEnable () {
        locAsset = (SimpleLocAsset) target;                      //Set the target
        locSerialized = new SerializedObject(locAsset);             //Serialize the object for modification
        avlLangs = locSerialized.FindProperty("availableLangs");    //Set lang property
        locKeys = locSerialized.FindProperty("localizationKeys");   //Set localization keys property
    }
    
    //Whenever the GUI is being used
    public override void OnInspectorGUI () {
        //Update the status of the list
        locSerialized.Update();

        //Variables for future use
        int langsCount = locAsset.availableLangs.Count;
        int locKeysCount = locAsset.localizationKeys.Count;
        bool langsDeleted = false;
        bool langsAdded = false;

        //######## Languages ########
        EditorGUILayout.LabelField("Languages:");
        //Text to show if there are no languages
        if (langsCount == 0)
            EditorGUILayout.LabelField("\tNo languages found.");
        else {
            //Show all the languages and allow the modification
            for (int i = 0; i < langsCount; i++) {
                //Serialized Properties
                SerializedProperty lang = avlLangs.GetArrayElementAtIndex(i);    //Get the element serialized

                //GUI Elements
                EditorGUILayout.BeginHorizontal();  //Horizontal GUI - Begin
                                                    //PopUp Selector
                lang.enumValueIndex = (int) (SimpleLocalizationLangs) EditorGUILayout.EnumPopup(         //Enum PopUp, needs casting
                    string.Format("\t{0}. {1} lang: ", i + 1, (SimpleLocalizationLangs) lang.intValue),  //Label
                    (SimpleLocalizationLangs) lang.enumValueIndex                                        //Selected value
                );
                //Button to Delete the language
                if (GUILayout.Button("x", GUILayout.ExpandWidth(false))) {
                    avlLangs.DeleteArrayElementAtIndex(i);  //Delete the element
                    langsCount--;                           //Reduce the number of availableLangs
                    i--;                                    //Reduce i to iterate through the next lang
                    langsDeleted = true;
                }
                EditorGUILayout.EndHorizontal();    //Horizontal GUI - End
            }
        }

        //Padding
        EditorGUILayout.Space();

        //Option to add the language
        if (GUILayout.Button("Add New Language", GUILayout.ExpandWidth(false))) {
            locAsset.availableLangs.Add(SimpleLocalizationLangs.Unknown);
            langsAdded = true;
        }

        //Apply modifications
        locSerialized.ApplyModifiedProperties();

        //Resize locKeys if needed
        if (langsAdded || langsDeleted) {
            //Go through each key resizing (could be optimized knowing which to delete from each key)
            foreach (LocalizationKey locKey in locAsset.localizationKeys) {
                //Add missing langs
                if (langsAdded)
                    //Loop for each of the languages available on the system
                    foreach (SimpleLocalizationLangs lang in locAsset.availableLangs)
                        //If the key does not contain the language
                        if (locKey.value.Find(value => value.key == lang) == null) 
                            locKey.value.Add(new LangText(lang, ""));
                //Remove extra langs
                if (langsDeleted) {
                    //Loop for on each of the languages in the key and remove them if they are not found in the available langs
                    for (int i = 0, langsCountInKey = locKey.value.Count; i < langsCountInKey; i++)
                        //If the available langs do not contain the language, delete it
                        if (!locAsset.availableLangs.Contains(locKey.value[i].key)) {
                            locKey.value.Remove(locKey.value[i]);   //Remove the language in the key
                            langsCountInKey--;                      //Decrease to adjust to the real size of the array
                            i--;                                    //Decrease for the next iteration
                        }
                }
            }
        }

        //Apply modifications
        locSerialized.ApplyModifiedProperties();

        //Padding
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        //######## Localization Serialized ########
        EditorGUILayout.LabelField("Key Localizations:");
        //Text to show if there are no keys
        if (locKeysCount == 0)
            EditorGUILayout.LabelField("\tNo keys found.");
        else {
            //Show all the languages and allow the modification
            for (int i = 0; i < locKeysCount; i++) {
                //Serialized Properties
                SerializedProperty locKey = locKeys.GetArrayElementAtIndex(i);      //Get the element serialized
                SerializedProperty key = locKey.FindPropertyRelative("key");        //Get the key of the localization key
                SerializedProperty value = locKey.FindPropertyRelative("value");      //Get the key of the localization value
                SerializedProperty shown = locKey.FindPropertyRelative("shown");    //Get the key of the localization value show status

                //GUI Elements
                EditorGUILayout.BeginHorizontal();  //Horizontal GUI - Begin

                //Fold Out for the langs on each key
                shown.boolValue = EditorGUILayout.Foldout(shown.boolValue, string.Format("\t{0}. Key:", i + 1), true);

                //Property field for the key
                EditorGUILayout.PropertyField(
                    key,
                    new GUIContent(""),
                    GUILayout.ExpandWidth(true)
                );

                //Button to Delete the Localization Key
                if (GUILayout.Button("x", GUILayout.ExpandWidth(false))) {
                    locKeys.DeleteArrayElementAtIndex(i);   //Delete the element
                    locKeysCount--;                         //Reduce the number of localization keys
                    i--;                                    //Reduce i to iterate through the next key
                    EditorGUILayout.EndHorizontal();    //Horizontal GUI - End
                    continue;
                }

                EditorGUILayout.EndHorizontal();    //Horizontal GUI - End

                //Show the langs and each of the values
                if (shown.boolValue) {
                    for (int j = 0; j < value.arraySize; j++) {
                        SerializedProperty langText = value.GetArrayElementAtIndex(j);
                        SerializedProperty lang = langText.FindPropertyRelative("key");
                        SerializedProperty text = langText.FindPropertyRelative("value");

                        EditorGUILayout.BeginHorizontal();  //Horizontal GUI - Begin

                        //Property field for the language
                        EditorGUILayout.LabelField(
                            "\t\t" + ((SimpleLocalizationLangs) lang.enumValueIndex).ToString()
                        );
                        //Property field for the text
                        EditorGUILayout.PropertyField(
                            text,
                            new GUIContent(""),
                            GUILayout.ExpandWidth(true)
                        );

                        EditorGUILayout.EndHorizontal();    //Horizontal GUI - End
                    }
                }
            }
        }

        //Padding
        EditorGUILayout.Space();

        //Buttons to add the localizationKeys
        EditorGUILayout.BeginHorizontal();  //Horizontal GUI - Begin
        bool locKeyAddButton = GUILayout.Button("Add New Key", GUILayout.ExpandWidth(false));
        bool locKeyAddIndexButton = GUILayout.Button("Add New Key at index", GUILayout.ExpandWidth(false));
        locKeyAddIndexValue = EditorGUILayout.IntField(locKeyAddIndexValue);
        EditorGUILayout.EndHorizontal();    //Horizontal GUI - End

        //Option to add the localization key
        if (locKeyAddButton)
            locAsset.localizationKeys.Add(new LocalizationKey("", locAsset.availableLangs));
        //Option to add the localization key at an specific index
        if (locKeyAddIndexButton) {
            if (locKeyAddIndexValue > 0 && locKeyAddIndexValue < locKeysCount)
                locAsset.localizationKeys.Insert(locKeyAddIndexValue - 1, new LocalizationKey("", locAsset.availableLangs));
            else
                Debug.LogError("ERROR - Index must be positive and less than the size of the keys.");
        }

        //Apply modifications
        locSerialized.ApplyModifiedProperties();
    }
}