using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Main Changes for level generation code expansion in this file.
//


public class GenerateLevel : MonoBehaviour
{  
    //List of room prefabs for the generator
    [SerializeField] GameObject RoomStartPrefab;
    [SerializeField] GameObject RoomEndPrefab;
    [SerializeField] List<GameObject> RoomsPrefab;
    [SerializeField] List<GameObject> VerticalRoomsPrefab;
    [SerializeField] List<GameObject> VerticalOpenRoomsPrefab;
    [SerializeField] List<GameObject> PlacedRooms;

    public int MapLength = 10;
    Vector3 StartingPosition = new Vector3(0,0,0);
    float RoomSize = 10; //Assuming both height and diameters
    
    List<int> RoomTypeSequence = new List<int>();
    //0 = straight x+1
    //1 = right Z+1
    //2 = left Z-1
    //3 = back X-1
    //4 = straight+up X+1 Y +1
    //5 = right+up
    //6 = left+up
    //7 = back+up
    //8 = Up
    //9 = spawn
    //10 = end

    //
    //Ok this code looks kinda bad because I could just make the sequence of coords in the first generate
    //And not use two of the same functions and too many lists
    //I was trying to figure out a coordinate bug through the use of two functions
    //Sometimes overlaps a tile even though the tile coordinate was already taken?
    //
    List<Vector3> TakenLocations = new List<Vector3>();

    void GenerateSequence()
    {
        
        Vector3 GenCords = new Vector3(0,0,0);
        Vector3 NextGenCords = new Vector3(0,0,0);
        Vector3 NextGenCordsBase = new Vector3(0,0,0); //if i made this better i wouldnt need a double v3 to solve this
        TakenLocations.Add(GenCords);
        RoomTypeSequence.Add(9);
        while(RoomTypeSequence.Count < MapLength)
        {
            bool canPlace = true;
            int nextVal = Random.Range(0,4);
            //Trying to prevent multiple rooms that are the same
            Debug.Log("Rolled a" + nextVal);
            float i = Random.Range(0,100);
            if(i > 86f)
            {nextVal+= 4;}
            
            switch (nextVal)
            {
                case 0:
                    Debug.Log("Straight");
                    NextGenCords.x += 1;
                    NextGenCordsBase = NextGenCords;
                    break;
                case 1:
                    Debug.Log("Right");
                    NextGenCords.z += 1;
                    NextGenCordsBase = NextGenCords;
                    break;
                case 2:
                    Debug.Log("Left");
                    NextGenCords.z -= 1;
                    NextGenCordsBase = NextGenCords;
                    break;
                case 3:
                    Debug.Log("Back");
                    NextGenCords.x -= 1;
                    NextGenCordsBase = NextGenCords;
                    break;
                case 4:
                    Debug.Log("Straight+Up");
                    NextGenCords.x += 1;
                    NextGenCordsBase = NextGenCords;
                    NextGenCords.y += 1;
                    break;
                case 5:
                    Debug.Log("Right+Up");
                    NextGenCords.z += 1;
                    NextGenCordsBase = NextGenCords;
                    NextGenCords.y += 1;
                    break;
                case 6:
                    Debug.Log("Left+Up");
                    NextGenCords.z -= 1;
                    NextGenCordsBase = NextGenCords;
                    NextGenCords.y += 1;
                    break;
                case 7:
                    Debug.Log("Back+Up");
                    NextGenCords.x -= 1;
                    NextGenCordsBase = NextGenCords;
                    NextGenCords.y += 1;
                    break;
                case 8:
                    Debug.Log("Up");
                    NextGenCords.x += 1;
                    NextGenCordsBase = NextGenCords;
                    NextGenCords.y += 1;
                    break;
                case 9:
                    Debug.Log("Start");
                    break;
                case 10:
                    Debug.Log("End");
                    break;

            }

            if(V3InList(NextGenCords,TakenLocations) == true)
            {
                Debug.Log("Location Taken : Sky");
                canPlace = false;
            }
            if(V3InList(NextGenCordsBase,TakenLocations) == true)
            {
                Debug.Log("Location Taken : Ground");
                canPlace = false;
            }
            if(canPlace)
            {
                Debug.Log("Location Added");
                RoomTypeSequence.Add(nextVal);
                GenCords = NextGenCords;
                TakenLocations.Add(NextGenCords);
                if(NextGenCords != NextGenCordsBase)
                {
                    TakenLocations.Add(NextGenCordsBase);
                }
            }
        }
        RoomTypeSequence.Add(10);
    }
    
