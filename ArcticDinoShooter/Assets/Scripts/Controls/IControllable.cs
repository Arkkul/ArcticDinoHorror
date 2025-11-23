using UnityEngine;

public interface IControllable 
{
    public void Move(Vector3 _direction);
    public void Run();
    public void StopRun();
    public void Crouch();
    public void Interact();
    public void Look();
    public bool IsMove();
    public void Jump();
}
