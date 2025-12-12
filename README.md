## 🔫 Shooting Range VR

> Virtual reality (VR) shooting game prototype, developed using Unity 6.

### 🎥 Gameplay Overview


| 60-Second Game Session | Weapon Skin Change |
| :---: | :---: |
| ![shooting-range](https://github.com/user-attachments/assets/4365e529-2785-41c6-938d-569f80fa754d) | ![shooting-range-skins](https://github.com/user-attachments/assets/b21a41fd-9b2e-43e0-8d97-bad2f440591c) |

### ✨ Core Gameplay Features

  * **Objective:** Achieve the highest score possible within **60 seconds**.
  * **Game Start:** The session begins by shooting the central target labeled "**Start Game**".
  * **Armament:** One pistol is available per hand.
  * **Target Management:**
      * Maximum of **3 active targets** at any time.
      * Targets respawn near-instantly using **Object Pooling**.
      * Each destroyed target grants **1 point**.
  * **End Game:** The game stops after 60 seconds. Targets disappear, and the "**Start Game**" button reappears, allowing for a restart without relaunching the project.
  * **Weapon Customization (Skin):** The weapon skin can be changed by shooting the desired model on the `Gun Stand`. (Note: This is a purely visual change; the shooting mechanics remain identical.)

### 🛠 Technical Stack and Optimizations

| Component | Detail | Note |
| :--- | :--- | :--- |
| **Engine** | Unity 6.0 | Version `6000.2.13f1` |
| **VR / Input** | XR Interaction Toolkit | Native locomotion (Teleportation/Continuous) is included via *Starter Assets*. |
| **Architecture** | **Object Pooling** & **Addressables** | Used for memory optimization and dynamic, multi-platform asset loading (PCVR/Quest). |
| **Scripting** | Coroutines | Score updates, pool cleanup, and target spawning are managed asynchronously to reduce CPU spikes and improve VR fluidity. |

### 📦 Installation

1.  Clone the repository.
2.  Open the project using Unity Hub (version **6000.2.13f1**).
3.  Open the scene: `Assets/Scenes/MainXRScene.unity`.

### 📚 Credits

The weapon models were integrated from the following assets:

  * [Low Poly Pistol Weapon Pack 1](https://assetstore.unity.com/packages/3d/props/guns/low-poly-pistol-weapon-pack-1-285693)
  * [Low Poly Pistol Weapon Pack 2](https://assetstore.unity.com/packages/3d/props/guns/low-poly-pistol-weapon-pack-2-304296)
