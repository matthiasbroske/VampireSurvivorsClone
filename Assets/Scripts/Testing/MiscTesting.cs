using UnityEngine;
using Vampire;

public class MiscTesting : MonoBehaviour
{
    [SerializeField] private PointBar healthBar, expBar;
    [SerializeField] private EntityManager entityManager;
    [SerializeField] private Character character;
    [SerializeField] private AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        // healthBar.Init(75, 0, 100);
        // expBar.Init(158, 102, 300);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        //     healthBar.AddPoints(1);
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        //     healthBar.SubtractPoints(1);
        // if (Input.GetKey(KeyCode.UpArrow))
        //     expBar.AddPoints(1);
        // if (Input.GetKey(KeyCode.DownArrow))
        //     expBar.SubtractPoints(1);
        if (Input.GetKeyDown(KeyCode.Space))
            entityManager.CollectAllCoinsAndGems();//entityManager.DamageAllVisibileEnemies(1);
        if (Input.GetKeyDown(KeyCode.G))
            entityManager.DamageAllVisibileEnemies(500);
        if (Input.GetKeyDown(KeyCode.E))
            character.GainExp(1000);
        if (Input.GetKeyDown(KeyCode.A))
            GetComponent<AudioSource>().Play();
    }
}
