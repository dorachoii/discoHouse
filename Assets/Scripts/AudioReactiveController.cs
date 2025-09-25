using UnityEngine;

public class AudioReactiveController : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;
    
    [Header("Material References")]
    public Material audioReactiveMaterial;
    public Renderer targetRenderer;
    
    [Header("Audio Reactive Settings")]
    [SerializeField] private float baseScale = 1.0f;
    [SerializeField] private float scaleMultiplier = 10.0f;
    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private float threshold = 0.01f;
    
    
    [Header("Audio Analysis Settings")]
    [SerializeField] private int spectrumSize = 64;
    
    // Private variables
    private float[] spectrum;
    private float audioLevel = 0f;
    
    void Start()
    {
        // AudioSource 자동 찾기
        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
        }
        
        // Material 자동 찾기
        if (audioReactiveMaterial == null && targetRenderer != null)
        {
            audioReactiveMaterial = targetRenderer.material;
        }
        
        // 올바른 셰이더로 변경
        if (audioReactiveMaterial != null)
        {
            Shader audioReactiveShader = Shader.Find("Unlit/AudioReactive");
            if (audioReactiveShader != null)
            {
                audioReactiveMaterial.shader = audioReactiveShader;
                Debug.Log("Shader changed to AudioReactive");
            }
            else
            {
                Debug.LogError("AudioReactive shader not found!");
            }
        }
        
        // 스펙트럼 배열 초기화
        spectrum = new float[spectrumSize];
    }
    
    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            // 오디오 스펙트럼 분석
            AnalyzeAudio();
            
            // 오디오 레벨에 따른 스케일 계산
            float audioIntensity = Mathf.Clamp01(audioLevel * sensitivity);
            
            if (audioIntensity > threshold)
            {
                float audioScale = audioIntensity * scaleMultiplier;
                UpdateMaterial(1.0f, audioScale);
            }
            else
            {
                UpdateMaterial(1.0f, 0f);
            }
        }
        else
        {
            UpdateMaterial(1.0f, 0f);
        }
    }
    
    void AnalyzeAudio()
    {
        // 오디오 스펙트럼 데이터 가져오기
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        
        // 전체 오디오 레벨 계산 (RMS 방식)
        float sum = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            sum += spectrum[i] * spectrum[i];
        }
        float rmsLevel = Mathf.Sqrt(sum / spectrum.Length);
        
        // 저주파수 대역 강조 (베이스, 킥드럼 등)
        float bassLevel = 0f;
        int bassRange = Mathf.RoundToInt(spectrum.Length * 0.1f); // 하위 10% 주파수
        for (int i = 0; i < bassRange; i++)
        {
            bassLevel += spectrum[i];
        }
        bassLevel /= bassRange;
        
        // 베이스 레벨을 전체 오디오 레벨에 반영
        audioLevel = Mathf.Max(rmsLevel, bassLevel * 2f);
        
    }
    
    void UpdateMaterial(float scale, float audioSpectrum)
    {
        if (audioReactiveMaterial != null)
        {
            audioReactiveMaterial.SetFloat("_Scale", scale);
            audioReactiveMaterial.SetFloat("_AudioSpectrum", audioSpectrum);
        }
    }
    
}

