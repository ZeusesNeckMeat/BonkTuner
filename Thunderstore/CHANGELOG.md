## Changelog

### 1.0.3
- Fixed issue where shrine charge rate reduction was scaling far too quickly

### 1.0.2
- Fixed issue where shrine charge rates were not properly updating

### 1.0.1
- Fixed issue where challenge runs were still using the config values, even when IsEnabledDuringChallenges was disabled

### 1.0.0 (Initial Release)

**Features:**
- In-game configuration UI (toggle with F5)
- Per-map modifiers (Desert, Forest, Graveyard)
  - Stage duration customization
  - Chest/shrine spawn rate multipliers
- Enemy spawn presets (Casual, Normal, Intense, Swarm, Apocalypse)
  - Modifies target enemy count, spawn intervals, and credit gain
- Shrine multipliers with frequency-based scaling
  - Charge rate multiplier
  - Reward multiplier
  - 11 frequency options (per level, per shrine, per stage, etc.)
- Enemy chest drop chance modifier
- Real-time UI with tabs, sliders, and visual feedback
- Config persistence between sessions
- Backup/restore system for map data to prevent state pollution