using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public GameObject rooms;
    public GameObject player;
    public string seed;
    private const int LevelCols = 5;
    private const int LevelRows = 3;

    private void Start()
    {
        Application.targetFrameRate = 60;

        var md5 = MD5Util.Md5Sum(seed);

        var chars = md5.ToCharArray();
        
        GenerateKingRoom();
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
        for (var actualLevel = 0; actualLevel <= LevelRows; actualLevel++)
        {
            for (var actualCol = 0; actualCol <= LevelCols; actualCol++)
            {
                if (actualCol == 0 && actualLevel == 0 || actualCol == LevelCols && actualLevel == LevelRows) continue;

                if (actualLevel == 0 && actualCol == LevelCols ||
                    actualLevel == 1 && actualCol == 0 ||
                    actualLevel == 2 && actualCol == LevelCols ||
                    actualLevel == 3 && actualCol == 0)
                {
                    GenerateMiniBossRoom(actualCol, actualLevel);
                }
                else
                {
                    GenerateRoom("room_0", actualCol, actualLevel);
                }
            }
        }
    }

    private void GenerateMiniBossRoom(int col, int row)
    {
        var miniBossRoomName = "mini_boss_room_" + row;
        
        GenerateRoom(miniBossRoomName, col, row);
    }

    private void GenerateRoom(string roomName, int cols, int row)
    {
        var room = rooms.transform.Find(roomName);
        var newRoom = Instantiate(room);
        newRoom.gameObject.SetActive(true);
        
        newRoom.transform.position = new Vector3(75 * cols, -75 * row, 0);
    }
}
