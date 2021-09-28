using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using MyBox;

namespace RFG.Core
{
  public class PlaceTilesTimed : MonoBehaviour
  {
    [Serializable]
    public class TileData
    {
      public Vector3Int Coordinates;
      public Tile Tile;
    }

    [Header("Settings")]
    public Tilemap Tilemap;
    public float Speed;
    public bool IsPlacing { get; set; }

    [Header("Tile Info")]
    public TileData[] tileData;

    [Header("Effects")]
    public Vector3 EffectsPostionOffset = Vector3.zero;
    public string[] PlaceEffects;
    public string[] RemoveEffects;

    [HideInInspector]
    private Transform _transform;
    private float _timeElapsed = 0f;
    private int _tileIndex = 0;

    private void Awake()
    {
      _transform = transform;
    }

    private void Update()
    {
      if (IsPlacing)
      {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > Speed)
        {
          _timeElapsed = 0;
          PlaceTile();
        }
      }
    }

    private void PlaceTile()
    {
      TileData data = tileData[_tileIndex];
      if (_transform != null)
      {
        _transform.position = Tilemap.CellToWorld(data.Coordinates) + EffectsPostionOffset;
        _transform.SpawnFromPool("Effects", PlaceEffects);
      }
      Tilemap.SetTile(data.Coordinates, data.Tile);
      _tileIndex++;
      if (_tileIndex >= tileData.Length)
      {
        IsPlacing = false;
        _tileIndex = 0;
      }
    }

    private void RemoveTile()
    {
      TileData data = tileData[_tileIndex];
      if (_transform != null)
      {
        _transform.position = Tilemap.CellToWorld(data.Coordinates);
        _transform.SpawnFromPool("Effects", RemoveEffects);
      }
      Tilemap.SetTile(data.Coordinates, null);
      _tileIndex++;
      if (_tileIndex >= tileData.Length)
      {
        IsPlacing = false;
        _tileIndex = 0;
      }
    }

    private static Tile GetTileByName(string name)
    {
      return (Tile)Resources.Load(name, typeof(Tile));
    }

    [ButtonMethod]
    public void AddTiles()
    {
      IsPlacing = true;
      while (IsPlacing)
      {
        PlaceTile();
      }
    }

    [ButtonMethod]
    public void RemoveTiles()
    {
      IsPlacing = true;
      while (IsPlacing)
      {
        RemoveTile();
      }
    }

  }
}