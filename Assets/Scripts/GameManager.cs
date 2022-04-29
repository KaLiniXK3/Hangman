using UnityEngine;
using TMPro;
using System.IO;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI trueCountText;
    [SerializeField] TextMeshProUGUI falseCountText;
    [SerializeField] TextMeshProUGUI wordFindText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] TextMeshProUGUI wrongLettersText;

    [SerializeField] GameObject trueImage;
    [SerializeField] GameObject falseImage;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;
    [SerializeField] GameObject[] manParts;

    [SerializeField] SoundManager soundManager;

    private string[] wordTypes;
    private string wrongLetter;

    private string[] words;
    private string chosenWord;
    private string hiddenWord;
    private string pressedLetter = "";

    private float timer;
    private int second;
    private int minute;

    private int faults;
    private string hint;

    private int trueCount;
    private int falseCount;

    bool gameEnd;

    private void Awake()
    {
        // TAKE WORDS FROM TEXT FILE
        wordTypes = Directory.GetFiles(Application.streamingAssetsPath, "*.txt");
    }
    private void Start()
    {
        Debug.Log("DataPath: " + Application.dataPath);
        Debug.Log("StreamingAssetsPath: " + Application.streamingAssetsPath);
        Debug.Log("PersistentDataPath: " + Application.persistentDataPath);


        //CREATE NEW WORD
        NewWord();
    }
    private void Update()
    {
        // SetTime
        if (!gameEnd) SetTime();
        KeyEvents();
    }
    private void KeyEvents()
    {
        // TRUE KEY OR WRONG KEY CHECK CASE
        if (Input.anyKeyDown && !gameEnd)
        {
            pressedLetter = Input.inputString;
            if (string.IsNullOrWhiteSpace(pressedLetter)) return;

            // CHECK CHOSEN WORD AND IGNORE CAPITAL OR LOWER CASES. IS KEY INSIDE OF OUR WORD?
            if (chosenWord.Contains(pressedLetter, StringComparison.InvariantCultureIgnoreCase))
            {
                // TAKE INDEX OF OUR KEY
                int indexLetter = chosenWord.IndexOf(pressedLetter, StringComparison.InvariantCultureIgnoreCase);
                while (indexLetter != -1)
                {
                    hiddenWord = hiddenWord.Substring(0, indexLetter) + pressedLetter + hiddenWord.Substring(indexLetter + 1);
                    Debug.Log(chosenWord);

                    indexLetter = chosenWord.IndexOf(pressedLetter, indexLetter + 1, StringComparison.InvariantCultureIgnoreCase);
                    soundManager.PlaySound("trueLetter");
                }
                wordFindText.text = hiddenWord;
            }
            // FAULT CASE
            else
            {
                // ADD MAN PARTS ACCORDING TO OUR FAULT COUNT
                manParts[faults].SetActive(true);
                soundManager.PlaySound("wrongLetter");
                faults++;
                // ADD WRONG LETTER TO GAME SCENE
                wrongLetter = pressedLetter;
                wrongLettersText.text += "[" + wrongLetter + "] ";

            }
            // LOSE CASE
            if (faults == manParts.Length)
            {
                loseText.SetActive(true);
                gameEnd = true;
                soundManager.PlaySound("wrongCount");
                falseImage.SetActive(true);

                // ADD FALSE COUNT
                falseCount++;
                falseCountText.text = falseCount.ToString();
                wordFindText.text = chosenWord;
            }
            // WIN CASE
            if (!hiddenWord.Contains("_"))
            {
                winText.SetActive(true);
                gameEnd = true;
                soundManager.PlaySound("trueCount");
                trueImage.SetActive(true);

                // ADD TRUE COUNT
                trueCount++;
                trueCountText.text = trueCount.ToString();
            }
        }

    }
    void SetTime()
    {
        // TIME ELAPSED
        timer += Time.deltaTime;
        minute = (int)(timer / 60);
        second = (int)(timer % 60);
        timerText.text = string.Format("{00:00}:{1:00}", minute, second);
    }
    public void NewWord()
    {
        // SELECT RANDOM WORD TYPE
        int indexWordType = Random.Range(0, wordTypes.Length);

        // ASSIGN RANDOM WORD TYPE TO WORDS ARRAY
        words = File.ReadAllLines(wordTypes[indexWordType]);

        // SELECT RANDOM WORD
        int index = Random.Range(0, words.Length);
        string word = words[index];

        // TAKE WORD TYPE'S TEXT FILE NAME
        hint = Path.GetFileNameWithoutExtension(wordTypes[indexWordType]);
        hintText.text = hint;

        hiddenWord = "";
        chosenWord = "";
        Debug.Log(word);

        //PUT UNDERSCORES FOR ALL LETTERS (CREATE HIDDEN WORD)
        for (int i = 0; i < word.Length; i++)
        {
            char letter = word[i];
            chosenWord += letter + " ";
            if (char.IsWhiteSpace(letter))
            {
                hiddenWord += "  ";
            }
            else
            {
                hiddenWord += "_ ";
            }
        }
        // CHANGE TO TEXT
        wordFindText.text = hiddenWord;
    }
    public void SetNewGame()
    {
        //RESET WORDS - BODY PARTS - FAULT COUNT - WRONG LETTERS
        chosenWord = "";
        hiddenWord = "";
        gameEnd = false;

        foreach (var parts in manParts)
        {
            parts.SetActive(false);
        }
        faults = 0;
        wrongLetter = "";
        wrongLettersText.text = "WRONG LETTERS  ------------";
        winText.SetActive(false);
        loseText.SetActive(false);
        NewWord();
    }
    public void QuitGame()
    {
        // QUIT GAME
        Application.Quit();
    }
}
