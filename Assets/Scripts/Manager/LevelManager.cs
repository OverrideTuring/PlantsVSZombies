using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Normal,
    Playing,
    Pausing
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    private Vector3 centerPosition = new Vector3(0, 0, -10);
    private Vector3 rightShiftedCameraPosition;
    private Vector3 leftShiftedCameraPosition;
    [Header("Camera Settings")]
    [SerializeField] private Transform rightShiftedCameraTransform;
    [SerializeField] private Transform leftShiftedCameraTransform;
    [Header("UI Settings")]
    [SerializeField] private Transform canvas;
    [SerializeField] private HouseOwnerTextUI houseOwnerTextUIPrefab;
    [SerializeField] private StartTextUI startTextUIPrefab;
    [SerializeField] private GameObject hugeWaveUIPrefab;
    [SerializeField] private GameObject lastWaveUIPrefab;
    [SerializeField] private GameObject zombieWonUIPrefab;
    [SerializeField] private WinGameObjectUI winGameObjectUIPrefab;
    [SerializeField] private AwardScreenController awardScreenPrefab;
    [SerializeField] private CardListBackground cardListBackground;
    [Header("Sun Spawner Dedication")]
    [SerializeField] private SunSpawner sunSpawner;
    [Header("Level Data Settings")]
    [SerializeField] private GameProcessConfig gameProcessConfig;
    [SerializeField] private LevelData levelData;
    AsyncOperation asyncOp;
    private GameState gameState = GameState.Normal;
    public GameState CurrentGameState { get { return gameState; } }

    public LevelData LevelData { get { return levelData; } }

    private void Awake()
    {
        Instance = this;
        rightShiftedCameraPosition = rightShiftedCameraTransform.position;
        leftShiftedCameraPosition = leftShiftedCameraTransform.position;
        if (levelData == null)
        {
            levelData = gameProcessConfig.GetLevelData(PlayerData.CurrentLevelIndex);
        }
    }

    private void Start()
    {
        GameEventManager.Instance.OnLevelStart += HandleLevelStart;
        GameEventManager.Instance.OnWaveEnd += HandleWaveEnd;
        GameEventManager.Instance.OnZombieGetIn += HandleZombieGetIn;
        GameEventManager.Instance.OnWinGame += HandleWinGame;
        StartLevel();
    }

    public void ToggleGameState()
    {
        switch (gameState)
        {
            case GameState.Normal:
                break;
            case GameState.Playing:
                PauseManager.Instance.Pause();
                UIManager.Instance.ShowMenuDialog();
                break;
            case GameState.Pausing:
                PauseManager.Instance.Resume();
                UIManager.Instance.CancelMenuDialog();
                break;
        }
    }

    public void ChangeGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    private void OnDisable()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnLevelStart -= HandleLevelStart;
            GameEventManager.Instance.OnWaveEnd -= HandleWaveEnd;
            GameEventManager.Instance.OnZombieGetIn -= HandleZombieGetIn;
            GameEventManager.Instance.OnWinGame -= HandleWinGame;
        }
    }

    public async void StartLevel()
    {
        ChangeGameState(GameState.Normal);

        AudioManager.Instance.PlayMusic(AudioConfig.BGM3, true, 0.9f);
        HouseOwnerTextUI houseOwnerTextUI = Instantiate(houseOwnerTextUIPrefab, canvas);
        await Task.Delay(1000);

        DOVirtual.DelayedCall(0.8f, houseOwnerTextUI.Disappear);
        await RightShiftCamera();

        // await SelectCards();
        await Task.Delay(1000);

        await ShiftCameraBack();

        cardListBackground.MoveDown();
        cardListBackground.DisableCards();
        await Task.Delay(1000);

        AudioManager.Instance.PlaySound2D(AudioConfig.READY_SET_PLANT);
        AudioManager.Instance.FadeOut(1.5f);
        Instantiate(startTextUIPrefab, canvas);
    }

    private Task RightShiftCamera()
    {
        return Camera.main.transform.DOMove(
            rightShiftedCameraPosition,
            2.5f).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }

    private Task ShiftCameraBack()
    {
        return Camera.main.transform.DOMove(
            centerPosition,
            2.0f).AsyncWaitForCompletion();
    }

    private Task LeftShiftCamera()
    {
        return Camera.main.transform.DOMove(
            leftShiftedCameraPosition,
            2.0f).AsyncWaitForCompletion();
    }

    private async Task SelectCards()
    {
        cardListBackground.MoveDown();
        // TODO: 选择卡片
        await Task.Delay(1000);
        cardListBackground.DisableCards();
    }

    private void HandleLevelStart()
    {
        ChangeGameState(GameState.Playing);
        AudioManager.Instance.PlayMusic(AudioConfig.BGM1);
        cardListBackground.EnableCards();
        sunSpawner.gameObject.SetActive(true);
        ZombieManager.Instance.StartSpawnZombies();
    }

    private async void HandleWaveEnd()
    {
        float duration = AudioManager.Instance.PlaySound2D(AudioConfig.HUGE_WAVE);
        GameObject hugeWaveUI =  Instantiate(hugeWaveUIPrefab, canvas);
        await PausableTask.Delay((int)(duration * 1000));
        Destroy(hugeWaveUI);
        await PausableTask.Delay(500);
        AudioManager.Instance.PlaySound2D(AudioConfig.SIREN);
        ZombieManager.Instance.StartSpawnZombies();
        if (ZombieManager.Instance.ReachLastWave())
        {
            GameObject lastWaveUI = Instantiate(lastWaveUIPrefab, canvas);
            duration = AudioManager.Instance.PlaySound2D(AudioConfig.LAST_WAVE);
            Destroy(lastWaveUI, duration);
        }
    }

    private async void HandleZombieGetIn(Zombie zombie)
    {
        // 停止游戏进行
        PauseManager.Instance.Unregister(zombie);
        PauseManager.Instance.Pause();
        ChangeGameState(GameState.Normal);
        cardListBackground.DisableCards();
        sunSpawner.gameObject.SetActive(false);
        ZombieManager.Instance.EndSpawnZombies();
        zombie.transform.DOMoveY(-0.21f, 1.0f);
        AudioManager.Instance.PlaySound2D(AudioConfig.LOSE_MUSIC);
        await Task.Delay(1800);

        // 镜头左移、播放音效并显示游戏失败UI
        await LeftShiftCamera();
        float duration;
        for(int i = 0; i < 2; i++)
        {
            duration = AudioManager.Instance.PlaySound2D(AudioConfig.GetRandomChomp());
            await Task.Delay(500);
        }
        duration = AudioManager.Instance.PlaySound2D(AudioConfig.SCREAM);
        Instantiate(zombieWonUIPrefab, canvas);

        // 一段时间后返回主菜单
        await Task.Delay((int)(duration * 1000));
        SceneManager.LoadScene(SceneConfig.MENU_SCENE);
    }

    private void HandleWinGame(Zombie zombie)
    {
        WinGameObjectUI winGameObjectUI = Instantiate(winGameObjectUIPrefab, canvas);
        winGameObjectUI.SetData(LevelData.winObjectData);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(zombie.transform.position);
        winGameObjectUI.transform.position = screenPos;
        winGameObjectUI.FlyAround();
    }

    public async void OnWinGameObjectPickedUp(WinGameObjectUI winGameObjectUI)
    {
        if (winGameObjectUI.Type == WinObjectType.PlantCard)
        {
            PlayerData.AddPlant(winGameObjectUI.PlantType);
        }
        PlayerData.CurrentLevelIndex++;
        if (gameProcessConfig.GetLevelData(PlayerData.CurrentLevelIndex) == null)
        {
            PlayerData.CurrentLevelIndex--;
            asyncOp = SceneManager.LoadSceneAsync(SceneConfig.MENU_SCENE);
        }
        else
        {
            asyncOp = SceneManager.LoadSceneAsync(SceneConfig.GAME_SCENE);
        }
        asyncOp.allowSceneActivation = false;

        // TODO: 白光动画
        AudioManager.Instance.PlaySound2D(AudioConfig.LIGHT_FILL);
        await Task.Delay(3000);
        Instantiate(awardScreenPrefab, canvas);
    }

    public void EnterNextLevel()
    {
        PoolManager.Instance.Clear();
        asyncOp.allowSceneActivation = true;
    }

    public void RestartLevel()
    {
        PoolManager.Instance.Clear();
        SceneManager.LoadScene(SceneConfig.GAME_SCENE);
    }

    public void ExitLevel()
    {
        PoolManager.Instance.Clear();
        SceneManager.LoadScene(SceneConfig.MENU_SCENE);
    }
}
