﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts
{
    public class MapGenerator : MonoBehaviour
    {

        public GameObject placeChecker;
        public List<GameObject> placeCheckers = new List<GameObject>();
        
        // public LayerMask layerMask = 10;

        public int width;
        public int height;
        public List<Room> rooms;
        public List<Coord> placementSpots;

        public int seed;
        public bool useRandomSeed;

        [Range(0, 100)]
        public int randomFillPercent;

        int[,] map;

        //void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        var gameObjects = GameObject.FindGameObjectsWithTag("Debug");

        //        for (var i = 0; i < gameObjects.Length; i++)
        //        {
        //            Destroy(gameObjects[i]);
        //        }

        //        GenerateMap();

        //        PlacePlayerAndExit();

        //    }
        //}

        public void GenerateMap()
        {
            map = new int[width, height];
            RandomFillMap();

            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
            }

            ProcessMap();

            int borderSize = 2;
            int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

            for (int x = 0; x < borderedMap.GetLength(0); x++)
            {
                for (int y = 0; y < borderedMap.GetLength(1); y++)
                {
                    if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                    {
                        borderedMap[x, y] = map[x - borderSize, y - borderSize];
                    }
                    else
                    {
                        borderedMap[x, y] = 1;
                    }
                }
            }

            MeshGenerator meshGen = GetComponent<MeshGenerator>();
            meshGen.GenerateMesh(borderedMap, 1);

            FindPlacementSpots();

            Debug.Log("Dungeon has " + rooms.Count + " rooms and " + placementSpots.Count + " placement spots.");


        }

        internal void FindPlacementSpots()
        {

            placementSpots = new List<Coord>();
            foreach (Room room in rooms)
            {
                room.placementSpots = new List<Coord>();


                foreach (Coord spot in room.nonEdgeTiles)
                {

                    var bounds = placeChecker.transform.lossyScale;
                    if (CheckBounds(CoordToWorldPoint(spot), bounds, LayerMask.GetMask("DungeonCheck")))
                    {
                        room.placementSpots.Add(spot);
                        placementSpots.Add(spot);

                    }

                }
            }
            foreach (GameObject checker in placeCheckers)
            {
                var col = checker.GetComponent<CapsuleCollider>();
                col.enabled = false;
                Destroy(checker);
            }



        }
        public bool CheckBounds(Vector3 position, Vector3 boundsSize, int layerMask)
        {
        //    Bounds boxBounds = new Bounds(position, boundsSize);

            //float sqrHalfBoxSize = boxBounds.extents.sqrMagnitude;
            //float overlapingSphereRadius = Mathf.Sqrt(sqrHalfBoxSize + sqrHalfBoxSize);

            /* Hoping I have the previous calculation right, move on to finding the nearby colliders */
            Collider[] hitColliders = Physics.OverlapSphere(position, 2, layerMask);
            //foreach (Collider hit in hitColliders)
            //{

            //    Debug.Log("Hit: " + hit + hit.gameObject.layer);

            //}

            if (hitColliders.Length == 0)
            {
                var checker = Instantiate(placeChecker, position, Quaternion.identity);
                placeCheckers.Add(checker);

                return (true);

            }
            else
            {
                return (false);
            }

        }

        public List<Vector3> GetPlayerAndExitSpots()
        {
            List<Coord> placeListA = new List<Coord>();
            List<Coord> placeListB = new List<Coord>();

            foreach (Coord place in placementSpots)
            {
                placeListA.Add(place);
                placeListB.Add(place);
            }

            int bestDistance = 0;
            Coord bestSpotA = new Coord();
            Coord bestSpotB = new Coord();

            foreach (Coord placeA in placeListA)
            {
                foreach (Coord placeB in placeListB)
                {

                    int distanceBetweenSpots = (int)(Mathf.Pow(placeA.tileX - placeB.tileX, 2) + Mathf.Pow(placeA.tileY - placeB.tileY, 2));

                    if (distanceBetweenSpots > bestDistance)
                    {
                        bestDistance = distanceBetweenSpots;

                        bestSpotA = placeA;
                        bestSpotB = placeB;
                    }
                }
            }

            var playerPosition = CoordToWorldPoint(bestSpotA);
            var exitPosition = CoordToWorldPoint(bestSpotB);
            placementSpots.Remove(bestSpotA);
            placementSpots.Remove(bestSpotB);
            var spots = new List<Vector3>();
            spots.Add(playerPosition);
            spots.Add(exitPosition);                  

            foreach (Room room in rooms)
            {
                if (room.placementSpots.Contains(bestSpotA))
                {
                    room.placementSpots.Remove(bestSpotA);
                    room.isPlayerRoom = true;
                }
                else if (room.placementSpots.Contains(bestSpotB))
                {
                    room.placementSpots.Remove(bestSpotB);
                    room.isExitRoom = true;
                }

            }

            return spots;

        }

        public void PlaceObjects(List<GameObject> objects)
        {
            foreach (GameObject thing in objects)
            {

                var spot = placementSpots[UnityEngine.Random.Range(0, placementSpots.Count)];
                thing.transform.position = CoordToWorldPoint(spot);

                Debug.Log("Placing " + thing.gameObject.name + " on spot " + spot.comp);

                placementSpots.Remove(spot);


            }
        }

        void ProcessMap()
        {
            List<List<Coord>> wallRegions = GetRegions(1);
            int wallThresholdSize = 50;

            foreach (List<Coord> wallRegion in wallRegions)
            {
                if (wallRegion.Count < wallThresholdSize)
                {
                    foreach (Coord tile in wallRegion)
                    {
                        map[tile.tileX, tile.tileY] = 0;
                    }
                }
            }

            List<List<Coord>> roomRegions = GetRegions(0);
            int roomThresholdSize = 50;
            List<Room> survivingRooms = new List<Room>();

            foreach (List<Coord> roomRegion in roomRegions)
            {
                if (roomRegion.Count < roomThresholdSize)
                {
                    foreach (Coord tile in roomRegion)
                    {
                        map[tile.tileX, tile.tileY] = 1;
                    }
                }
                else
                {
                    survivingRooms.Add(new Room(roomRegion, map));
                }
            }
            survivingRooms.Sort();
            survivingRooms[0].isMainRoom = true;
            survivingRooms[0].isAccessibleFromMainRoom = true;

            rooms = survivingRooms;

            ConnectClosestRooms(survivingRooms);
        }

        void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
        {

            List<Room> roomListA = new List<Room>();
            List<Room> roomListB = new List<Room>();

            if (forceAccessibilityFromMainRoom)
            {
                foreach (Room room in allRooms)
                {
                    if (room.isAccessibleFromMainRoom)
                    {
                        roomListB.Add(room);
                    }
                    else
                    {
                        roomListA.Add(room);
                    }
                }
            }
            else
            {
                roomListA = allRooms;
                roomListB = allRooms;
            }

            int bestDistance = 0;
            Coord bestTileA = new Coord();
            Coord bestTileB = new Coord();
            Room bestRoomA = new Room();
            Room bestRoomB = new Room();
            bool possibleConnectionFound = false;

            foreach (Room roomA in roomListA)
            {
                if (!forceAccessibilityFromMainRoom)
                {
                    possibleConnectionFound = false;
                    if (roomA.connectedRooms.Count > 0)
                    {
                        continue;
                    }
                }

                foreach (Room roomB in roomListB)
                {
                    if (roomA == roomB || roomA.IsConnected(roomB))
                    {
                        continue;
                    }

                    for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                    {
                        for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                        {
                            Coord tileA = roomA.edgeTiles[tileIndexA];
                            Coord tileB = roomB.edgeTiles[tileIndexB];
                            int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                            if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                            {
                                bestDistance = distanceBetweenRooms;
                                possibleConnectionFound = true;
                                bestTileA = tileA;
                                bestTileB = tileB;
                                bestRoomA = roomA;
                                bestRoomB = roomB;
                            }
                        }
                    }
                }
                if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
                {
                    CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                }
            }

            if (possibleConnectionFound && forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                ConnectClosestRooms(allRooms, true);
            }

            if (!forceAccessibilityFromMainRoom)
            {
                ConnectClosestRooms(allRooms, true);
            }
        }

        void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
        {
            Room.ConnectRooms(roomA, roomB);
            Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100);

            List<Coord> line = GetLine(tileA, tileB);
            foreach (Coord c in line)
            {
                DrawCircle(c, 5);
            }
        }

        void DrawCircle(Coord c, int r)
        {
            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    if (x * x + y * y <= r * r)
                    {
                        int drawX = c.tileX + x;
                        int drawY = c.tileY + y;
                        if (IsInMapRange(drawX, drawY))
                        {
                            map[drawX, drawY] = 0;
                        }
                    }
                }
            }
        }

        List<Coord> GetLine(Coord from, Coord to)
        {
            List<Coord> line = new List<Coord>();

            int x = from.tileX;
            int y = from.tileY;

            int dx = to.tileX - from.tileX;
            int dy = to.tileY - from.tileY;

            bool inverted = false;
            int step = Math.Sign(dx);
            int gradientStep = Math.Sign(dy);

            int longest = Mathf.Abs(dx);
            int shortest = Mathf.Abs(dy);

            if (longest < shortest)
            {
                inverted = true;
                longest = Mathf.Abs(dy);
                shortest = Mathf.Abs(dx);

                step = Math.Sign(dy);
                gradientStep = Math.Sign(dx);
            }

            int gradientAccumulation = longest / 2;
            for (int i = 0; i < longest; i++)
            {
                line.Add(new Coord(x, y));

                if (inverted)
                {
                    y += step;
                }
                else
                {
                    x += step;
                }

                gradientAccumulation += shortest;
                if (gradientAccumulation >= longest)
                {
                    if (inverted)
                    {
                        x += gradientStep;
                    }
                    else
                    {
                        y += gradientStep;
                    }
                    gradientAccumulation -= longest;
                }
            }

            return line;
        }

        Vector3 CoordToWorldPoint(Coord tile)
        {
            return new Vector3(-width / 2 + .5f + tile.tileX, 1f, -height / 2 + .5f + tile.tileY);
        }

        List<List<Coord>> GetRegions(int tileType)
        {
            List<List<Coord>> regions = new List<List<Coord>>();
            int[,] mapFlags = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                    {
                        List<Coord> newRegion = GetRegionTiles(x, y);
                        regions.Add(newRegion);

                        foreach (Coord tile in newRegion)
                        {
                            mapFlags[tile.tileX, tile.tileY] = 1;
                        }
                    }
                }
            }

            return regions;
        }

        List<Coord> GetRegionTiles(int startX, int startY)
        {
            List<Coord> tiles = new List<Coord>();
            int[,] mapFlags = new int[width, height];
            int tileType = map[startX, startY];

            Queue<Coord> queue = new Queue<Coord>();
            queue.Enqueue(new Coord(startX, startY));
            mapFlags[startX, startY] = 1;

            while (queue.Count > 0)
            {
                Coord tile = queue.Dequeue();
                tiles.Add(tile);

                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                        {
                            if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                            {
                                mapFlags[x, y] = 1;
                                queue.Enqueue(new Coord(x, y));
                            }
                        }
                    }
                }
            }
            return tiles;
        }

        bool IsInMapRange(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }


        void RandomFillMap()
        {
            if (useRandomSeed)
            {
                seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

            }

            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }
                }
            }
        }

        void SmoothMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        map[x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        map[x, y] = 0;

                }
            }
        }

        int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (IsInMapRange(neighbourX, neighbourY))
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        public struct Coord : IComparable<Coord>
        {
            public string comp;
            public int tileX;
            public int tileY;

            public Coord(int x, int y)
            {
                tileX = x;
                tileY = y;
                comp = "X:" + tileX + "Y:" + tileY;
            }


            public int CompareTo(Coord otherCoord)

            {

                return otherCoord.comp.CompareTo(comp);
            }
        }


        public class Room : IComparable<Room>
        {
            public List<Coord> tiles;
            public List<Coord> edgeTiles;
            public List<Coord> nonEdgeTiles;
            public List<Room> connectedRooms;
            public int roomSize;
            public bool isAccessibleFromMainRoom;
            public bool isMainRoom;
            public bool isPlayerRoom = false;
            public bool isExitRoom = false;

            public List<Coord> placementSpots;

            public Room()
            {
            }

            public Room(List<Coord> roomTiles, int[,] map)
            {
                tiles = roomTiles;
                roomSize = tiles.Count;
                connectedRooms = new List<Room>();
                nonEdgeTiles = new List<Coord>();
                foreach (Coord tile in roomTiles)
                {
                    nonEdgeTiles.Add(tile);
                }
                edgeTiles = new List<Coord>();
                foreach (Coord tile in tiles)
                {
                    for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                    {
                        for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                        {
                            if (x == tile.tileX || y == tile.tileY)
                            {
                                if (map[x, y] == 1)
                                {
                                    edgeTiles.Add(tile);
                                    nonEdgeTiles.Remove(tile);
                                }
                            }
                        }
                    }
                }
            }

            public void SetAccessibleFromMainRoom()
            {
                if (!isAccessibleFromMainRoom)
                {
                    isAccessibleFromMainRoom = true;
                    foreach (Room connectedRoom in connectedRooms)
                    {
                        connectedRoom.SetAccessibleFromMainRoom();
                    }
                }
            }

            public static void ConnectRooms(Room roomA, Room roomB)
            {
                if (roomA.isAccessibleFromMainRoom)
                {
                    roomB.SetAccessibleFromMainRoom();
                }
                else if (roomB.isAccessibleFromMainRoom)
                {
                    roomA.SetAccessibleFromMainRoom();
                }
                roomA.connectedRooms.Add(roomB);
                roomB.connectedRooms.Add(roomA);
            }

            public bool IsConnected(Room otherRoom)
            {
                return connectedRooms.Contains(otherRoom);
            }

            public int CompareTo(Room otherRoom)
            {
                return otherRoom.roomSize.CompareTo(roomSize);
            }
        }



    }
}