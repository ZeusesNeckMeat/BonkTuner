using Assets.Scripts.Game.Other;
using Assets.Scripts.Game.Spawning.New.Timelines;

using System.Collections.Generic;

using UnityEngine;

namespace BonkTuner.Services;

internal class MapDataService : ServiceBase
{
    private static readonly Dictionary<int, MapDataBackup> _originalMapData = new();
    private static readonly Dictionary<int, List<TimelineEventBackup>> _originalTimelineEvents = new();

    private class MapDataBackup
    {
        public float StageDuration { get; set; }
        public float NumChestsMultiplier { get; set; }
        public float NumShrinesMultiplier { get; set; }
        public float NumShrinesPotsAndOtherMultiplier { get; set; }
    }

    private class TimelineEventBackup
    {
        public int StageIndex { get; set; }
        public List<TimelineEvent> OriginalEvents { get; set; }
        public float OriginalStageTime { get; set; }
    }

    public static void TrySetMapDataOnNewMap(RunConfig newRunConfig)
    {
        if (!CanContinue())
            return;

        if (newRunConfig.mapData == null)
        {
            Main.Logger.LogWarning($"[{nameof(MapDataService)}.{nameof(TrySetMapDataOnNewMap)}] MapData is null for the new run config. Cannot apply map modifiers.");
            return;
        }

        var mapModifierConfig = ConfigService.GetMapModifiers(newRunConfig.mapData.eMap);

        // Always restore to original values first (in case config was disabled)
        RestoreOriginalMapData(newRunConfig);

        if (!mapModifierConfig?.IsEnabled ?? false)
        {
            Main.Logger.LogInfo($"[{nameof(MapDataService)}.{nameof(TrySetMapDataOnNewMap)}] Map modifiers disabled for {newRunConfig.mapData.eMap}, using original values");
            return;
        }

        // Backup original values before modification
        BackupOriginalMapData(newRunConfig);

        Main.Logger.LogInfo($"[{nameof(MapDataService)}.{nameof(TrySetMapDataOnNewMap)}] Applying map modifiers for {newRunConfig.mapData.eMap}");
        Main.Logger.LogInfo($" - Stage Duration Change | From {newRunConfig.mapData.stageDuration} to {mapModifierConfig.StageDurationSeconds}");
        Main.Logger.LogInfo($" - Chest Spawn Multiplier Change | From {newRunConfig.mapData.numChestsMultiplier} to {mapModifierConfig.ChestSpawnMultiplier}");
        Main.Logger.LogInfo($" - Shrine Only Spawn Multiplier Change | From {newRunConfig.mapData.numShrinesMultiplier} to {mapModifierConfig.ShrineOnlySpawnMultiplier}");
        Main.Logger.LogInfo($" - Shrine and Pot Spawn Multiplier Change | From {newRunConfig.mapData.numShrinesPotsAndOtherMultiplier} to {mapModifierConfig.ShrineAndPotSpawnMultiplier}");

        newRunConfig.mapData.stageDuration = mapModifierConfig.StageDurationSeconds;
        newRunConfig.mapData.numChestsMultiplier = mapModifierConfig.ChestSpawnMultiplier;
        newRunConfig.mapData.numShrinesMultiplier = mapModifierConfig.ShrineOnlySpawnMultiplier;
        newRunConfig.mapData.numShrinesPotsAndOtherMultiplier = mapModifierConfig.ShrineAndPotSpawnMultiplier;

        for (var i = 0; i < newRunConfig.mapData.stages.Count; i++)
        {
            var stage = newRunConfig.mapData.stages[i];
            UpdateTimelineEvents(stage.stageTimeline, mapModifierConfig.StageDurationSeconds, i);
        }
    }

    private static void BackupOriginalMapData(RunConfig newRunConfig)
    {
        var mapDataHash = newRunConfig.mapData.GetHashCode();

        // Only backup if we haven't already
        if (_originalMapData.ContainsKey(mapDataHash))
        {
            Main.Logger.LogDebug($"[{nameof(MapDataService)}.{nameof(BackupOriginalMapData)}] MapData already backed up");
            return;
        }

        Main.Logger.LogInfo($"[{nameof(MapDataService)}.{nameof(BackupOriginalMapData)}] Backing up original MapData values");

        _originalMapData[mapDataHash] = new MapDataBackup
        {
            StageDuration = newRunConfig.mapData.stageDuration,
            NumChestsMultiplier = newRunConfig.mapData.numChestsMultiplier,
            NumShrinesMultiplier = newRunConfig.mapData.numShrinesMultiplier,
            NumShrinesPotsAndOtherMultiplier = newRunConfig.mapData.numShrinesPotsAndOtherMultiplier
        };

        // Backup timeline events for each stage
        var timelineBackups = new List<TimelineEventBackup>();
        for (var i = 0; i < newRunConfig.mapData.stages.Count; i++)
        {
            var stage = newRunConfig.mapData.stages[i];
            if (stage?.stageTimeline?.events == null)
                continue;

            var originalEvents = new List<TimelineEvent>();
            for (var j = 0; j < stage.stageTimeline.events.Count; j++)
            {
                var timelineEvent = stage.stageTimeline.events[j];
                originalEvents.Add(new TimelineEvent
                {
                    eTimelineEvent = timelineEvent.eTimelineEvent,
                    duration = timelineEvent.duration,
                    enemies = timelineEvent.enemies,
                    timeMinutes = timelineEvent.timeMinutes
                });
            }

            timelineBackups.Add(new TimelineEventBackup
            {
                StageIndex = i,
                OriginalEvents = originalEvents,
                OriginalStageTime = stage.stageTimeline.stageTime
            });
        }

        _originalTimelineEvents[mapDataHash] = timelineBackups;
    }

