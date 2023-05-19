/*******************************************************************************************
*
*   raylib [models] example - Mesh picking in 3d mode, ground plane, triangle, mesh
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*   Example contributed by Joel Davis (@joeld42)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;

namespace Examples.Models
{
    public class MeshPicking
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - mesh picking");

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(20.0f, 20.0f, 20.0f);
            camera.target = new Vector3(0.0f, 8.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.6f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            // Picking ray
            Ray ray = new Ray();

            Model tower = LoadModel("resources/models/obj/turret.obj");
            Texture2D texture = LoadTexture("resources/models/obj/turret_diffuse.png");
            Raylib.SetMaterialTexture(ref tower, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);

            Vector3 towerPos = new Vector3(0.0f, 0.0f, 0.0f);
            BoundingBox towerBBox = GetMeshBoundingBox(tower.meshes[0]);

            // Ground quad
            Vector3 g0 = new Vector3(-50.0f, 0.0f, -50.0f);
            Vector3 g1 = new Vector3(-50.0f, 0.0f, 50.0f);
            Vector3 g2 = new Vector3(50.0f, 0.0f, 50.0f);
            Vector3 g3 = new Vector3(50.0f, 0.0f, -50.0f);

            // Test triangle
            Vector3 ta = new Vector3(-25.0f, 0.5f, 0.0f);
            Vector3 tb = new Vector3(-4.0f, 2.5f, 1.0f);
            Vector3 tc = new Vector3(-8.0f, 6.5f, 0.0f);

            Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);

            // Test sphere
            Vector3 sp = new Vector3(-30.0f, 5.0f, 5.0f);
            float sr = 4.0f;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second

            //----------------------------------------------------------------------------------
            // Main game loop
            //--------------------------------------------------------------------------------------
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                //----------------------------------------------------------------------------------
                // Update
                //----------------------------------------------------------------------------------
                if (IsCursorHidden())
                {
                    UpdateCamera(ref camera, CameraMode.CAMERA_FIRST_PERSON);
                }

                // Toggle camera controls
                if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
                {
                    if (IsCursorHidden())
                    {
                        EnableCursor();
                    }
                    else
                    {
                        DisableCursor();
                    }
                }

                // Display information about closest hit
                RayCollision collision = new RayCollision();
                string hitObjectName = "None";
                collision.distance = float.MaxValue;
                collision.hit = false;
                Color cursorColor = Color.WHITE;

                // Get ray and test against objects
                ray = GetMouseRay(GetMousePosition(), camera);

                // Check ray collision aginst ground quad
                RayCollision groundHitInfo = GetRayCollisionQuad(ray, g0, g1, g2, g3);
                if (groundHitInfo.hit && (groundHitInfo.distance < collision.distance))
                {
                    collision = groundHitInfo;
                    cursorColor = Color.GREEN;
                    hitObjectName = "Ground";
                }

                // Check ray collision against test triangle
                RayCollision triHitInfo = GetRayCollisionTriangle(ray, ta, tb, tc);
                if (triHitInfo.hit && (triHitInfo.distance < collision.distance))
                {
                    collision = triHitInfo;
                    cursorColor = Color.PURPLE;
                    hitObjectName = "Triangle";

                    bary = Vector3Barycenter(collision.point, ta, tb, tc);
                }

                // Check ray collision against test sphere
                RayCollision sphereHitInfo = GetRayCollisionSphere(ray, sp, sr);
                if ((sphereHitInfo.hit) && (sphereHitInfo.distance < collision.distance))
                {
                    collision = sphereHitInfo;
                    cursorColor = Color.ORANGE;
                    hitObjectName = "Sphere";
                }

                // Check ray collision against bounding box first, before trying the full ray-mesh test
                RayCollision boxHitInfo = GetRayCollisionBox(ray, towerBBox);
                if (boxHitInfo.hit && boxHitInfo.distance < collision.distance)
                {
                    collision = boxHitInfo;
                    cursorColor = Color.ORANGE;
                    hitObjectName = "Box";

                    // Check ray collision against model meshes
                    RayCollision meshHitInfo = new RayCollision();
                    for (int m = 0; m < tower.meshCount; m++)
                    {
                        // NOTE: We consider the model.transform for the collision check but
                        // it can be checked against any transform matrix, used when checking against same
                        // model drawn multiple times with multiple transforms
                        meshHitInfo = GetRayCollisionMesh(ray, tower.meshes[m], tower.transform);
                        if (meshHitInfo.hit)
                        {
                            // Save the closest hit mesh
                            if ((!collision.hit) || (collision.distance > meshHitInfo.distance))
                            {
                                collision = meshHitInfo;
                            }
                            break;
                        }
                    }

                    if (meshHitInfo.hit)
                    {
                        collision = meshHitInfo;
                        cursorColor = Color.ORANGE;
                        hitObjectName = "Mesh";
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode3D(camera);

                // Draw the tower
                DrawModel(tower, towerPos, 1.0f, Color.WHITE);

                // Draw the test triangle
                DrawLine3D(ta, tb, Color.PURPLE);
                DrawLine3D(tb, tc, Color.PURPLE);
                DrawLine3D(tc, ta, Color.PURPLE);

                // Draw the test sphere
                DrawSphereWires(sp, sr, 8, 8, Color.PURPLE);

                // Draw the mesh bbox if we hit it
                if (boxHitInfo.hit)
                {
                    DrawBoundingBox(towerBBox, Color.LIME);
                }

                // If we hit something, draw the cursor at the hit point
                if (collision.hit)
                {
                    DrawCube(collision.point, 0.3f, 0.3f, 0.3f, cursorColor);
                    DrawCubeWires(collision.point, 0.3f, 0.3f, 0.3f, Color.RED);

                    Vector3 normalEnd = collision.point + collision.normal;
                    DrawLine3D(collision.point, normalEnd, Color.RED);
                }

                DrawRay(ray, Color.MAROON);

                DrawGrid(10, 10.0f);

                EndMode3D();

                // Draw some debug GUI text
                DrawText($"Hit Object: {hitObjectName}", 10, 50, 10, Color.BLACK);

                if (collision.hit)
                {
                    int ypos = 70;

                    DrawText($"Distance: {collision.distance}", 10, ypos, 10, Color.BLACK);

                    DrawText($"Hit Pos: {collision.point}", 10, ypos + 15, 10, Color.BLACK);

                    DrawText($"Hit Norm: {collision.normal}", 10, ypos + 30, 10, Color.BLACK);

                    if (triHitInfo.hit)
                    {
                        DrawText($"Barycenter: {bary}", 10, ypos + 45, 10, Color.BLACK);
                    }
                }

                DrawText("Right click mouse to toggle camera controls", 10, 430, 10, Color.GRAY);

                DrawText("(c) Turret 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, Color.GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(tower);         // Unload model
            UnloadTexture(texture);     // Unload texture

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
