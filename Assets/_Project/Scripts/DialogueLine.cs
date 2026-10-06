using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;  // Nama Pembicara (misal: "Benny Badger", "Agent Pip")
    public Sprite characterAvatar; // Gambar muka karakter
    [TextArea(3, 5)]
    public string sentence;        // Isi percakapan
}