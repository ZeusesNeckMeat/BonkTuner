namespace BonkTuner.Configs.EnemySpawnModifiers;
internal class SwarmSpawnModifiers : SpawnModifiersBase
{
    public override EnemySpawnPreset Preset => EnemySpawnPreset.Swarm;
    public override string Description => "Constant pressure - Triple the enemies on screen";
    public override float TargetSpawnsMultiplier => 3;
    public override float CreditMultiplier => 1;
    public override float SpawnIntervalMultiplier => 1;
}