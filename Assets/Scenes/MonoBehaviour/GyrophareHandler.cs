using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class GyrophareHandler : MonoBehaviour
{   
    // pour activer le pick des couleurs en HDR dans l'interface
    // https://unitycoder.com/blog/2019/05/25/enable-hdr-color-picker-for-shader-or-script/
    PostProcessVolume m_Volume;
    ColorGrading m_color_grading;

    [ColorUsage(true, true)]
    public Color lower_filter_color;
    [ColorUsage(true, true)]
    public Color upper_filter_color;
    public float coef_vitesse;
    public bool enable = false;

    void Start()
    {
        m_color_grading = ScriptableObject.CreateInstance<ColorGrading>();
        m_color_grading.enabled.Override(true);
        // m_color_grading.intensity.Override(1f);
        // m_color_grading.
        m_color_grading.colorFilter.Override(lower_filter_color);
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_color_grading);
    }

    void Update()
    {
        // m_color_grading.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);
        if (enable)
            m_color_grading.colorFilter.Interp(
                lower_filter_color,
                upper_filter_color,
            Mathf.Sin(Time.realtimeSinceStartup * coef_vitesse));

    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}
