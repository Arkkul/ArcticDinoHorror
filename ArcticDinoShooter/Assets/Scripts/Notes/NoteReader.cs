using UnityEngine;
using TMPro;

public class NoteReader : MonoBehaviour
{
    [SerializeField] private GameObject _notePanel;

    [SerializeField] private TMP_Text _tittle;
    [SerializeField] private TMP_Text _data;

    public void ShowNote(Note _noteData)
    {
        _tittle.text = _noteData.tittle;
        _data.text = _noteData.data;

        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;

        _notePanel.SetActive(true);
    }

    public void CloseNote()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        _notePanel.SetActive(false);
    }
}
