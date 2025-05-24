using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    public Transform player;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip mario;
    public AudioClip mariachi;
    public AudioClip checkpointSound;

    [Header("Visual FX")]
    public GameObject checkpointFX;

    [Header("API")]
    public string apiMusicUrl = "https://api.agify.io/?name=mario";

    private void Start()
    {
        // Restaurar la posición del jugador
        if (PlayerPrefs.HasKey("checkpoint_x"))
        {
            float x = PlayerPrefs.GetFloat("checkpoint_x");
            float y = PlayerPrefs.GetFloat("checkpoint_y");
            float z = PlayerPrefs.GetFloat("checkpoint_z");
            player.position = new Vector3(x, y, z);
            Debug.Log("✔ Posición restaurada.");
        }

        // Reproducir la última canción guardada
        if (PlayerPrefs.HasKey("last_song"))
        {
            string lastSong = PlayerPrefs.GetString("last_song");
            PlaySong(lastSong);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveCheckpoint(other.transform.position);

            // 🔊 Sonido del checkpoint
            if (checkpointSound != null)
                AudioSource.PlayClipAtPoint(checkpointSound, transform.position);

            // ✨ FX visual
            if (checkpointFX != null)
                Instantiate(checkpointFX, transform.position, Quaternion.identity);

            // 🎵 Cambiar canción con la API
            StartCoroutine(GetSongFromAPI());
        }
    }

    void SaveCheckpoint(Vector3 pos)
    {
        PlayerPrefs.SetFloat("checkpoint_x", pos.x);
        PlayerPrefs.SetFloat("checkpoint_y", pos.y);
        PlayerPrefs.SetFloat("checkpoint_z", pos.z);
        PlayerPrefs.Save();
        Debug.Log("✔ Checkpoint guardado.");
    }

    IEnumerator GetSongFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiMusicUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Alternar entre canciones
            string lastSong = PlayerPrefs.GetString("last_song", "");
            string newSong = (lastSong == "mario") ? "mariachi" : "mario";

            PlaySong(newSong);
            PlayerPrefs.SetString("last_song", newSong);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("❌ Error en la petición de música: " + request.error);
        }
    }

    void PlaySong(string songName)
    {
        switch (songName.ToLower())
        {
            case "mario":
                audioSource.clip = mario;
                break;
            case "mariachi":
                audioSource.clip = mariachi;
                break;
            default:
                Debug.LogWarning("⚠️ Canción no reconocida.");
                return;
        }

        audioSource.Play();
        Debug.Log("🎵 Reproduciendo: " + songName);
    }
}
