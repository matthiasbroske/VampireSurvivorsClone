namespace Vampire
{
    public class Magnet : Collectable
    {   
        protected override void OnCollected()
        {
            entityManager.CollectAllCoinsAndGems();
            Destroy(gameObject);
        }
    }
}
