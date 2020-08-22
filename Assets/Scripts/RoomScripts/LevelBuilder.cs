using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum SpawnerType
{
    Node,
    Portal,
    Regular
}

public class LevelBuilder : MonoBehaviour
{
    public int baseDoorCost;
    //Rooms
    //Generic Room = 1
    //Generic Pink = 2
    //Generic Purple = 3
    //Generic Teal = 4
    //Generic Turquoise = 5
    public GameObject genericRoom;
    public GameObject genericRoomSpinPink;
    public GameObject genericRoomSpinPurple;
    public GameObject genericRoomSpinTeal;
    public GameObject genericRoomSpinTurquoise;

    //Shop = 6
    //Gas = 7
    public GameObject shopRoom;
    public GameObject gasRoom;

    //Links and deadends
    public GameObject link;
    public GameObject linkX;
    public GameObject endNorth;
    public GameObject endSouth;
    public GameObject endWest;
    public GameObject endEast;

    public GameObject node;
    public GameObject portal;
    public GameObject regularSpawner;

    public GameObject zombie;

    public int area;
    public int spawnerChance;
    public int hostileRoomChance;
    public int[] portalCoord = new int[2];

    private int[] startRoom = new int[] { 0, 0 };
    private int[] nodeOneCoord = new int[2];
    private int[] nodeTwoCoord = new int[2];
    private int[] nodeThreeCoord = new int[2];
    private int[] shopOneCoord = new int[2];
    private int[] shopTwoCoord = new int[2];

    private Vector3 start;

    private Vector3 rightLink;
    private Vector3 rightRoom;

    private Vector3 bottomLink;
    private Vector3 bottomRoom;

    private Vector3 topLink;
    private Vector3 leftLink;

    private int[,] map;
    private GenericRoom[,] mapGenericRoom;

    // Start is called before the first frame update
    void Start()
    {
        initialSetup();
        generateLevel();

        positionPlayer(startRoom[0], startRoom[1]);

        addSpawner(startRoom[0], startRoom[1], SpawnerType.Regular);

        addSpawner(portalCoord[0], portalCoord[1], SpawnerType.Portal);
        addSpawner(nodeOneCoord[0], nodeOneCoord[1], SpawnerType.Node);
        addSpawner(nodeTwoCoord[0], nodeTwoCoord[1], SpawnerType.Node);
        addSpawner(nodeThreeCoord[0], nodeThreeCoord[1], SpawnerType.Node);

        addRandomSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        mapGenericRoom[startRoom[0], startRoom[1]].setRoomActive();
    }

    public void initialSetup()
    {
        initialCoordLinksRoom();

        map = new int[area, area];

        //Set Points For Shop
        shopOneCoord = new int[] { (int)(Math.Floor(area * (0.33))), (int)(Math.Floor(area * (0.33))) };
        shopTwoCoord = new int[] { (int)(Math.Floor(area * (0.66))), (int)(Math.Floor(area * (0.66))) };

        //Generate Random Nodes
        generateRandomNodes();

        int[] curr = { startRoom[0], startRoom[1] };

        map = generateRandomMap(curr, portalCoord, map);
        map = generateRandomMap(curr, nodeOneCoord, map);
        map = generateRandomMap(curr, nodeTwoCoord, map);
        map = generateRandomMap(curr, nodeThreeCoord, map);
        map = generateRandomMap(curr, shopOneCoord, map);
        map = generateRandomMap(curr, shopTwoCoord, map);

        addMapVariability();

        int length = map.GetLength(0);
        mapGenericRoom = new GenericRoom[length, length];
    }

