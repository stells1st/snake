using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject PausePanel;
    public Button ResumeButton;
    public Button SwitchButton;
    public Button EndGameRestartButton;
    public Button EndGameExitButton;
    public Slider SpeedControl;
    public Snake Snake;
    public Sprite[] SwitchSprites = new Sprite[2];
    public Slider SnakeSizeSlider;
    public TMP_Text SnakeSizeCounterText;
    public TMP_Text SnakeSizeText;
    public TMP_Text RecordSnakeSizeText;
    public GameObject EndMenu;
    


    private int _recordSnakeSize;
    private void Start()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        ResumeButton.onClick.AddListener(ResumeGame);
        SwitchButton.onClick.AddListener(SwitchWalls);
        SwitchButton.image.sprite = SwitchSprites[0];
        EndGameRestartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        EndGameExitButton.onClick.AddListener(() => Application.Quit(0));

        Snake.CanCrossWall = true;
        SpeedControl.value = Snake.SpeedMultiplier;
        SpeedControl.onValueChanged.AddListener(RenderSnakeSpeed);
        SnakeSizeSlider.value = Snake.InitialSize;
        SnakeSizeSlider.onValueChanged.AddListener(AdjustSnakeSize);
        AdjustSnakeSize(Snake.InitialSize);

 
    }


    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }


        if(Snake.InitialSize > _recordSnakeSize)
        {
            _recordSnakeSize = Snake.InitialSize;
            SaveSnakeSize();
        }

        SnakeSizeText.text = $"Size: {Snake.InitialSize}";
        RecordSnakeSizeText.text = $"Record Size: {_recordSnakeSize}";
        LoadSnakeSize();
    }

    private void RenderSnakeSpeed(float value)
    {
        Snake.SpeedMultiplier = value;
    }
    private void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void SwitchWalls()
    {
        Snake.CanCrossWall = !Snake.CanCrossWall;
        if (Snake.CanCrossWall)
        {
            SwitchButton.image.sprite = SwitchSprites[0];
        }
        else
        {
            SwitchButton.image.sprite = SwitchSprites[1];
        }
    }

    private void PauseGame()
    {
        PausePanel.SetActive(!PausePanel.activeSelf);
        if (PausePanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    private void AdjustSnakeSize(float size)
    {
        Snake.InitialSize = (int)size;
        SnakeSizeCounterText.text = ((int)(size)).ToString();
    }
    private void SaveSnakeSize()
    {
        PlayerPrefs.SetInt("record",_recordSnakeSize);
        PlayerPrefs.Save();
    }
    public void LoadSnakeSize()
    {
        _recordSnakeSize = PlayerPrefs.GetInt("record", 0);
    }
    public void ActiveEndMenu()
    {
        EndMenu.SetActive(true);
        Snake.gameObject.transform.position = Vector3.zero;
        Snake.Speed = 0f;
    }
}
