    public class ShootManHealth: Health
{
    public override void Death()
    {
        Destroy(gameObject);   
    }
}
