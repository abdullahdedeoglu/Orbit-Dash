# Orbit Dash

A hyper-casual mobile game made in Unity. Tilt the arena with your finger, roll the ball into the collectibles and keep it away from the traps for as long as you can.

**Status:** playable prototype, not released yet.

## How it plays

- **Drag to tilt.** Swiping tilts the whole arena on two axes (clamped so it never flips), and the ball rolls with gravity.
- **Collect** the pickups that keep spawning on the arena for points.
- **Avoid** the death traps. Obstacles and walls get in the way.
- **One more try.** When you die you can watch an ad to continue from where you were.

A short interactive tutorial runs on first launch and teaches each piece one step at a time.

## Under the hood

- **Data-driven spawning.** Collectibles, obstacles and traps are ScriptableObjects (prefab, spawn interval, lifetime, max count). A queued spawn manager places them with a minimum distance from each other so nothing overlaps.
- **Game state.** A singleton game manager handles start, pause, game over, continue and restart. High score and tutorial progress are saved with PlayerPrefs.
- **Ads.** Unity Ads: a rewarded ad to continue, and an interstitial every second game over.
- Landscape only, touch input.

## Tech

Unity 2021.3 LTS · C# · Unity Ads · TextMeshPro

## Running it

Open the `Orbit Dash` folder with Unity 2021.3.44f1 (or a later 2021.3), load `Assets/Scenes/MainMenuScene.unity` and press Play. Build for Android or iOS to try the touch controls on a device.
