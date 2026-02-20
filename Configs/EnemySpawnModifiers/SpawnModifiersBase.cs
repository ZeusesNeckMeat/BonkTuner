namespace BonkTuner.Configs.EnemySpawnModifiers;
internal abstract class SpawnModifiersBase
{
    public abstract EnemySpawnPreset Preset { get; }
    public abstract string Description { get; }
    public abstract float TargetSpawnsMultiplier { get; }
    public abstract float CreditMultiplier { get; }
    public abstract float SpawnIntervalMultiplier { get; }
}