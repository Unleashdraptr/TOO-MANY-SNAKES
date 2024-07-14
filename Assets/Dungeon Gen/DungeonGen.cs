using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Unity.AI.Navigation;


public class DungeonGen : MonoBehaviour
{
    public GameObject player;
    public GameObject floorParent;
    public NavMeshSurface navMeshSurface;
    public GameObject snakeStart;
    public GameObject corridorPrefab;
    public GameObject floorPrefab;
    public GameObject roofPrefab;
    public GameObject[] roomPrefabs;
    public int sizeX = 25;
    public int sizeZ = 25;
    public int maxAttempts = 100;
    public int failLimit = 15;
    public int tileSize = 4;
    public float roomHeight = 3.4f;
    private List<Room> rooms = new List<Room>();
    private Node[,] map;
    private List<Node> nodeList = new List<Node>();
    void Start()
    {
        map = new Node[sizeX,sizeZ];

        //room placement
        int count = 0;
        int attempts = 0;

        while (count < failLimit && attempts < maxAttempts)
        {
            Room room = new Room(roomPrefabs[Random.Range(0, roomPrefabs.Length)], tileSize, ref rooms, sizeX, sizeZ);
            if (!room.place())
            {
                count++;
            }
            attempts++;
        }
        Debug.Log(rooms.Count+" rooms mapped successfully");
        foreach(Room room in rooms) 
        {
            room.spawn(tileSize);
        }

        //creating a 2d array of the map
        foreach(Room room in rooms)
        {
            List<(int,int)> cells = room.getInhabetingCells();
            foreach((int,int) tile in cells)
            {
                map[tile.Item1,tile.Item2] = new Node(tile.Item1,tile.Item2,true);
            }
        }

        //placing root
        bool rootPlaced = false;
        List<Node> nodeStack = new List<Node>();
        while(!rootPlaced)
        {
            int posX = Random.Range(0,sizeX);
            int posZ = Random.Range(0,sizeZ);
            if(map[posX,posZ]==null)
            {
                map[posX,posZ] = new Node(posX,posZ,true,true);
                nodeStack.Add(map[posX,posZ]);
                rootPlaced = true;
            }
        }
        // filling in all the nodes
        for(int x = 0; x<map.GetLength(0); x++){
            for(int z = 0; z<map.GetLength(1); z++){
                if(map[x,z] == null) map[x,z] = new Node(x,z);
            }
        }
        //applying DFS
        while(nodeStack.Count>0)
        {
            (int,int) currentPos = nodeStack.Last().getPosition();
            List<Node> nextStepOptions = new List<Node>();
            if(validNode(currentPos.Item1+1,currentPos.Item2)) nextStepOptions.Add(map[currentPos.Item1+1,currentPos.Item2]);
            if(validNode(currentPos.Item1-1,currentPos.Item2)) nextStepOptions.Add(map[currentPos.Item1-1,currentPos.Item2]);
            if(validNode(currentPos.Item1,currentPos.Item2+1)) nextStepOptions.Add(map[currentPos.Item1,currentPos.Item2+1]);
            if(validNode(currentPos.Item1,currentPos.Item2-1)) nextStepOptions.Add(map[currentPos.Item1,currentPos.Item2-1]);
            if(nextStepOptions.Count==0) 
            {
                nodeStack.RemoveAt(nodeStack.Count-1);
                continue;
            }
            Node nextStep = nextStepOptions[Random.Range(0,nextStepOptions.Count)];
            nodeStack.Last().adopt(nextStep);
            nodeStack.Add(nextStep);
        }
        for(int x = 0; x<map.GetLength(0); x++){
            for(int z = 0; z<map.GetLength(1); z++){
                if(map[x,z].hasParent() || map[x,z].isRoot()) map[x,z].spawn(player, floorParent, snakeStart,corridorPrefab,floorPrefab,roofPrefab,sizeX,sizeZ,tileSize,roomHeight);
            }
        }
        navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool validNode(int posX, int posZ)
    {
        return posX>=0 && posX<sizeX && posZ>=0 &&posZ<sizeZ && !map[posX,posZ].getVisited();
    }

    public class Room
    {
        private List<Room> map;
        private int mapX;
        private int mapZ;
        private GameObject room;
        private List<(int,int)> checkCells;
        private List<(int,int)> inhabetingCells;
        private int sizeX;
        private int sizeZ;
        private int posX;
        private int posZ;
        public Room(GameObject room, int tileSize, ref List<Room> map, int mapX, int mapZ)
        {
            //Debug.Log("creating room");
            Vector3 renderer = room.GetComponent<Renderer>().bounds.size;
            this.room = room;
            this.sizeX = Mathf.CeilToInt(renderer.x / tileSize);
            this.sizeZ = Mathf.CeilToInt(renderer.z / tileSize);
            this.map = map;
            this.mapX = mapX;
            this.mapZ = mapZ;
            //Debug.Log("room created");
        }
        public void setPos(int x, int z)
        {
            posX = x;
            posZ = z;
        }
        public bool place()
        {
            for (int i=0; i<20; i++)
            {
                int x = Random.Range(0,mapX-sizeX);
                int z = Random.Range(0,mapZ-sizeZ);
                bool successful = true;
                foreach (Room room in map){
                    if(isColliding(x,z,room))
                    {
                        successful = false;
                        break;
                    }
                }
                if(successful)
                {
                    setPos(x,z);
                    map.Add(this);
                    Debug.Log($"room added successfully at {x},{z}");
                    return true;
                }
            }
            Debug.Log("failed attempt");
            return false;
        }
        private bool isColliding(int testX, int testZ,Room room)
        {
            List<(int,int)> cellsToCheck = room.cellsToCheck();
            inhabetingCells = new List<(int, int)>();

            for(int x = testX; x<testX+sizeX; x++)
            {
                for(int z = testZ; z<testZ+sizeZ;z++)
                {
                    inhabetingCells.Add((x,z));
                }
            }
            foreach((int,int) i in inhabetingCells)
            {
                foreach((int,int) j in cellsToCheck)
                {
                    if(i.Item1 == j.Item1 && i.Item2 == j.Item2) return true;
                }
            }
            return false;
        }
        public List<(int,int)> cellsToCheck()
        {
            if(checkCells!=null) return checkCells;
            List<(int,int)> checkList = new List<(int,int)>();
            for(int x = posX-1; x<posX+sizeX+1; x++)
            {
                for(int z = posZ-1; z<posZ+sizeZ+1; z++)
                {
                    if(x>=0 && z>=0 && x<mapX && z<mapZ) checkList.Add((x,z));
                }
            }
            checkCells = checkList;
            return checkList;
        }
        public void spawn(int tileSize)
        {
            //Debug.Log("attempt instantiation");
            //Instantiate(room,new Vector3((posX)*tileSize,0,(posZ)*tileSize), Quaternion.identity);
            //Debug.Log("completed instantiation");
        }
        public List<(int,int)> getInhabetingCells() 
        { 
            if (inhabetingCells==null) 
            {
                inhabetingCells = new List<(int, int)>();

                for(int x = posX; x<posX+sizeX; x++)
               {
                    for(int z = posZ; z<posZ+sizeZ;z++)
                   {
                        inhabetingCells.Add((x,z));
                   }
                }
            }
            return inhabetingCells;
        }
    }
    public class Node
    {
        private GameObject corridorPrefab;
        protected bool visited = false;
        private int posX;
        private int posZ;
        private bool spawnPoint = false;
        private List<Node> childNodes = new List<Node>();
        private Node parentNode;
        public Node(int posX, int posZ)
        {
            this.posX = posX;
            this.posZ = posZ;
        }
        public Node(int posX, int posZ, bool visited)
        {
            this.posX = posX;
            this.posZ = posZ;
            this.visited = visited;
        }
        public Node(int posX,int posZ, bool visited, bool spawnPoint)
        {
            this.posX = posX;
            this.posZ = posZ;
            this.visited = visited;
            this.spawnPoint = spawnPoint;
        }
        public void adopt(Node child)
        {
            (int,int) childPos = child.getPosition();
            //if(Mathf.Abs(childPos.Item1 - posX) == 1 ^  Mathf.Abs(childPos.Item2 - posZ) == 1) throw new System.Exception($"the child node at {childPos.Item1},{childPos.Item2} is not next to the node at {posX},{posZ}");
            childNodes.Add(child);
            child.visit(this);
        }
        public void visit(Node Parent)
        { 
            visited = true; 
            parentNode = Parent;
        }
        public bool hasParent(){ return parentNode!=null; }
        public bool isRoot(){ return visited && childNodes.Count != 0 && parentNode==null; }
        public (int,int) getPosition(){ return (posX,posZ); }
        public bool getVisited(){ return visited; }
        public int getDirection(Node node)
        {
            int nodeX = node.getPosition().Item1;
            int nodeZ = node.getPosition().Item2;
            if(nodeZ - posZ == -1) return 0;
            if(nodeX - posX == 1) return 1;
            if(nodeZ - posZ == 1) return 2;
            if(nodeX - posX == -1) return 3;
            return 4;
        }
        public void spawn(GameObject player,GameObject floorParent, GameObject snakeStart, GameObject corridor,GameObject floor, GameObject roof, int sizeX, int sizeZ, int tileSize,float roomHeight)
        {
            bool[] walls = new bool[]{true,true,true,true}; // walls to be placed in NESW order

            if(parentNode!=null)
            {
                walls[getDirection(parentNode)] = false;
            }
            foreach(Node child in childNodes)
            {
                walls[getDirection(child)] = false;
            }
            
            if (walls[0]) Instantiate(corridor, new Vector3((posX + 0.5f) * tileSize, roomHeight/2f - 0.5f, posZ * tileSize), Quaternion.Euler(-90, 90, 0)); // North wall
            if (walls[1]) Instantiate(corridor, new Vector3((posX + 1) * tileSize, roomHeight/2f - 0.5f, (posZ + 0.5f) * tileSize), Quaternion.Euler(-90, 0, 0));  // East wall
            if (walls[2]) Instantiate(corridor, new Vector3((posX + 0.5f) * tileSize, roomHeight/2f - 0.5f, (posZ + 1) * tileSize), Quaternion.Euler(-90, -90, 0)); // South wall
            if (walls[3]) Instantiate(corridor, new Vector3(posX * tileSize, roomHeight/2f - 0.5f, (posZ + 0.5f) * tileSize), Quaternion.Euler(-90, 180, 0));  // West wall
            
            var floorMan = Instantiate(floor, new Vector3((posX + 0.5f) * tileSize, 0, (posZ + 0.5f) * tileSize), Quaternion.Euler(-90, 0, 0)); //floor
            floorMan.transform.parent = floorParent.transform;
            Instantiate(roof, new Vector3((posX + 0.5f) * tileSize, roomHeight, (posZ + 0.5f) * tileSize), Quaternion.Euler(90, 0, 0)); //roof
            
            if(spawnPoint) 
            {
                Debug.Log("setting player");
                Instantiate(player,new Vector3((posX + 0.5f) * tileSize, roomHeight/2f - 0.5f, (posZ + 0.5f) * tileSize), Quaternion.identity);
            }
            else{
                if(Random.Range(0,100)>75) Instantiate(snakeStart,new Vector3((posX + 0.5f) * tileSize, 0.5f, (posZ + 0.5f) * tileSize), Quaternion.identity);
            }
        }
    }
}