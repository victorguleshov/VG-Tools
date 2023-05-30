using UnityEngine;
using UnityEngine.Tilemaps;

namespace VG
{
    public class GridCreator : MonoBehaviour
    {
        [SerializeField] private SpriteAlignment alignment = SpriteAlignment.Center;
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap tilemap0;

        [SerializeField] private TileBase tilePrefab;

        private Vector3 defaultPosition;

        private void Awake()
        {
            defaultPosition = transform.position;
        }

        private void Create(int[,] map)
        {
            SetAlignment(map.GetLength(0), map.GetLength(1));
            CreateGrid(map);
        }

        private void SetAlignment(int rows, int cols)
        {
            defaultPosition = transform.position;
            switch (alignment)
            {
                case SpriteAlignment.TopRight:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(cols * cellSize.x, rows * cellSize.y);
                    break;
                }

                case SpriteAlignment.TopCenter:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(cols * cellSize.x / 2, rows * cellSize.y);
                    break;
                }

                case SpriteAlignment.TopLeft:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(0, rows * cellSize.y);
                    break;
                }
                case SpriteAlignment.RightCenter:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(cols * cellSize.x, rows * cellSize.y / 2);
                    break;
                }
                case SpriteAlignment.Center:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(cols * cellSize.x / 2, rows * cellSize.y / 2);
                    break;
                }
                case SpriteAlignment.LeftCenter:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(0, rows * cellSize.y / 2);
                    break;
                }

                case SpriteAlignment.BottomRight:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(cols * cellSize.x, 0);
                    break;
                }
                case SpriteAlignment.BottomCenter:
                {
                    var cellSize = grid.cellSize;
                    transform.position -= new Vector3(cols * cellSize.x / 2, 0);
                    break;
                }
                case SpriteAlignment.BottomLeft:
                {
                    break;
                }
            }
        }

        public void Create(int cols, int rows)
        {
            var map = new int [rows, cols];
            for (var index0 = 0; index0 < map.GetLength(0); index0++)
            for (var index1 = 0; index1 < map.GetLength(1); index1++)
                map[index0, index1] = 1;

            Create(map);
        }

        private void CreateGrid(int[,] map, int paddingBottom = 0)
        {
            tilemap0.ClearAllTiles();

            var length0 = map.GetLength(0);
            var length1 = map.GetLength(1);

            for (var row = paddingBottom; row < length0; row++)
            for (var col = 0; col < length1; col++)
                if (map[row, col] == 1)
                    tilemap0.SetTile(new Vector3Int(col, row, 0), tilePrefab);
        }

        public void SetWorldTileSize(Vector2 size)
        {
            var cellSize = grid.cellSize;
            transform.localScale = new Vector3(size.x / cellSize.x, size.y / cellSize.y, 1);
        }

        public void SetWorldTileSize(float size)
        {
            var cellSize = grid.cellSize;
            transform.localScale = new Vector3(size / cellSize.x, size / cellSize.y, 1);
        }

        public static int[,] GenerateArray(int width, int height, bool empty)
        {
            var map = new int[width, height];
            for (var x = 0; x < map.GetUpperBound(0); x++)
            for (var y = 0; y < map.GetUpperBound(1); y++)
                if (empty)
                    map[x, y] = 0;
                else
                    map[x, y] = 1;

            return map;
        }
    }
}