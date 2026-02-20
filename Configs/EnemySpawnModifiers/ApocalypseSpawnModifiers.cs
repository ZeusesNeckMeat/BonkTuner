namespace BonkTuner.Configs.EnemySpawnModifiers;
internal class ApocalypseSpawnModifiers : SpawnModifiersBase
{
    public override EnemySpawnPreset Preset => EnemySpawnPreset.Apocalypse;
    public override string Description => "Absolute chaos - Everything maxed out";
    public override float TargetSpawnsMultiplier => 4;
    public override float CreditMultiplier => 2.5f;
    public override float SpawnIntervalMultiplier => 0.5f;
}