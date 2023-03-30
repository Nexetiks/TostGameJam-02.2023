using UnityEngine;

[CreateAssetMenu(fileName = "Sfxs", menuName = "Audio/Sfxs", order = 0)]
public class Sfxs : ScriptableObject
{
    [field: SerializeField]
    public SfxData Hum { get; private set; }
    [field: SerializeField]
    public SfxData Bells { get; private set; }
    [field: SerializeField]
    public SfxData Pop { get; private set; }
    [field: SerializeField]
    public SfxData Rock { get; private set; }
    [field: SerializeField]
    public SfxData Slap { get; private set; }
    [field: SerializeField]
    public SfxData Pickup { get; private set; }
    [field: SerializeField]
    public SfxData Shoot { get; private set; }
    [field: SerializeField]
    public SfxData Impact { get; private set; }
}
