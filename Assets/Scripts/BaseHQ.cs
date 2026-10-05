using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHQ : MonoBehaviour
{

  
    [SerializeField] GameObject _eagleHQHonor;
    [SerializeField] GameObject _eagleHQDestrpoyed;
  

    GameObject honor;
    Vector3 _eaglePosition;

    private int _eagleHit ;

    public int EagleHit
    {
        get { return _eagleHit; }
        set { _eagleHit = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _eaglePosition = new Vector3(-25, -303, 0);
         honor = Instantiate(_eagleHQHonor, _eaglePosition, Quaternion.identity);
       
    }

    public void TakeDamage()
    {
       
        
        EagleHit--;
        if(EagleHit < 1 )
        {
            Instantiate(_eagleHQDestrpoyed,_eaglePosition,Quaternion.identity);
            Destroy(honor);
            GameOverManager gameOver = FindAnyObjectByType<GameOverManager>();
            if (gameOver != null)
            { 
                gameOver.ShowGameOver();
            }
            Debug.Log($"У объекта {gameObject.name} осталось попыток: {EagleHit} GameOver");
            return;
        }
    }
}
