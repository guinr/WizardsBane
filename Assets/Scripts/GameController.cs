using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public GameObject rooms;
    public GameObject player;

    private const string Seed = "d";
    private const int Height = 5;
    private const int Width = 5;
    private string[,] _rooms;

    private void Start()
    {
        Application.targetFrameRate = 60;

        var md5 = MD5Util.Md5Sum(Seed);

        var chars = md5.ToCharArray();

        var normalRooms = int.TryParse(chars[0].ToString(), out _);
        
        _rooms = new string[Height, Width];

        GenerateKingAndBossRooms(normalRooms);
        GenerateSubBossesRooms();
        GenerateRandomRooms(chars);

        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                var room = rooms.transform.Find(_rooms[i, j]);

                var newRoom = Instantiate(room);
                
                newRoom.gameObject.SetActive(true);
                newRoom.transform.position = new Vector3(-18 * j, -14 * i, 0);

                if (_rooms[i, j] != "king_room") continue;
                
                player.transform.position = new Vector3(-18 * j + (normalRooms ? -9 : -18), -14 * i, 0);
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
