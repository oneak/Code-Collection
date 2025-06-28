# 🟫 Block Terrain Generator

![Screenshot](screenshot.png)

**Block Terrain Generator** Block Terrain Generator is a free, open-source terrain generation tool built in **Unity 6** by [oneak](https://realmmadness.com/oneak).  
It lets you quickly create and experiment with block-style environments for your Unity projects, ideal for prototyping or stylized voxel games

## ✨ Features
- Procedural block terrain generation
- Built for Unity 6 for maximum compatibility
- Fully commented and beginner-friendly code

## 🆓 Usage & License

You are **free to use, modify, and improve** this terrain generator in any way you like.  
Make it your own, contribute back, or use it as a learning resource!

- **No restrictions:** Use in personal, educational, or commercial projects
- **Attribution appreciated:** Please credit **oneak** if you use or share this tool

## 🛠️ How to Use: Quick Start Guide

Follow these steps to get your block terrain up and running in Unity

1. **Copy or Clone the Folder**  
   - Copy the `Block Terrain Generator` folder into your Unity project's `Assets` directory

2. **Create an Empty GameObject**  
   - In your Unity scene, right-click in the Hierarchy and select **Create Empty** 
   - Rename it (for example, `BlockGenerator`)

3. **Add the Script**  
   - Drag the `BlockGenerator.cs` script from your project onto the empty GameObject you just created

4. **Create a Cube Prefab**  
   - In the Project window, right-click and select **Create > 3D Object > Cube**
   - Adjust its size and appearance as desired
   - Drag the cube from the Hierarchy into your Project window to make it a prefab (e.g., name it `BlockPrefab`)
   - Delete the original cube from your scene to keep things tidy

5. **Assign the Prefab to the Script**  
   - Select your `BlockGenerator` GameObject
   - In the Inspector, find the `Block Prefab` field on the script component
   - Drag your `BlockPrefab` prefab into this field

6. **(Optional) Add Decoration Prefabs**  
   - If you want trees or rocks, create prefabs for them and assign them to the `Tree Prefab` and `Rock Prefab` fields

7. **(Optional) Add a Player**  
   - Assign a player prefab to the `Player Prefab` field, or leave it empty for a terrain-only scene

8. **Play!**  
   - Hit the **Play** button in Unity
   - The script will generate a blocky terrain based on your settings
   - Press `R` to generate terrain again

---

**Made with ❤️ by [oneak](https://realmmadness.com/oneak) – part of the [Code Collection](../../README.md)**

**License:** [GNU GPL v2](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html)