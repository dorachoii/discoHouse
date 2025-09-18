using UnityEngine;
using MidiJack;

public class MidiTest : MonoBehaviour
{
    [Header("Window References")]
    public WindowSpawner windowSpawner;
    
    // MIDI 노트 매핑 (4x4 그리드)
    // 48 49 50 51
    // 44 45 46 47  
    // 40 41 42 43
    // 36 37 38 39
    private const int GRID_SIZE = 4;
    private const int START_NOTE = 36; // 좌하단 시작 노트
    
    void Start()
    {
        // WindowSpawner 자동 찾기
        if (windowSpawner == null)
        {
            windowSpawner = FindObjectOfType<WindowSpawner>();
        }
        
        // MIDI 이벤트 등록
        MidiMaster.noteOnDelegate += OnNoteOn;
        MidiMaster.noteOffDelegate += OnNoteOff;
        
        Debug.Log("MIDI Window Controller 시작! 4x4 그리드 매핑 완료");
    }
    
    void OnDestroy()
    {
        // MIDI 이벤트 해제
        MidiMaster.noteOnDelegate -= OnNoteOn;
        MidiMaster.noteOffDelegate -= OnNoteOff;
    }
    
    void OnNoteOn(MidiChannel channel, int note, float velocity)
    {
        Vector2Int windowPos = GetWindowPosition(note);
        
        if (IsValidPosition(windowPos))
        {
            Debug.Log($"🎹 MIDI Note ON: {note} -> Window[{windowPos.x},{windowPos.y}]");
            OpenWindow(windowPos.x, windowPos.y);
        }
        else
        {
            Debug.Log($"🎹 MIDI Note ON: {note} (매핑되지 않은 노트)");
        }
    }
    
    void OnNoteOff(MidiChannel channel, int note)
    {
        Vector2Int windowPos = GetWindowPosition(note);
        
        if (IsValidPosition(windowPos))
        {
            Debug.Log($"🎹 MIDI Note OFF: {note} -> Window[{windowPos.x},{windowPos.y}]");
        }
    }
    
    Vector2Int GetWindowPosition(int midiNote)
    {
        // 규칙: 36 + (row * 4) + col (좌하단부터 시작)
        // 노트 범위 체크: 36 ~ 51
        if (midiNote < START_NOTE || midiNote >= START_NOTE + GRID_SIZE * GRID_SIZE)
            return new Vector2Int(-1, -1);
        
        int relativeNote = midiNote - START_NOTE;
        int row = relativeNote / GRID_SIZE;
        int col = relativeNote % GRID_SIZE;
        
        // Y축 뒤집기: 0행(36~39)이 아래쪽, 3행(48~51)이 위쪽
        int flippedRow = GRID_SIZE - 1 - row;
        
        return new Vector2Int(col, flippedRow);
    }
    
    bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < GRID_SIZE && pos.y >= 0 && pos.y < GRID_SIZE;
    }
    
    void OpenWindow(int x, int y)
    {
        if (!IsValidPosition(new Vector2Int(x, y)))
        {
            Debug.LogWarning($"❌ 잘못된 창문 위치: [{x},{y}]");
            return;
        }
        
        if (windowSpawner?.windows == null)
        {
            Debug.LogWarning("❌ WindowSpawner 또는 windows 배열을 찾을 수 없습니다!");
            return;
        }
        
        GameObject window = windowSpawner.windows[x, y];
        if (window?.GetComponent<WindowController>() is WindowController controller)
        {
            controller.OpenWindow();
            Debug.Log($"✅ 창문 [{x},{y}] 열기!");
        }
        else
        {
            Debug.LogWarning($"❌ 창문 또는 WindowController를 찾을 수 없습니다: [{x},{y}]");
        }
    }
}
