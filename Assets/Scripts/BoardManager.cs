using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max){
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5,9);
    public Count foodCount = new Count(1,5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList(){
        gridPositions.Clear();
        //Loop through x axis (columns).
            for(int x = 1; x < columns-1; x++)
            {
                //Within each column, loop through y axis (rows).
                for(int y = 1; y < rows-1; y++)
                {
                    //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                    gridPositions.Add(new Vector3(x,y,0f));
                }
            }
    }

    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup(){
        boardHolder = new GameObject("Board").transform;
        for(int x = -1; x < columns + 1; x++){
            for(int y = -1; y < rows + 1; y++){
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x==-1 || x==columns || y==-1 ||y==rows){
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition(){
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range (0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range (minimum, maximum+1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for(int i = 0; i < objectCount; i++){
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    
    public void SetupScene(int level){
        //Creates the outer walls and floor.
        BoardSetup();
        //Reset our list of gridpositions.
        InitializeList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level,2f);
        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate(exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
    }
    
}
