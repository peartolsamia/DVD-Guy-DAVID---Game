using UnityEngine;
using UnityEngine.UIElements;

public class SkillPointUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private const string LABEL_NAME = "SkillPointLabel";

    private Label skillPointLabel;

    private void Awake()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        skillPointLabel = uiDocument.rootVisualElement.Q<Label>(LABEL_NAME);
    }

    private void Start()
    {
        // Player.Instance is not guaranteed to be ready in OnEnable (script execution order between different objects is not guaranteed),
        // so we subscribe here instead - all objects' Awake calls are guaranteed to finish before any object's Start runs,
        // so Player.Instance is guaranteed to be ready at this point.
        if (Player.Instance != null && Player.Instance.SkillPoints != null)
        {
            Player.Instance.SkillPoints.OnSkillPointsChanged += UpdateSkillPointText;
        }
        else
        {
            Debug.LogWarning("SkillPointUI: Player.Instance or SkillPoints still not ready in Start!");
        }

        UpdateSkillPointText();
    }



    private void OnDisable()
    {
        if (Player.Instance != null && Player.Instance.SkillPoints != null)
        {
            Player.Instance.SkillPoints.OnSkillPointsChanged -= UpdateSkillPointText;
        }
    }

    private void UpdateSkillPointText()
    {

        if (skillPointLabel == null)
        {
            Debug.LogError("SkillPointLabel couldnt found! Check UXML name.");
            return;
        }

        if (Player.Instance == null || Player.Instance.SkillPoints == null)
        {
            Debug.LogWarning("Player.Instance or SkillPoints is not ready yet!");
            return;
        }


        skillPointLabel.text = $"Skill Points: {Player.Instance.SkillPoints.Current}";
        //Debug.Log($"[UI] Updated: {skillPointLabel.text}");
    }
}