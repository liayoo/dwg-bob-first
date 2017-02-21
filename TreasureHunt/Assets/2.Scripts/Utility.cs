using UnityEngine;
using System.IO;

public class Utility : MonoBehaviour {

	public static void SaveImage(string fileName, byte[] bytes)
    {
        var file = File.Open("Assets/Resources/" + fileName, FileMode.Create);

        var binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
		Debug.Log ("image saved");
    }
    public static Sprite CreateSprite(string imageName)
    {
        Texture2D tex = Resources.Load<Texture2D>(imageName);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}
