using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] private float VignetteMinIntensity;
    [SerializeField] private float VignetteIntensityRange;
    private PostProcessVolume volume;
    private Vignette vignette;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        vignette = volume.profile.GetSetting<Vignette>();
    }

    // Update is called once per frame
    void Update()
    {
        vignette.intensity.value = VignetteMinIntensity + VignetteIntensityRange*GameManager.getTimeProcessing();
    }
}
