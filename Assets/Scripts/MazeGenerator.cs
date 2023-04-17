using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
  public int mazeWidth = 10;
  public int mazeHeight = 10;
  public GameObject floorPrefab;
  public GameObject wallPrefab;
  public GameObject endPrefab;
  public Material beamMaterial;
  public Material wallMaterial;
  public Material floorMaterial;

  public float wallThickness = 0.1f;
  public float wallHeight = 1.5f; // height of the wall game object
  public float wallLength = 1f;
  public float cellSize = 2f; // size of each cell in the maze
  public float floorThickness = 0.1f; // thickness of the floor game object

  private int startX = 0;
  private int startY = 0;
  private int endX;
  private int endY;

  public TMPro.TextMeshProUGUI winText;
  public Button restartButton;
  public UserInput userInput;
  public MoveCamera moveCamera;

  private GameObject beam;
  private Vector3 mazePosition;
  private int[,] maze;

  public void SetSize(int size) {
    mazeWidth = size;
    mazeHeight = size;
    StartMaze();
  }
  void StartMaze()
  {
    mazePosition = new Vector3(((float) mazeWidth)/2 - 0.5f, -0.5f, ((float) mazeHeight)/2 - 0.5f);
    endX = mazeWidth-1;
    endY = mazeHeight-1;
    maze = GenerateMaze();
    RenderMaze();
  }

  int[,] GenerateMaze()
  {
    int[,] maze = new int[mazeWidth, mazeHeight];
    bool[,] visited = new bool[mazeWidth, mazeHeight];


    GenerateMazeRecursive(startX, startY, endX, endY, maze, visited);

    return maze;
  }

  void GenerateMazeRecursive(int x, int y, int endX, int endY, int[,] maze, bool[,] visited)
  {
    visited[x, y] = true;

    int[] directions = new int[] { 0, 1, 2, 3 };
    Shuffle(directions);

    foreach (int direction in directions)
    {
      int dx = 0;
      int dy = 0;

      if (direction == 0) dx = 1;
      if (direction == 1) dx = -1;
      if (direction == 2) dy = 1;
      if (direction == 3) dy = -1;

      int nextX = x + dx;
      int nextY = y + dy;

      if (nextX < 0 || nextX >= mazeWidth || nextY < 0 || nextY >= mazeHeight || visited[nextX, nextY]) continue;

      maze[x, y] |= 1 << direction;
      maze[nextX, nextY] |= 1 << (direction ^ 1);

      GenerateMazeRecursive(nextX, nextY, endX, endY, maze, visited);

      if (nextX == endX && nextY == endY) return;
    }
  }

  void Shuffle(int[] array)
  {
    for (int i = array.Length - 1; i > 0; i--)
    {
      int j = Random.Range(0, i + 1);
      int temp = array[i];
      array[i] = array[j];
      array[j] = temp;
    }
  }

  void RenderMaze()
  {

    // Create the maze floor
    Vector3 floorPosition = mazePosition;

    for (int x = 0; x < mazeWidth; x++)
    {
      for (int y = 0; y < mazeHeight; y++)
      {
        Vector3 tilePosition = new Vector3(x - mazeWidth / 2f + 0.5f, 0, y - mazeHeight / 2f + 0.5f) + mazePosition;

        GameObject floorTile = Instantiate(floorPrefab, tilePosition, Quaternion.identity);
        floorTile.transform.localScale = new Vector3(cellSize, floorThickness, cellSize);

        // Add a BoxCollider component to the floor tile
        BoxCollider collider = floorTile.AddComponent<BoxCollider>();
        //collider.size = new Vector3(cellSize, 0.1f, cellSize); // Set the collider size to match the floor tile size
        collider.transform.localScale = new Vector3(cellSize, floorThickness, cellSize);
        collider.gameObject.layer = LayerMask.NameToLayer("FloorTile");
        floorTile.tag = "FloorTile";

          // Assign the floorMaterial to the floor tile
        Renderer floorRenderer = floorTile.GetComponent<Renderer>();
        if (floorRenderer != null)
        {
            floorRenderer.material = floorMaterial;
        }

      }
    }
    // Create the maze walls
    for (int x = 0; x < mazeWidth; x++)
    {
      for (int y = 0; y < mazeHeight; y++)
      {
        Vector3 wallPosition = new Vector3(x - mazeWidth / 2f + 0.5f, 0, y - mazeHeight / 2f + 0.5f) + mazePosition;

        if ((maze[x, y] & 1) == 0)
        {
          GameObject wall = Instantiate(wallPrefab, wallPosition + new Vector3(0.5f, 0, 0), Quaternion.identity);
          wall.transform.localScale = new Vector3(wallThickness, wallHeight - 2 * wallThickness, wallLength);
          wall.tag = "MazeWall";
          BoxCollider wallCollider = wall.AddComponent<BoxCollider>();
          wallCollider.transform.localScale = new Vector3(wallThickness, wallHeight - 2 * wallThickness, wallLength);
          Renderer wallRenderer = wall.GetComponent<Renderer>();
          if (wallRenderer != null)
          {
              wallRenderer.material = wallMaterial;
          }
        }

        if ((maze[x, y] & 2) == 0)
        {
          GameObject wall = Instantiate(wallPrefab, wallPosition + new Vector3(-0.5f, 0, 0), Quaternion.identity);
          wall.transform.localScale = new Vector3(wallThickness, wallHeight - 2 * wallThickness, wallLength);
          wall.tag = "MazeWall";
          BoxCollider wallCollider = wall.AddComponent<BoxCollider>();
          wallCollider.transform.localScale = new Vector3(wallThickness, wallHeight - 2 * wallThickness, wallLength);
          Renderer wallRenderer = wall.GetComponent<Renderer>();
          if (wallRenderer != null)
          {
              wallRenderer.material = wallMaterial;
          }
        }

        if ((maze[x, y] & 4) == 0)
        {
          GameObject wall = Instantiate(wallPrefab, wallPosition + new Vector3(0, 0, 0.5f), Quaternion.identity);
          wall.transform.localScale = new Vector3(wallLength, wallHeight - 2 * wallThickness, wallThickness);
          wall.tag = "MazeWall";
          BoxCollider wallCollider = wall.AddComponent<BoxCollider>();
          wallCollider.transform.localScale = new Vector3(wallLength, wallHeight - 2 * wallThickness, wallThickness);
          Renderer wallRenderer = wall.GetComponent<Renderer>();
          if (wallRenderer != null)
          {
              wallRenderer.material = wallMaterial;
          }
        }

        if ((maze[x, y] & 8) == 0)
        {
          GameObject wall = Instantiate(wallPrefab, wallPosition + new Vector3(0, 0, -0.5f), Quaternion.identity);
          wall.transform.localScale = new Vector3(wallLength, wallHeight - 2 * wallThickness, wallThickness);
          wall.tag = "MazeWall";
          BoxCollider wallCollider = wall.AddComponent<BoxCollider>();
          wallCollider.transform.localScale = new Vector3(wallLength, wallHeight - 2 * wallThickness, wallThickness);
          Renderer wallRenderer = wall.GetComponent<Renderer>();
          if (wallRenderer != null)
          {
              wallRenderer.material = wallMaterial;
          }
        }

        // Create the end object
        if (x == endX && y == endY)
        {
          Vector3 endPosition = wallPosition + new Vector3(0, wallHeight / 2f, 0);
          GameObject end = Instantiate(endPrefab, endPosition, Quaternion.identity);
          end.tag = "EndPoint";

          // Add a SphereCollider component to the end object
          SphereCollider sphereCollider = end.AddComponent<SphereCollider>();

          // Set the radius of the sphere collider to half the height of the wall
          float radius = 1f;
          sphereCollider.radius = .01f;
          sphereCollider.isTrigger = true;

          //Add beam of light over end EndPoint
          beam = new GameObject("BeamOfLight");
          beam.transform.position = end.transform.position;
          beam.transform.rotation = Quaternion.identity;
          beam.transform.parent = end.transform;

          MeshRenderer renderer = beam.AddComponent<MeshRenderer>();
          renderer.material = beamMaterial;

          MeshFilter filter = beam.AddComponent<MeshFilter>();
          filter.mesh = CreateBeamMesh();

          Light light = beam.AddComponent<Light>();
          light.type = LightType.Point;
          light.range = 5.0f;
          light.intensity = 3.0f;
          light.color = Color.green;
          beam.tag = "EndPoint";



          ////


          // Add the EndObjectScript component to the end object
          EndPointScript endScript = end.AddComponent<EndPointScript>();

          // Assign any necessary variables to the EndObjectScript component
          endScript.winText = winText;
          endScript.restartButton = restartButton;
          endScript.userInput = userInput;
          endScript.moveCamera = moveCamera;
        }

        floorPosition = new Vector3(x * cellSize, 0, y * cellSize) + mazePosition;

      }
    }
  }

  public void ResetMaze() {
    // Delete all maze game objects
    GameObject[] mazeObjects = GameObject.FindGameObjectsWithTag("FloorTile");
    mazeObjects = mazeObjects.Concat(GameObject.FindGameObjectsWithTag("MazeWall")).ToArray();
    mazeObjects = mazeObjects.Concat(GameObject.FindGameObjectsWithTag("EndPoint")).ToArray();
    foreach (GameObject mazeObject in mazeObjects) {
      Destroy(mazeObject);
    }

  }
  private Mesh CreateBeamMesh()
  {
    Mesh mesh = new Mesh();

    float height = 5.0f;
    float width = 0.2f;

    Vector3[] vertices = new Vector3[4];
    vertices[0] = new Vector3(-width, 0, 0);
    vertices[1] = new Vector3(width, 0, 0);
    vertices[2] = new Vector3(-width, height, 0);
    vertices[3] = new Vector3(width, height, 0);

    int[] triangles = new int[6];
    triangles[0] = 0;
    triangles[1] = 2;
    triangles[2] = 1;
    triangles[3] = 1;
    triangles[4] = 2;
    triangles[5] = 3;

    mesh.vertices = vertices;
    mesh.triangles = triangles;

    return mesh;
  }


}
