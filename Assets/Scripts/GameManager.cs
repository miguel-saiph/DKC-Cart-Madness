using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    private GameObject donkey;
    private GameObject diddy;

    private int _donkeyPos = 1;
    private int _diddyPos = 2;

    public int DonkeyPos
    {
        get { return _donkeyPos; }
        set { _donkeyPos = value; }
    }

    public int DiddyPos
    {
        get { return _diddyPos; }
        set { _diddyPos = value; }
    }

    private Vector2[] cartPositions = { new Vector2(0.1359997f, 0.212f), new Vector2(-0.06500006f, 0.18f) };
    
    
    void Awake()
    {

        if (gm == null)
        {
            gm = this.gameObject.GetComponent<GameManager>();
        }

    }

    private void Start()
    {
        donkey = GameObject.Find("Donkey");
        diddy = GameObject.Find("Diddy");

        if (DonkeyPos != 0) donkey.transform.localPosition = cartPositions[_donkeyPos - 1];
        if (DiddyPos != 0) diddy.transform.localPosition = cartPositions[_diddyPos - 1];


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
