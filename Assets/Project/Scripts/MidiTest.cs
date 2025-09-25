using UnityEngine;
using MidiJack;
using UnityEngine.VFX;
using System.Collections;

public class MidiTest : MonoBehaviour
{
    [Header("Window References")]
    public WindowSpawner windowSpawner;
    public VisualEffect visualEffect;
    
    [Header("VFX Settings")]
    public bool enableVFX = true;
    float forceStrength = 100f;
    
    private Coroutine stopCoroutine;

    
    // MIDI 노트 매핑 (4x4 그리드)
    // 48 49 50 51
    // 44 45 46 47  
    // 40 41 42 43
    // 36 37 38 39
    private const int GRID_SIZE = 4;
    private const int START_NOTE = 36; // 좌하단 시작 노트
    
    void Start()
    {
        // MIDI 이벤트 등록
        MidiMaster.noteOnDelegate += OnNoteOn;
    }
    
    void OnDestroy()
    {
        // MIDI 이벤트 해제
        MidiMaster.noteOnDelegate -= OnNoteOn;

    }
    
    void OnNoteOn(MidiChannel channel, int note, float velocity)
    {
        Vector2Int windowPos = GetWindowPosition(note);
        
        if (IsValidPosition(windowPos))
        {
            OpenWindow(windowPos.x, windowPos.y);
            
            // VFX 힘 적용
            if (enableVFX)
            {
                ApplyVFXForce(windowPos.x, windowPos.y, 0);
            }
        }
    }



    Vector2Int GetWindowPosition(int midiNote)
    {

        if (midiNote < START_NOTE || midiNote >= START_NOTE + GRID_SIZE * GRID_SIZE)
            return new Vector2Int(-1, -1);
        
        int relativeNote = midiNote - START_NOTE;
        int row = relativeNote / GRID_SIZE;
        int col = relativeNote % GRID_SIZE;
        
        int flippedRow = GRID_SIZE - 1 - row;
        
        return new Vector2Int(col, flippedRow);
    }
    
    bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < GRID_SIZE && pos.y >= 0 && pos.y < GRID_SIZE;
    }
    
    void OpenWindow(int x, int y)
    {
        if (!IsValidPosition(new Vector2Int(x, y))) return;
        
        if (windowSpawner?.windows == null) return;
        
        GameObject window = windowSpawner.windows[x, y];
        if (window?.GetComponent<WindowController>() is WindowController controller)
        {
            controller.OpenWindow();
        }
    }
    
    void ApplyVFXForce(int x, int y, float velocity)
    {
        if (visualEffect == null) return;
        
        // 창문 위치 가져오기
        if (windowSpawner?.windows == null || !IsValidPosition(new Vector2Int(x, y)))
            return;
        
        GameObject window = windowSpawner.windows[x, y];
        if (window == null) return;
        

        Vector3 windowPosition = window.transform.position;
        Vector3 vfxPosition = visualEffect.transform.position;
        Vector3 forceDirection = (windowPosition - vfxPosition);
        
        visualEffect.SetVector3("ThrowPos", forceDirection * forceStrength);
        

        if (stopCoroutine != null)
        {
            StopCoroutine(stopCoroutine);
        }
        
        visualEffect.Play();
        Debug.Log("VFX Play");

        stopCoroutine = StartCoroutine(StopVFXAfterDelay(2.0f));
    }
    
    IEnumerator StopVFXAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (visualEffect != null)
        {
            visualEffect.Stop();
            Debug.Log("VFX Stopped");
        }
        stopCoroutine = null; 
    }
}
