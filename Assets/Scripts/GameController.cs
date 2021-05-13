using System;
using System.Globalization;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public GameObject rooms;
    public GameObject player;
    private const int LevelCols = 4;
    private const int LevelRows = 4;

    private void Start()
    {
        Application.targetFrameRate = 60;

        InitCastle();
    }

    private void InitCastle()
    {
        GenerateKingRoom();
        GenerateCornerRoom(LevelCols, 0);
        GenerateCornerRoom(0, LevelRows, true);
        GenerateBossRoom();
        GenerateRandomRooms();
    }

    private void GenerateKingRoom()
    {
        GenerateRoom("king_room", 0, 0);
        player.transform.position = new Vector3(10, -8.5f, 0);
        player.SetActive(true);
    }

    private void GenerateBossRoom()
    {
        GenerateRoom("boss_room", LevelCols, LevelRows);
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
                    GenerateRoom("room_" + chars[md5Index], actualCol, actualLevel);

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
        GenerateRoom("corner_room", col, row, flip);
    }

    private void GenerateMiniBossRoom(int col, int row)
    {
        var miniBossRoomName = "mini_boss_room_" + row;
        
        GenerateRoom(miniBossRoomName, col, row);
    }

    private void GenerateRoom(string roomName, int cols, int row, bool flip = false)
    {
        var room = rooms.transform.Find(roomName);
        var newRoom = Instantiate(room);
        newRoom.gameObject.SetActive(true);
        
        newRoom.transform.position = new Vector3(75 * cols, -75 * row, 0);

        if (flip)
        {
            newRoom.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }
}
