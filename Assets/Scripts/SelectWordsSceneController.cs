using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectWordsSceneController : MonoBehaviour
{
    public GameObject titleUIObject;
    public InputField nameInputField;

    public GameObject selectWordsUIObject;

    public Dropdown dropdownWordA;
    public Dropdown dropdownWordB;
    public Dropdown dropdownWordC;

    public List<string> wordListA = new List<string>() { "Empty" };
    public List<string> wordListB = new List<string>() { "Empty" };
    public List<string> wordListC = new List<string>() { "Empty" };

    private void Start()
    {
        selectWordsUIObject.SetActive(false);
        titleUIObject.SetActive(true);

        dropdownWordA.options = GenerateDropdownDatas(wordListA);
        dropdownWordB.options = GenerateDropdownDatas(wordListB);
        dropdownWordC.options = GenerateDropdownDatas(wordListC);

        dropdownWordA.value = Random.Range(0, wordListA.Count - 1);
        dropdownWordB.value = Random.Range(0, wordListB.Count - 1);
        dropdownWordC.value = Random.Range(0, wordListC.Count - 1);
    }

    public void ShowSelectWords()
    {
        Core.Instance.PlayerName = string.IsNullOrEmpty(nameInputField.text) ? "Unknown" : nameInputField.text;

        selectWordsUIObject.SetActive(true);
        titleUIObject.SetActive(false);
    }

    public void OnValueChanged(int result)
    {
        Debug.Log("Connected Words; " + GetConnectedWords());
    }

    public string GetConnectedWords()
    {
        return dropdownWordA.captionText.text
             + dropdownWordB.captionText.text
             + dropdownWordC.captionText.text;
    }

    public void StartMainGame()
    {
        Core.Instance.Message = GetConnectedWords();
        Core.Instance.OnMainGame();
    }

    private List<Dropdown.OptionData> GenerateDropdownDatas(List<string> strList)
    {
        return new List<Dropdown.OptionData>(strList.Select(str => new Dropdown.OptionData { text = str }));
    }
}