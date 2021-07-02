using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{

    public GameObject rooms;
    public GameObject player;
    
    private const int LevelCols = 4;
    private const int LevelRows = 4;
    private Hashtable _generatedRooms;

    private void Start()
    {
        Application.targetFrameRate = 60;

        InitCastle();
        TeleportPlayerToStart();
    }

    private void InitCastle()
    {
        _generatedRooms = new Hashtable();
        GenerateKingRoom();
        GenerateCornerRoom(LevelCols, 0);
        GenerateBossRoom();
        GenerateRandomRooms();
    }

    private void GenerateKingRoom()
    {
        var kingRoom = GenerateRoom("king_room", 0, 0);
        _generatedRooms.Add(new Vector2(0, 0), kingRoom);
    }

    private void GenerateBossRoom()
    {
        var bossRoom = GenerateRoom("boss_room", LevelCols, LevelRows);
        _generatedRooms.Add(new Vector2(LevelCols, LevelRows), bossRoom);
    }

    private void GenerateRandomRooms()
    {
        var seed = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        var md5 = MD5Util.Md5Sum(seed);
        var chars = md5.ToCharArray();
        var md5Index = 0;
        
        for (var actualLevel = 0; actualLevel <= LevelRows; actualLevel++)
        {
            for (var actualCol = 0; actualCol <= LevelCols; actualCol++)
            {
                if (actualCol == 0 && actualLevel == 0 ||
                    actualCol == LevelCols && actualLevel == 0 ||
                    actualCol == LevelCols && actualLevel == 1 ||
                    actualCol == 0 && actualLevel == 2 ||
                    actualCol == LevelCols && actualLevel == 3 ||
                    actualCol == 0 && actualLevel == LevelRows ||
                    actualCol == LevelCols && actualLevel == LevelRows) continue;

                if (actualLevel == 1 && actualCol == 0 ||
                         actualLevel == 2 && actualCol == LevelCols ||
                         actualLevel == 3 && actualCol == 0)
                {
                    GenerateMiniBossRoom(actualCol, actualLevel);
                }
                else
                {
                    var flip = actualLevel % 2 != 0;
                    var roomName = "room_" + chars[md5Index];
                    //roomName = "room_0";
                    var room = GenerateRoom(roomName, actualCol, actualLevel, flip);
                    _generatedRooms.Add(new Vector2(actualCol, actualLevel), room);

                    if (md5Index == chars.Length)
                    {
                        md5Index = 0;
                    }
                    else
                    {
                        md5Index++;
                    }
                }
            }
        }
    }

    private void GenerateCornerRoom(int col, int row, bool flip = false)
    {
        var cornerRoom = GenerateRoom("corner_room", col, row, flip);
        _generatedRooms.Add(new Vector2(col, row), cornerRoom);
    }

    private void GenerateMiniBossRoom(int col, int row)
    {
        var miniBossRoom = GenerateRoom("mini_boss_room_" + row, col, row);
        _generatedRooms.Add(new Vector2(col, row), miniBossRoom);
    }

    private GameObject GenerateRoom(string roomName, int cols, int row, bool flip = false)
    {
        var room = rooms.transform.Find(roomName);
        var newRoom = Instantiate(room);
        newRoom.gameObject.SetActive(true);
        
        newRoom.transform.position = new Vector3(50 * cols, -25 * row, 0);

        if (flip)
        {
            newRoom.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        return newRoom.gameObject;
    }

    private void TeleportPlayerToStart()
    {
        var startPosition = GetRoomStartPosition(new Vector2(0, 0));
        TeleportPlayer(startPosition.x, startPosition.y);
    }

    private Vector3 GetRoomStartPosition(Vector2 roomPosition)
    {
        var room = (GameObject) _generatedRooms[roomPosition];
        return room.transform.Find("StartPosition").position;
    }

    private void TeleportPlayer(float x, float y)
    {
        player.transform.position = new Vector3(x, y, 0);
    }
}
