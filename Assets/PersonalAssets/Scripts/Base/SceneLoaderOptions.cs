using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoaderOptions", menuName = "Scriptable Objects/SceneLoaderOptions")]
public class SceneLoaderOptions : ScriptableObject
{
    public string TargetSceneName;

    [Tooltip("Подсказки во время загрузки")]
    public string[] Tips;

    [Tooltip("Начальное качество сцены")]
    [Range(0, 4)]
    public int StartQualityLevel;

    [SerializeField]
    public Sprite[] Backgrounds;

    public int backgroundFadeDuration = 2;
    public int backgroundLifeDuration = 5;
}