    private static void RestoreOriginalMapData(RunConfig newRunConfig)
    {
        var mapDataHash = newRunConfig.mapData.GetHashCode();

        if (!_originalMapData.TryGetValue(mapDataHash, out var backup))
        {
            Main.Logger.LogDebug($"[{nameof(MapDataService)}.{nameof(RestoreOriginalMapData)}] No backup found for MapData");
            return;
        }

        Main.Logger.LogInfo($"[{nameof(MapDataService)}.{nameof(RestoreOriginalMapData)}] Restoring original MapData values");

        newRunConfig.mapData.stageDuration = backup.StageDuration;
        newRunConfig.mapData.numChestsMultiplier = backup.NumChestsMultiplier;
        newRunConfig.mapData.numShrinesMultiplier = backup.NumShrinesMultiplier;
        newRunConfig.mapData.numShrinesPotsAndOtherMultiplier = backup.NumShrinesPotsAndOtherMultiplier;

        // Restore timeline events
        if (_originalTimelineEvents.TryGetValue(mapDataHash, out var timelineBackups))
        {
            foreach (var timelineBackup in timelineBackups)
            {
                if (timelineBackup.StageIndex >= newRunConfig.mapData.stages.Count)
                    continue;

                var stage = newRunConfig.mapData.stages[timelineBackup.StageIndex];
                if (stage?.stageTimeline == null)
                    continue;

                stage.stageTimeline.events.Clear();
                foreach (var evt in timelineBackup.OriginalEvents)
                {
                    stage.stageTimeline.events.Add(evt);
                }

                stage.stageTimeline.stageTime = timelineBackup.OriginalStageTime;
            }
        }
    }

    private static void UpdateTimelineEvents(StageTimeline stageTimeline, float newDurationSeconds, int stageIndex)
    {
        if (stageTimeline == null || stageTimeline.WasCollected)
        {
            Main.Logger.LogWarning($"[{nameof(MapDataService)}.{nameof(UpdateTimelineEvents)}] StageTimeline is null for stage {stageIndex}. Cannot apply stage duration modifiers.");
            return;
        }

        if (stageTimeline.events == null || stageTimeline.events.Count == 0)
        {
            Main.Logger.LogWarning($"[{nameof(MapDataService)}.{nameof(UpdateTimelineEvents)}] StageTimeline events are null or empty for stage {stageIndex}. Cannot update timeline events for stage duration change.");
            return;
        }

        Main.Logger.LogInfo($"[{nameof(MapDataService)}.{nameof(UpdateTimelineEvents)}] Updating timeline events for stage {stageIndex} duration change to {newDurationSeconds} seconds.");

        stageTimeline.stageTime = newDurationSeconds;
        var newStageDurationMinutes = Mathf.RoundToInt(newDurationSeconds / 60);
        var firstOfEachEventType = new Dictionary<ETimelineEvent, TimelineEvent>();
        var lastEventTime = 0f;

        var events = stageTimeline.events;
        for (var i = 0; i < events.Count; i++)
        {
            var type = events[i].eTimelineEvent;
            if (!firstOfEachEventType.ContainsKey(type))
            {
                Main.Logger.LogInfo($" - Found first event of type {type} at time {events[i].GetTimeSeconds()} seconds.");
                firstOfEachEventType[type] = events[i];
            }
        }

        var lastOfEachEventType = new Dictionary<ETimelineEvent, TimelineEvent>();
        for (var i = events.Count - 1; i >= 0; i--)
        {
            var type = events[i].eTimelineEvent;
            if (!lastOfEachEventType.ContainsKey(type))
            {
                Main.Logger.LogInfo($" - Found last event of type {type} at time {events[i].GetTimeSeconds()} seconds.");
                lastOfEachEventType[type] = events[i];
            }
        }

        foreach (var timelineEvent in firstOfEachEventType.Values)
        {
            lastEventTime = lastOfEachEventType[timelineEvent.eTimelineEvent].timeMinutes;
            var timelineEventFrequencyMinutes = timelineEvent.timeMinutes;
            while (lastEventTime + timelineEventFrequencyMinutes < newStageDurationMinutes)
            {
                lastEventTime += timelineEventFrequencyMinutes;
                var newEvent = new TimelineEvent
                {
                    eTimelineEvent = timelineEvent.eTimelineEvent,
                    duration = timelineEvent.duration,
                    enemies = timelineEvent.enemies,
                    timeMinutes = lastEventTime
                };

                stageTimeline.events.Add(newEvent);
                Main.Logger.LogInfo($" - Added new event of type {timelineEvent.eTimelineEvent} at time {newEvent.GetTimeSeconds()} seconds.");
            }
        }
    }
}