using UnityEngine;

public class Sleeper : MonoBehaviour
{
    private bool isParalyzed = false;
    private bool isDistracted = false;

    public void Paralyze()
    {
        isParalyzed = true;
        // Остановить движение
    }

    public void WakeUp()
    {
        isParalyzed = false;
        // Возобновить движение
    }

    public void Distract()
    {
        if (!isParalyzed)
        {
            isDistracted = true;
            // Переместиться к источнику шума
        }
    }

    public void StopDistract()
    {
        isDistracted = false;
        // Вернуться к обычному поведению
    }

    public void Alert()
    {
        if (!isParalyzed)
        {
            // Реакция на тревогу
        }
    }
}
