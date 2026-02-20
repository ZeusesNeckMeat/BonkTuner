namespace BonkTuner.Configs.EnemySpawnModifiers;
internal class NormalSpawnModifiers : SpawnModifiersBase
{
    public override EnemySpawnPreset Preset => EnemySpawnPreset.Normal;
    public override string Description => "Vanilla game behavior";
    public override float TargetSpawnsMultiplier => 1;
    public override float CreditMultiplier => 1;
    public override float SpawnIntervalMultiplier => 1;
}