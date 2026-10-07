using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    private void Awake()
    {
        // Mencegah audio tumpang tindih jika kembali ke scene awal
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        
        // Mempertahankan GameObject ini (dan lagunya) saat pindah scene
        DontDestroyOnLoad(this.gameObject);
    }
}