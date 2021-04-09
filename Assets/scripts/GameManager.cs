using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Math;
public class GameManager : MonoBehaviour
{
    public GameObject Player_1;
    public GameObject Player_2;
    public GameObject[] spawnlocation;
    public Camera[] gamecam;
    
    private int currentcam;
    int[,] location = new int[7, 6];
    int[] move = new int[7];
    int turns = 0;
    public bool Ai = false;
    public bool Aiturn = false;
    public bool turn = false; //jei false player 1; true player 2
    // Start is called before the first frame update
    public void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                location[i, j] = 0;
            }
        }
        turn = false;
        Aiturn = false;

        currentcam = 0;
        for (int i = 1; i < gamecam.Length; i++)
        {
            gamecam[i].gameObject.SetActive(false);
        }
        gamecam[currentcam].gameObject.SetActive(true);


    }
    public void newgame()
    {
        SceneManager.LoadScene(0);
    }
    public void Turn_AI()
    {
        if (Ai)
        {
            Ai = false;

        }
        else
        {
            Ai = true;
        }
    }

    public int check;
   
    // Update is called once per frame
    
    void Update()
    {
        
        
        
        if (Ai) 
        {
            
            if (Aiturn == true)
            {
                if (check!=1 || check != 2 || check != 0)
                {

                    Aiturn = false;
                StartCoroutine(Coroutine());
                }
            }
        
        }
    }
    public bool beingHandled = false;

    IEnumerator Coroutine()
    {
        
        yield return new WaitForSeconds(1);
        turn = true;
       
        var moves = new List<Tuple<int, int>>();
        for (int i = 0; i < 7; i++)
        {
            turn = true;
            if (atnaujinti(i) == false)
                continue;
            
            moves.Add(Tuple.Create(i, MinMax(6, false)));
            
            RemoveCoin(i);
        }

        turn = true;
        int maxMoveScore = moves.Max(t => t.Item2);
        var bestMoves = moves.Where(t => t.Item2 == maxMoveScore).ToList();
        var numba = bestMoves[UnityEngine.Random.Range(0, bestMoves.Count)].Item1;
        





        if (atnaujinti(numba) == true)
        {



            Instantiate(Player_2, spawnlocation[numba].transform.position, Quaternion.identity);
            check = laimetojas();
            turn = false;
            Aiturn = false;
            turns++;

            if (check == 1)
            {
                Debug.Log("Player 1 win");

                gamecam[currentcam].gameObject.SetActive(false);
                gamecam[1].gameObject.SetActive(true);

            }
            else if (check == 2)
            {
                Debug.Log("Player 2 win");

                gamecam[currentcam].gameObject.SetActive(false);
                gamecam[2].gameObject.SetActive(true);

            }
            else if (check == 0 && turns == 42)
            {
                Debug.Log("Draw");

                gamecam[currentcam].gameObject.SetActive(false);
                gamecam[3].gameObject.SetActive(true);

            }

        }
    }

    public void Turn(int collumn)
    {

        if (atnaujinti(collumn) == true)
        {
            
            if (turn == false)
            {
                Instantiate(Player_1, spawnlocation[collumn].transform.position, Quaternion.identity);
                check = laimetojas();
                turn = true;
                Aiturn = true;
            }
            else
            {
                Instantiate(Player_2, spawnlocation[collumn].transform.position, Quaternion.identity);
                check = laimetojas();
                turn = false;

            }
            turns++;
            
            
            if (check == 1)
            {
                Debug.Log("Player 1 win");
                
                gamecam[currentcam].gameObject.SetActive(false);
                gamecam[1].gameObject.SetActive(true);
            }
            else if (check == 2)
            {
                Debug.Log("Player 2 win");
                
                gamecam[currentcam].gameObject.SetActive(false);
                gamecam[2].gameObject.SetActive(true);
            }
            else if(check == 0 && turns == 42)
            {
                Debug.Log("Draw");
                
                gamecam[currentcam].gameObject.SetActive(false);
                gamecam[3].gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("0");
            }


        }

        

    }
    
        bool atnaujinti(int column)
    {
        for (int i = 0; i < 6; i++)
        {
            if (location[column, i] == 0)
            {
                if (turn == false)
                {
                    location[column, i] = 1;
                }
                else
                {
                    location[column, i] = 2;
                }
                
                return true;
            }
        }
        return false;
    }
    void RemoveCoin(int column)
    {
        for (int i = 5; i >= 0; i--)
        {
            if (location[column, i] == 1 )
            {
                location[column, i] = 0;
                break;
            }
            else if(location[column, i] == 2)
            {
                location[column, i] = 0;
                break;
            }
        }
    }
        
    public int MinMax(int depth,bool minmaxplayer)
    {
        if (depth<=0)
        return 0;
        
        var winner = laimetojas();
        if (winner == 2)
            return depth;
        if (winner == 1)
            return -depth;
        if (turns == 42)
            return 0;

        int bestValue = minmaxplayer ? -1 : 1;
        for (int i = 0; i < 7; i++)
        {
            changeturn();
            if (atnaujinti(i)==false)
            continue;
            int v = MinMax(depth - 1, !minmaxplayer);
            bestValue = minmaxplayer ? Mathf.Max(bestValue, v) : Mathf.Min(bestValue, v);
            RemoveCoin(i);
        }

        return bestValue;

    }
    public void changeturn()
    {
        if (turn)
        {
            turn = false;
        }
        else
        {
            turn = true;
        }
    }
    int laimetojas()
    {
        if (turn == false)
        {
            //->
            for (int j = 0; j < 6 - 3; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (location[i, j] == 1 && location[i, j + 1] == 1 && location[i, j + 2] == 1 && location[i,j + 3] == 1)
                    {
                        return 1;
                    }
                }
            }
            // |>
            for (int i = 0; i < 7 - 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (location[i,j] == 1 && location[i + 1,j] == 1 && location[i + 2,j] == 1 && location[i + 3,j] == 1)
                    {
                        return 1;
                    }
                }
            }
            // />
            for (int i = 3; i < 7; i++)
            {
                for (int j = 0; j < 6 - 3; j++)
                {
                    if (location[i,j] == 1 && location[i - 1,j + 1] == 1 && location[i - 2,j + 2] == 1 && location[i - 3,j + 3] == 1)
                        return 1;
                }
            }
            // \>
            for (int i = 3; i < 7; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    if (location[i,j] == 1 && location[i - 1,j - 1] == 1 && location[i - 2,j - 2] == 1 && location[i - 3,j - 3] == 1)
                        return 1;
                }
            }
            return 0;
        }
        else
        {
            //->
            for (int j = 0; j < 6 - 3; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (location[i, j] == 2 && location[i, j + 1] == 2 && location[i, j + 2] == 2 && location[i,j + 3] == 2)
                    {
                        return 2;
                    }
                }
            }
            // |>
            for (int i = 0; i < 7 - 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (location[i,j] == 2 && location[i + 1,j] == 2 && location[i + 2,j] == 2 && location[i + 3,j] == 2)
                    {
                        return 2;
                    }
                }
            }
            // />
            for (int i = 3; i < 7; i++)
            {
                for (int j = 0; j < 6 - 3; j++)
                {
                    if (location[i,j] == 2 && location[i - 1,j + 1] == 2 && location[i - 2,j + 2] == 2 && location[i - 3,j + 3] == 2)
                        return 2;
                }
            }
            // \>
            for (int i = 3; i < 7; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    if (location[i,j] == 2 && location[i - 1,j - 1] == 2 && location[i - 2,j - 2] == 2 && location[i - 3,j - 3] == 2)
                        return 2;
                }
            }
            return 0;
        }
    }

}

