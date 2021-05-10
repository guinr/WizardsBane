using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public GameObject rooms;
    public GameObject player;

    public string seed;
    private const int Height = 5;
    private const int Width = 5;
    private string[,] _rooms;

    private void Start()
    {
        Application.targetFrameRate = 60;

        var md5 = MD5Util.Md5Sum(seed);

        var chars = md5.ToCharArray();

        var normalRooms = int.TryParse(chars[0].ToString(), out _);
        
        _rooms = new string[Height, Width];

        GenerateKingAndBossRooms(normalRooms);
        GenerateSubBossesRooms();
        GenerateRandomRooms(chars);

        var lastHeight = 0f;
        var lastWidth = 0f;

        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                var room = rooms.transform.Find(_rooms[i, j]);

                var newRoom = Instantiate(room);
                
                newRoom.gameObject.SetActive(true);
                var box = newRoom.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
                var boxSize = box.size;
                
                newRoom.transform.position = new Vector3(lastHeight, lastWidth, 0);

                lastHeight += boxSize.x;

                if (j == Width -1)
                {
                    lastHeight = 0f;
                    lastWidth -= boxSize.y;                    
                }

                if (_rooms[i, j] != "king_room") continue;
                
                player.transform.position = new Vector3(boxSize.x * j + (normalRooms ? -boxSize.y * 1.3f : boxSize.y/3), -14 * i, 0);
                player.SetActive(true);
            }
        }
    }

    private void GenerateKingAndBossRooms(bool normalRooms)
    {
        if (normalRooms)
        {
            _rooms[0, 0] = "king_room";
            _rooms[Height - 1, Width - 1] = "boss_room";
        }
        else
        {
            _rooms[0, Width - 1] = "king_room";
            _rooms[Height - 1, 0] = "boss_room";
        }
    }

    private void GenerateSubBossesRooms()
    {
        _rooms[1, Random.Range(0, Width - 1)] = "first_boss_room";
        _rooms[2, Random.Range(0, Width - 1)] = "second_boss_room";
        _rooms[3, Random.Range(0, Width - 1)] = "third_boss_room";
    }

    private void GenerateRandomRooms(char[] md5Shards)
    {
        var shardIndex = 0;

        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (_rooms[i, j] != null) continue;

                _rooms[i, j] = "room_" + md5Shards[shardIndex];
                    
                shardIndex++;
            }
        }
    }
}
