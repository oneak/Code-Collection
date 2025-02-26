/* 
 * GNU General Public License v2
 * This script is licensed under the GNU GPL v2
 * You are free to modify and distribute it under the same license
 * Credit: Oneak (https://realmmadness.com/oneak)
 */

using UnityEngine;

public class Framerate : MonoBehaviour
{
    private float deltaTime = 0.0f; // Variable to store the time between frames for FPS calculation
    private int fps = 0; // Variable to store the current FPS (frames per second)
    
    // Public variable to define the target framerate in the editor
    // If set to -1, Unity will decide the framerate (unlimited)
    public int targetFrameRate = -1; 

    // Update is called once per frame
    void Update()
    {
        // Calculate the time between frames (smoothed)
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate the FPS by inverting the delta time
        fps = (int)(1.0f / deltaTime);
        
        // Set the target frame rate for the application
        // If targetFrameRate is -1, Unity will not limit the frame rate (unlimited framerate)
        Application.targetFrameRate = targetFrameRate;
    }

    // OnGUI is called to render and handle GUI events
    void OnGUI()
    {
        // Create a style for the FPS display text
        GUIStyle style = new GUIStyle();
        style.fontSize = 30; // Set font size for FPS display
        style.normal.textColor = Color.white; // Set font color to white for visibility

        // Display the current FPS in the top-left corner of the screen
        GUI.Label(new Rect(10, 10, 200, 50), "FPS: " + fps, style);
    }
}