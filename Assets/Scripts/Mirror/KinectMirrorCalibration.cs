using UnityEngine;
using UnityEngine.UI;
using nuitrack;

/// <summary>
/// KinectMirrorCalibration
///
/// Captures the Azure Kinect color stream via Nuitrack and displays it
/// full-screen as a horizontal mirror (like a real mirror).
///
/// SETUP in Unity:
///   1. Create a Canvas (Screen Space - Overlay, or Camera).
///   2. Add a RawImage child that covers the whole canvas.
///   3. Attach this script to any GameObject (e.g. "MirrorManager").
///   4. Drag the RawImage into the 'Mirror Display' field.
///   5. Make sure NuitrackManager is present in the scene.
/// </summary>
public class KinectMirrorCalibration : MonoBehaviour
{
    [Header("Display")]
    [Tooltip("The RawImage that will show the Kinect color feed.")]
    [SerializeField] private RawImage mirrorDisplay;

    [Header("Mirror")]
    [Tooltip("Flip horizontally so the image behaves like a real mirror.")]
    [SerializeField] private bool flipHorizontal = true;

    [Header("Calibration (adjust at runtime via SetOffset / SetScale)")]
    [SerializeField] private float offsetX = 0f;
    [SerializeField] private float offsetY = 0f;
    [SerializeField] private float scaleX = 1f;
    [SerializeField] private float scaleY = 1f;

    private Texture2D colorTexture;
    private bool initialized = false;

    void OnEnable()
    {
        NuitrackManager.onColorUpdate += OnColorFrame;
    }

    void OnDisable()
    {
        NuitrackManager.onColorUpdate -= OnColorFrame;
    }

    void Start()
    {
        ApplyCalibration();
    }

    private void OnColorFrame(ColorFrame frame)
    {
        if (!initialized)
        {
            colorTexture = new Texture2D(frame.Cols, frame.Rows, TextureFormat.RGB24, false);
            initialized = true;

            if (mirrorDisplay != null)
                mirrorDisplay.texture = colorTexture;

            FitToCanvas();
        }

        colorTexture.LoadRawTextureData(frame.Data);
        colorTexture.Apply();
    }

    public void ApplyCalibration()
    {
        if (mirrorDisplay == null) return;

        RectTransform rt = mirrorDisplay.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot     = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(offsetX, offsetY);

        float signX = flipHorizontal ? -1f : 1f;
        rt.localScale = new Vector3(signX * scaleX, scaleY, 1f);
    }

    public void FitToCanvas()
    {
        if (mirrorDisplay == null || colorTexture == null) return;

        RectTransform parentRT = mirrorDisplay.transform.parent
                                 ?.GetComponent<RectTransform>();
        if (parentRT == null) return;

        float canvasW = parentRT.rect.width;
        float canvasH = parentRT.rect.height;
        if (canvasW <= 0 || canvasH <= 0) return;

        float canvasAspect  = canvasW / canvasH;
        float textureAspect = (float)colorTexture.width / colorTexture.height;

        if (canvasAspect >= textureAspect)
            mirrorDisplay.rectTransform.sizeDelta = new Vector2(canvasH * textureAspect, canvasH);
        else
            mirrorDisplay.rectTransform.sizeDelta = new Vector2(canvasW, canvasW / textureAspect);

        offsetX = 0f; offsetY = 0f;
        scaleX  = 1f; scaleY  = 1f;
        ApplyCalibration();
    }

    public void SetOffset(float x, float y) { offsetX = x; offsetY = y; ApplyCalibration(); }
    public void SetScale(float sx, float sy) { scaleX = sx; scaleY = sy; ApplyCalibration(); }
    public void SetFlip(bool enable)         { flipHorizontal = enable; ApplyCalibration(); }
    public bool IsInitialized => initialized;
}

