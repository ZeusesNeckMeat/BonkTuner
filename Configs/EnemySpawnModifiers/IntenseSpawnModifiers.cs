namespace BonkTuner.Configs.EnemySpawnModifiers;
internal class IntenseSpawnModifiers : SpawnModifiersBase
{
    public override EnemySpawnPreset Preset => EnemySpawnPreset.Intense;
    public override string Description => "Challenging - More frequent waves with more enemies";
    public override float TargetSpawnsMultiplier => 1.5f;
    public override float CreditMultiplier => 1.5f;
    public override float SpawnIntervalMultiplier => 0.75f;
}