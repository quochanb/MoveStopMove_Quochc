using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assign this script to the indicator prefabs.
/// </summary>
public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image scoreImage;
    [SerializeField] private CanvasGroup canvasGroup;
    private Image indicatorImage;
    private Text distanceText;

    /// <summary>
    /// Gets if the game object is active in hierarchy.
    /// </summary>
    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

    /// <summary>
    /// Gets the indicator type
    /// </summary>
    public IndicatorType Type
    {
        get
        {
            return indicatorType;
        }
    }

    void Awake()
    {
        indicatorImage = transform.GetComponent<Image>();
        distanceText = transform.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Sets the image color for the indicator.
    /// </summary>
    /// <param name="color"></param>
    public void SetImageColor(Color color)
    {
        indicatorImage.color = color;
        if (scoreImage != null)
        {
            scoreImage.color = color;
        }
    }

    /// <summary>
    /// Sets the distance text for the indicator.
    /// </summary>
    /// <param name="value"></param>
    public void SetDistanceText(float value)
    {
        distanceText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
    }

    /// <summary>
    /// Sets the distance text rotation of the indicator.
    /// </summary>
    /// <param name="rotation"></param>
    public void SetTextRotation(Quaternion rotation)
    {
        distanceText.rectTransform.rotation = rotation;
        if (scoreImage != null)
        {
            scoreImage.rectTransform.rotation = rotation;
        }
    }

    /// <summary>
    /// Sets the indicator as active or inactive.
    /// </summary>
    /// <param name="value"></param>
    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }

    public void SetScoreText(int value)
    {
        if (scoreText != null)
        {
            scoreText.text = value.ToString();
        }
    }

    public void SetNameText(string value, Color color)
    {
        if(nameText != null)
        {
            nameText.text = value;
            nameText.color = color;
        }
    }

    public void SetAlpha(float value)
    {
        canvasGroup.alpha = value;
    }
}

public enum IndicatorType
{
    BOX,
    ARROW
}
