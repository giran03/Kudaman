using UnityEngine;
using UnityEngine.UI;

public class PlayerGenderSelector : MonoBehaviour
{
    string playerGender = "male";
    int selectedIndex = 0;
    public void SelectedMale() => playerGender = "male";
    public void SelectedFemale() => playerGender = "female";
    public void SelectedMaleVariant(int index) => selectedIndex = index;
    [Header("Variant Configs")]
    public Image maleDisplay;
    public Sprite[] maleVariants;
    public Image femaleDisplay;
    public Sprite[] femaleVariants;

    public void SavePlayerPreferences()
    {
        PlayerPrefs.SetString("playerGender", playerGender);
        PlayerPrefs.SetInt("playerGenderVariant", selectedIndex);
    }
    void OnDisable()
    {
        SavePlayerPreferences();
        Debug.Log($"Saving player gender: {playerGender}, selected Variant Index: {selectedIndex}");
    }

    public void NextMaleVariant()
    {
        selectedIndex = (selectedIndex + 1) % maleVariants.Length;
        maleDisplay.sprite = maleVariants[selectedIndex];
    }

    public void PreviousMaleVariant()
    {
        selectedIndex = (selectedIndex - 1 + maleVariants.Length) % maleVariants.Length;
        maleDisplay.sprite = maleVariants[selectedIndex];
    }

    public void NextFemaleVariant()
    {
        selectedIndex = (selectedIndex + 1) % femaleVariants.Length;
        femaleDisplay.sprite = femaleVariants[selectedIndex];
    }

    public void PreviousFemaleVariant()
    {
        selectedIndex = (selectedIndex - 1 + femaleVariants.Length) % femaleVariants.Length;
        femaleDisplay.sprite = femaleVariants[selectedIndex];
    }
}
