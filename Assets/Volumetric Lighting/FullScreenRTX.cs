using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FullScreenRTX : MonoBehaviour
{
    [SerializeField]
    private int width = 1920;
    [SerializeField]
    private int height = 1080;

    RenderTexture rtx;

    public KeyCode key = KeyCode.L;

    private Camera camera;
    bool render;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void OnGUI()
    {
        if(render)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), rtx, ScaleMode.ScaleToFit, false);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(key))
        {
            RenderTexture rtx = new RenderTexture(width, height, 24);
            camera.targetTexture = rtx;
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            render = true;
            camera.Render();
            RenderTexture.active = rtx;
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            render = false;
            camera.targetTexture = null;
            RenderTexture.active = null;

            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(1920, 1080);
            System.IO.File.WriteAllBytes(filename, bytes);
            Application.OpenURL(filename);
        }
    }

    public string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screen_{1}x{2}_{3}.png", Path.GetFullPath("."), width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
}
