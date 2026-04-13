using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// MirrorCalibrationUI
///
/// Panneau de calibration runtime (afficher/masquer avec F1).
/// Connecter les sliders et boutons depuis l'Inspector — tout est optionnel.
/// </summary>
public class MirrorCalibrationUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private KinectMirrorCalibration mirrorCalibration;

    [Header("Sliders (optionnel)")]
    [SerializeField] private Slider offsetXSlider;  // -500 à 500
    [SerializeField] private Slider offsetYSlider;
    [SerializeField] private Slider scaleXSlider;   // 0.5 à 3
    [SerializeField] private Slider scaleYSlider;

    [Header("Labels TextMeshPro (optionnel)")]
    [SerializeField] private TMP_Text offsetXLabel;
    [SerializeField] private TMP_Text offsetYLabel;
    [SerializeField] private TMP_Text scaleXLabel;
    [SerializeField] private TMP_Text scaleYLabel;

    [Header("Boutons (optionnel)")]
    [SerializeField] private Button fitToCanvasButton;
    [SerializeField] private Button resetButton;

    [Header("Toggle miroir (optionnel)")]
    [SerializeField] private Toggle mirrorToggle;

    [Header("Panneau")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F1;
    [SerializeField] private GameObject calibrationPanel;

    private float cx, cy, sx = 1f, sy = 1f;

    void Start()
    {
        SetupSlider(offsetXSlider, -500f, 500f, 0f, v => { cx = v; UpdateLabel(offsetXLabel,"Offset X",v,"F0"); Apply(); });
        SetupSlider(offsetYSlider, -500f, 500f, 0f, v => { cy = v; UpdateLabel(offsetYLabel,"Offset Y",v,"F0"); Apply(); });
        SetupSlider(scaleXSlider,   0.5f,  3f,  1f, v => { sx = v; UpdateLabel(scaleXLabel, "Scale X", v,"F2"); Apply(); });
        SetupSlider(scaleYSlider,   0.5f,  3f,  1f, v => { sy = v; UpdateLabel(scaleYLabel, "Scale Y", v,"F2"); Apply(); });

        if (fitToCanvasButton != null)
            fitToCanvasButton.onClick.AddListener(() => { mirrorCalibration?.FitToCanvas(); ResetSliders(); });

        if (resetButton != null)
            resetButton.onClick.AddListener(() => { cx=cy=0; sx=sy=1; ResetSliders(); Apply(); });

        if (mirrorToggle != null)
        {
            mirrorToggle.isOn = true;
            mirrorToggle.onValueChanged.AddListener(v => mirrorCalibration?.SetFlip(v));
        }

        if (calibrationPanel != null)
            calibrationPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && calibrationPanel != null)
            calibrationPanel.SetActive(!calibrationPanel.activeSelf);
    }

    private void Apply() => mirrorCalibration?.SetOffset(cx, cy);

    private void ResetSliders()
    {
        if (offsetXSlider != null) offsetXSlider.value = 0f;
        if (offsetYSlider != null) offsetYSlider.value = 0f;
        if (scaleXSlider  != null) scaleXSlider.value  = 1f;
        if (scaleYSlider  != null) scaleYSlider.value  = 1f;
    }

    private static void SetupSlider(Slider s, float min, float max, float val,
                                     UnityEngine.Events.UnityAction<float> cb)
    {
        if (s == null) return;
        s.minValue = min; s.maxValue = max; s.value = val;
        s.onValueChanged.AddListener(cb);
    }

    private static void UpdateLabel(TMP_Text t, string prefix, float v, string fmt)
    {
        if (t != null) t.text = $"{prefix}: {v.ToString(fmt)}";
    }
}

