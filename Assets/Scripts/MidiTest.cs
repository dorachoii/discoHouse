using UnityEngine;
using MidiJack;

public class MidiTest : MonoBehaviour
{
    void Start()
    {
        // MIDI 이벤트 등록
        MidiMaster.noteOnDelegate += OnNoteOn;
        MidiMaster.noteOffDelegate += OnNoteOff;
        MidiMaster.knobDelegate += OnKnob;
        
        Debug.Log("MIDI Test 스크립트가 시작되었습니다. MIDI 입력을 기다리는 중...");
    }
    
    void OnDestroy()
    {
        // MIDI 이벤트 해제
        MidiMaster.noteOnDelegate -= OnNoteOn;
        MidiMaster.noteOffDelegate -= OnNoteOff;
    }
    
    void OnNoteOn(MidiChannel channel, int note, float velocity)
    {
        string noteName = GetNoteName(note);
        Debug.Log($"🎹 MIDI Note ON: {noteName} ({note}) - Velocity: {velocity:F2} - Channel: {channel}");
    }
    
    void OnNoteOff(MidiChannel channel, int note)
    {
        string noteName = GetNoteName(note);
        Debug.Log($"🎹 MIDI Note OFF: {noteName} ({note}) - Channel: {channel}");
    }
    
    
    string GetNoteName(int noteNumber)
    {
        string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        int octave = (noteNumber / 12) - 1;
        int note = noteNumber % 12;
        return $"{noteNames[note]}{octave}";
    }
    

}
