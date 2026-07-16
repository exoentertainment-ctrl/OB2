using UnityEngine;

[CreateAssetMenu(fileName = "UI Clips", menuName = "Audio SO/UI Clips")]
public class UIAudioClips : ScriptableObject
{
    public AudioClipSO selectSound;
    public AudioClipSO clickSound;
    public AudioClipSO closeSound;
    public AudioClipSO errorSound;
    public AudioClipSO insufficientCredits;
    public AudioClipSO endLevelSound;
    public AudioClipSO levelLostSound;
    public AudioClipSO specialWeaponSound;
}