    public void generateLevel()
    {
        int length = map.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (map[i, j] > 0)
                {
                    Vector3 currentRoomPosition = start + (rightRoom * j) + (bottomRoom * i);
                    GameObject currentRoom = spawnRoom(map[i, j], currentRoomPosition);

                    //Set room id 
                    GenericRoom roomScript = currentRoom.GetComponent<GenericRoom>(); 
                    roomScript.roomId = "[ " + i + "," + j + " ]"; 

                    //Set generic room script 
                    mapGenericRoom[i, j] = roomScript; 

                    //Instantiate links and connect 
                    //South Room 
                    if (i + 1 < length && map[i + 1, j] > 0) 
                    {  
                        Vector3 currentLinkPosition = currentRoomPosition + (bottomLink); 
                        GameObject currentLink = Instantiate(linkX, currentLinkPosition, Quaternion.Euler(0, 0, 0)); 
                         
                        Door door = currentLink.GetComponentInChildren(typeof(Door)) as Door;
                        door.cost = (baseDoorCost * (i + 1) + baseDoorCost * (j + 1));
                        door.id = "[" + i + "," + j + "]" + " South";
                        door.rooms.Add(roomScript);

                        roomScript.south = door;
                    }
                    else
                    {
                        //Dead End
                        Vector3 currentLinkPosition = currentRoomPosition + (bottomLink);
                        GameObject deadEnd = Instantiate(endSouth, currentLinkPosition, Quaternion.Euler(0, 0, 0));
                    }

                    //Right Room / East Room
                    //j + 1 < length && map[i, j + 1] == 1
                    if (j + 1 < length && map[i, j + 1] > 0)
                    {
                        Vector3 currentLinkPosition = currentRoomPosition + (rightLink);
                        GameObject currentLink = Instantiate(link, currentLinkPosition, Quaternion.Euler(0, 0, 0));

                        Door door = currentLink.GetComponentInChildren(typeof(Door)) as Door;
                        door.cost = (baseDoorCost * (i+1) + baseDoorCost * (j+1));
                        door.id = "[" + i + "," + j + "]" + " East";
                        door.rooms.Add(roomScript);

                        roomScript.east = door;
                    }
                    else
                    {
                        Vector3 currentLinkPosition = currentRoomPosition + (rightLink);
                        GameObject deadEnd = Instantiate(endEast, currentLinkPosition, Quaternion.Euler(0, 0, 0));
                    }

                    //Left Room / West Room
                    if (j - 1 >= 0 && map[i, j - 1] > 0)
                    {
                        //1.Make 2D array of GenericRooms
                        //2.Ask for left room's east door
                        if (mapGenericRoom[i, j - 1] != null)
                        {
                            GenericRoom leftGenericRoom = mapGenericRoom[i, j - 1];

                            roomScript.west = leftGenericRoom.east;

                            roomScript.west.rooms.Add(roomScript);
                        }
                        else
                        {
                            Debug.Log("ERROR. Somehow null but it shouldn't be");
                        }

                    }
                    else
                    {
                        Vector3 currentLinkPosition = currentRoomPosition + (leftLink);
                        GameObject deadEnd = Instantiate(endWest, currentLinkPosition, Quaternion.Euler(0, 0, 0));
                    }

                    //Top Room / North
                    if (i - 1 >= 0 && map[i - 1, j] > 0)
                    {
                        //Ask for top room's south door
                        if (mapGenericRoom[i - 1, j] != null)
                        {
                            GenericRoom topGenericRoom = mapGenericRoom[i - 1, j];

                            roomScript.north = topGenericRoom.south;

                            roomScript.north.rooms.Add(roomScript);
                        }
                        else
                        {
                            Debug.Log("ERROR. Somehow null but it shouldn't be");
                        }
                    }
                    else
                    {
                        Vector3 currentLinkPosition = currentRoomPosition + (topLink);
                        GameObject deadEnd = Instantiate(endNorth, currentLinkPosition, Quaternion.Euler(0, 0, 0));
                    }
                }
            }

        }
    }

    public GameObject spawnRoom(int roomType, Vector3 roomPos)
    {
        GameObject currentRoom = null;
        if(roomType == 1)
        {
            currentRoom = Instantiate(genericRoom, roomPos, Quaternion.Euler(0, 0, 0));
        }
        else if(roomType == 2)
        {
            currentRoom = Instantiate(genericRoomSpinPink, roomPos, Quaternion.Euler(0, 0, 0));
        }
        else if(roomType == 3)
        {
            currentRoom = Instantiate(genericRoomSpinPurple, roomPos, Quaternion.Euler(0, 0, 0));
        }
        else if(roomType == 4)
        {
            currentRoom = Instantiate(genericRoomSpinTeal, roomPos, Quaternion.Euler(0, 0, 0));
        }
        else if(roomType == 5)
        {
            currentRoom = Instantiate(genericRoomSpinTurquoise, roomPos, Quaternion.Euler(0, 0, 0));
        }
        else if(roomType == 6)
        {
            currentRoom = Instantiate(shopRoom, roomPos, Quaternion.Euler(0, 0, 0));
        }
        else if (roomType == 7)
        {
            currentRoom = Instantiate(gasRoom, roomPos, Quaternion.Euler(0, 0, 0));
        }

        return currentRoom;
    }

    public void positionPlayer(int i, int j)
    {
        Vector3 newPosition = start + (rightRoom * j) + (bottomRoom * i);
        Vector3 offset = new Vector3(25, 5, 10);

        newPosition = newPosition + offset;

        PlayerManager.instance.player.GetComponent<Transform>().position = newPosition;
        PlayerManager.instance.player.GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
    }

    public void addSpawner(int i, int j, SpawnerType type)
    {
        Vector3 newPosition = start + (rightRoom * j) + (bottomRoom * i);
        switch (type)
        {
            case SpawnerType.Node:
                GameObject nodeLocal = Instantiate(node, newPosition, Quaternion.Euler(0, 0, 0));
                nodeLocal.transform.parent = mapGenericRoom[i, j].gameObject.transform;

                nodeLocal.GetComponent<Node>().zombie = zombie;

                nodeLocal.transform.localPosition = new Vector3(30, 5, -20);
                break;
            case SpawnerType.Portal:
                GameObject portalLocal = Instantiate(portal, newPosition, Quaternion.Euler(0, 0, 0));
                portalLocal.transform.parent = mapGenericRoom[i, j].gameObject.transform;

                portalLocal.GetComponent<Portal>().zombie = zombie;

                portalLocal.transform.localPosition = new Vector3(25, 0, -25);
                break;
            case SpawnerType.Regular:
                GameObject regular = Instantiate(regularSpawner, newPosition, Quaternion.Euler(0, 0, 0));
                regular.transform.parent = mapGenericRoom[i, j].gameObject.transform;

                regular.GetComponent<SpawnZombie>().enemy = zombie;

                regular.transform.localPosition = new Vector3(25, 5, -25);
                break;
        }
    }

    public int[,] generateRandomMap(int[] curr, int[] end, int[,] map)
    {
        int length = map.GetLength(0);
        int i = curr[0];
        int j = curr[1];

        int iEnd = end[0];
        int jEnd = end[1];

        if (i == iEnd && j == jEnd)
        {
            map[i, j] = 1;
            return map;
        }

        int rightPotential = jEnd - j;
        int bottomPotential = iEnd - i;

        map[i, j] = 1;
        int[] next;

        int direction = determineDirection(rightPotential, bottomPotential, i, j, length);

        //Right
        if (direction == 0)
        {
            next = new int[] { i, j + 1 };
        }
        else
        {
            next = new int[] { i + 1, j };
        }

        return generateRandomMap(next, end, map);

    }

    private int determineDirection(int rightPotential, int bottomPotential, int i, int j, int length)
    {
        int total = rightPotential + bottomPotential;
        float rightChance = (float)rightPotential / total;
        float bottomChance = (float)bottomPotential / total;

        if (i + 1 >= length)
        {
            bottomChance = 0;
            rightChance = 1;
        }

        if (j + 1 >= length)
        {
            bottomChance = 1;
            rightChance = 0;
        }

        double roll = (double)UnityEngine.Random.Range(0, 100) / 100;


        if (roll < rightChance)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    private void initialCoordLinksRoom()
    {
        //Coordinate 
        start = new Vector3(1000, 0, 1000);

        rightLink = new Vector3(25.5f, 4.5f, -61f);
        rightRoom = new Vector3(0, 0, -71);

        bottomLink = new Vector3(-10f, 4.5f, -26.5f);
        bottomRoom = new Vector3(-71f, 0f, 0f);

        topLink = new Vector3(61.06f, 4.53f, -26.49f);
        leftLink = new Vector3(25.44f, 4.51f, 9.96f);
    }
    
    private void generateRandomNodes()
    {
        nodeOneCoord = new int[] { UnityEngine.Random.Range(0, area - 1), UnityEngine.Random.Range(0, area - 1) };

        while(
            (sameCoords(nodeOneCoord, startRoom)) || 
            (sameCoords(nodeOneCoord, portalCoord)) ||
            (sameCoords(nodeOneCoord, shopOneCoord)) ||
            (sameCoords(nodeOneCoord, shopTwoCoord))
            )
        {
            nodeOneCoord = new int[] { UnityEngine.Random.Range(0, area - 1), UnityEngine.Random.Range(0, area - 1) };
        }

        nodeTwoCoord = new int[] { UnityEngine.Random.Range(0, area - 1), UnityEngine.Random.Range(0, area - 1) };

        while ( (sameCoords(nodeTwoCoord, startRoom)) || 
                (sameCoords(nodeTwoCoord, portalCoord)) || 
                (sameCoords(nodeTwoCoord, nodeOneCoord)) ||
                (sameCoords(nodeTwoCoord, shopOneCoord)) ||
                (sameCoords(nodeTwoCoord, shopTwoCoord))
                )
        {
            nodeTwoCoord = new int[] { UnityEngine.Random.Range(0, area - 1), UnityEngine.Random.Range(0, area - 1) };
        }

        nodeThreeCoord = new int[] { UnityEngine.Random.Range(0, area - 1), UnityEngine.Random.Range(0, area - 1) };

        while(  (sameCoords(nodeThreeCoord, startRoom)) ||
                (sameCoords(nodeThreeCoord, portalCoord)) ||
                (sameCoords(nodeThreeCoord,nodeTwoCoord)) || 
                (sameCoords(nodeThreeCoord,nodeOneCoord)) ||
                (sameCoords(nodeThreeCoord, shopOneCoord)) ||
                (sameCoords(nodeThreeCoord, shopTwoCoord))
                )
        {
            nodeThreeCoord = new int[] { UnityEngine.Random.Range(0, area - 1), UnityEngine.Random.Range(0, area - 1) };
        }
    }

    private bool sameCoords(int[] a, int[] b)
    {
        if(a[0] == b[0] && a[1] == b[1])
        {
            return true;
        }

        return false;
    }

    private void addRandomSpawner()
    {
        for (int i = 0; i < area; i++)
        {
            for (int j = 0; j < area; j++)
            {
                if (map[i, j] >= 1)
                {
                    int[] curr = new int[] { i, j };

                    addMapVariabilitySpawner(curr);
                }
            }
        }
    }
    
    private void addMapVariability()
    {
        for(int i = 0; i < area; i++)
        {
            for(int j = 0; j < area; j++)
            {
                if(map[i,j] == 1)
                {
                    int[] curr = new int[] { i, j };
                    addMapVariabilityRoom(curr);
                }
            }
        }
    }

    private void addMapVariabilitySpawner(int[] curr)
    {
        if(
            !sameCoords(curr, portalCoord) && 
            !sameCoords(curr, nodeOneCoord) && 
            !sameCoords(curr, nodeTwoCoord) && 
            !sameCoords(curr, nodeThreeCoord) && 
            !sameCoords(curr, startRoom) &&
            !sameCoords(curr, shopOneCoord) &&
            !sameCoords(curr, shopTwoCoord)
            )
        {
            int spawnerRoll = UnityEngine.Random.Range(0, 100);

            if(spawnerRoll < spawnerChance)
            {
                addSpawner(curr[0], curr[1], SpawnerType.Regular);
            }
        }
    }

    private void addMapVariabilityRoom(int[] curr)
    {
        if (!sameCoords(curr, portalCoord) && !sameCoords(curr, nodeOneCoord) && !sameCoords(curr, nodeTwoCoord) && !sameCoords(curr, nodeThreeCoord) && !sameCoords(curr, startRoom))
        {
            int hostileRoll = UnityEngine.Random.Range(0, 100);

            if (hostileRoll < hostileRoomChance)
            {
                map[curr[0], curr[1]] = addMapVarHostile();
            }
            else
            {
                map[curr[0], curr[1]] = addMapVarGenericRoom();
            }

            if(sameCoords(curr, shopOneCoord))
            {
                map[curr[0], curr[1]] = 6;
            }

            if(sameCoords(curr, shopTwoCoord))
            {
                map[curr[0], curr[1]] = 6;
            }
        }
    }

    private int addMapVarGenericRoom()
    {
        return UnityEngine.Random.Range(1,5);
    }

    private int addMapVarHostile()
    {
        return 7;
    }
}
