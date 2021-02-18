using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class GyrophareHandler : MonoBehaviour
{
    // pour activer le pick des couleurs en HDR dans l'interface
    // https://unitycoder.com/blog/2019/05/25/enable-hdr-color-picker-for-shader-or-script/
    // et pour utiliser le post processing dans un script
    // https://github.com/Unity-Technologies/PostProcessing/wiki/Manipulating-the-Stack
    PostProcessVolume m_Volume;
    public float coef_vitesse;

    // Color Grading
    ColorGrading m_color_grading;
    [ColorUsage(true, true)]
    public Color lower_filter_color;
    [ColorUsage(true, true)]
    public Color upper_filter_color;

    // Vignette
    public float lower_intensity;
    public float upper_intensity;
    Vignette m_vignette;

    void Awake()
    {
        m_color_grading = ScriptableObject.CreateInstance<ColorGrading>();
        m_vignette = ScriptableObject.CreateInstance<Vignette>();

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_color_grading);
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_vignette);
    }

    void Update()
    {
        float t = Mathf.Sin(Time.realtimeSinceStartup * coef_vitesse);

        m_color_grading.colorFilter.value = Color.Lerp(
            lower_filter_color,
            upper_filter_color,
            t);

        m_vignette.intensity.value = Mathf.Lerp(
            lower_intensity,
            upper_intensity,
            t);
    }

    void OnEnable()
    {
        m_color_grading.enabled.Override(true);
        m_vignette.enabled.Override(true);
        m_color_grading.colorFilter.Override(lower_filter_color);
        m_vignette.intensity.Override(lower_intensity);
    }

    void OnDisable()
    {
        m_color_grading.enabled.Override(false);
        m_vignette.enabled.Override(false);
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}