    //probably an inbuilt function i cant think of for this
    bool V3InList(Vector3 v3, List<Vector3> v3List)
    {
        for(int i = 0; i < v3List.Count; i++)
        {
            //Debug.Log("values" + v3 +","+ v3List[i]);
            if(v3List[i].Equals(v3))
            {
                return true;
            }
        }
        return false;
    }

    void PlaceMap()
    {
        
        for(int i = 0; i < RoomTypeSequence.Count; i++)
        {   
            int room = RoomTypeSequence[i];
            int RandomRoomH = Random.Range(0,RoomsPrefab.Count);
            int RandomRoomV = Random.Range(0,VerticalRoomsPrefab.Count);
            int RandomRoomVo = Random.Range(0,VerticalOpenRoomsPrefab.Count);
            switch (room)
            {
                case 0:
                    Debug.Log("Straight");
                    StartingPosition.x += RoomSize;
                    Instantiate(RoomsPrefab[RandomRoomH], StartingPosition, Quaternion.identity).name = i +" type: 0";
                    break;
                case 1:
                    Debug.Log("Right");
                    StartingPosition.z += RoomSize;
                    Instantiate(RoomsPrefab[RandomRoomH], StartingPosition, Quaternion.identity).name = i +" type: 1";
                    break;
                case 2:
                    Debug.Log("Left");
                    StartingPosition.z -= RoomSize;
                    Instantiate(RoomsPrefab[RandomRoomH], StartingPosition, Quaternion.identity).name = i +" type: 2";
                    break;
                case 3:
                    Debug.Log("Back");
                    StartingPosition.x -= RoomSize;
                    Instantiate(RoomsPrefab[RandomRoomH], StartingPosition, Quaternion.identity).name = i +" type: 3";
                    break;
                case 4:
                    Debug.Log("Straight+Up");
                    StartingPosition.x += RoomSize;
                    Instantiate(VerticalRoomsPrefab[RandomRoomV], StartingPosition, Quaternion.identity).name = i +" type: 4";
                    StartingPosition.y += RoomSize;
                    Instantiate(VerticalOpenRoomsPrefab[RandomRoomVo], StartingPosition, Quaternion.identity).name = i +" type: 4 1";
                    break;
                case 5:
                    Debug.Log("Right+Up");
                    StartingPosition.z += RoomSize;
                    Instantiate(VerticalRoomsPrefab[RandomRoomV], StartingPosition, Quaternion.identity).name = i +" type: 5";
                    StartingPosition.y += RoomSize;
                    Instantiate(VerticalOpenRoomsPrefab[RandomRoomVo], StartingPosition, Quaternion.identity).name = i +" type: 5 1";
                    break;
                case 6:
                    Debug.Log("Left+Up");
                    StartingPosition.z -= RoomSize;
                    Instantiate(VerticalRoomsPrefab[RandomRoomV], StartingPosition, Quaternion.identity).name = i +" type: 6";
                    StartingPosition.y += RoomSize;
                    Instantiate(VerticalOpenRoomsPrefab[RandomRoomVo], StartingPosition, Quaternion.identity).name = i +" type: 6 1";
                    break;
                case 7:
                    Debug.Log("Back+Up");
                    StartingPosition.x -= RoomSize;
                    Instantiate(VerticalRoomsPrefab[RandomRoomV], StartingPosition, Quaternion.identity).name = i +" type: 7";
                    StartingPosition.y += RoomSize;
                    Instantiate(VerticalOpenRoomsPrefab[RandomRoomVo], StartingPosition, Quaternion.identity).name = i +" type: 7 1";
                    break;
                case 8: //This ones actually unused 
                    Debug.Log("Up");
                    StartingPosition.x += RoomSize;
                    Instantiate(VerticalRoomsPrefab[RandomRoomV], StartingPosition, Quaternion.identity).name = i +" type: 8";
                    StartingPosition.y += RoomSize;
                    Instantiate(VerticalOpenRoomsPrefab[RandomRoomVo], StartingPosition, Quaternion.identity).name = i +" type: 8 1";
                    break;
                case 9:
                    Debug.Log("Start");
                    Instantiate(RoomStartPrefab, StartingPosition, Quaternion.identity).name = i +" type: 9";
                    break;
                case 10:
                    Debug.Log("End");
                    Instantiate(RoomEndPrefab, StartingPosition, Quaternion.identity).name = i +" type: 10";
                    break;

            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        GenerateSequence();
        PlaceMap();
    }

}
