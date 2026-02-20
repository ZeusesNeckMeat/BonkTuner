namespace BonkTuner.Configs.EnemySpawnModifiers;
internal class CasualSpawnModifiers : SpawnModifiersBase
{
    public override EnemySpawnPreset Preset => EnemySpawnPreset.Casual;
    public override string Description => "Relaxed pace - Fewer enemies, more breathing room";
    public override float TargetSpawnsMultiplier => 0.7f;
    public override float CreditMultiplier => 0.8f;
    public override float SpawnIntervalMultiplier => 1.3f;
}