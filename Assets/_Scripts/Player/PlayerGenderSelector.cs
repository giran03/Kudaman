using UnityEngine;

public class PlayerGenderSelector : MonoBehaviour
{
    string playerGender = "Male";
    public void SelectedMale() => playerGender = "Male";
    public void SelectedFemale() => playerGender = "Female";

    public void SavePlayerGender() => PlayerPrefs.SetString("playerGender", playerGender);
    void OnDisable()
    {
        SavePlayerGender();
        Debug.Log($"Saving player gender: {playerGender}");
    }
}
