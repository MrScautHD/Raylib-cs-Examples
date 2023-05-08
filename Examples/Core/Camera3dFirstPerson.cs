/*******************************************************************************************
*
*   raylib [core] example - 3d camera first person
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples.Core
{
    public class Camera3dFirstPerson
    {
        public const int MAX_COLUMNS = 20;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d camera first person");

            // Define the camera to look into our 3d world (position, target, up vector)
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(4.0f, 2.0f, 4.0f);
            camera.target = new Vector3(0.0f, 1.8f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 60.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            // Generates some random columns
            float[] heights = new float[MAX_COLUMNS];
            Vector3[] positions = new Vector3[MAX_COLUMNS];
            Color[] colors = new Color[MAX_COLUMNS];

            for (int i = 0; i < MAX_COLUMNS; i++)
            {
                heights[i] = (float)GetRandomValue(1, 12);
                positions[i] = new Vector3(GetRandomValue(-15, 15), heights[i] / 2, GetRandomValue(-15, 15));
                colors[i] = new Color(GetRandomValue(20, 255), GetRandomValue(10, 55), 30, 255);
            }

            CameraMode cameraMode = CameraMode.CAMERA_FIRST_PERSON; // Set a first person camera mode

            SetTargetFPS(60);                           // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())                // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // Switch camera mode
                if (IsKeyPressed(KEY_ONE))
                {
                    cameraMode = CameraMode.CAMERA_FREE;
                    camera.up = new Vector3(0.0f, 1.0f, 0.0f);
                }

                if (IsKeyPressed(KEY_TWO))
                {
                    cameraMode = CameraMode.CAMERA_FIRST_PERSON;
                    camera.up = new Vector3(0.0f, 1.0f, 0.0f);
                }

                if (IsKeyPressed(KEY_THREE))
                {
                    cameraMode = CameraMode.CAMERA_THIRD_PERSON;
                    camera.up = new Vector3(0.0f, 1.0f, 0.0f);
                }

                if (IsKeyPressed(KEY_FOUR))
                {
                    cameraMode = CameraMode.CAMERA_ORBITAL;
                    camera.up = new Vector3(0.0f, 1.0f, 0.0f);
                }

                // Switch camera projection
                if (IsKeyPressed(KEY_P))
                {
                    if (camera.projection == CameraProjection.CAMERA_PERSPECTIVE)
                    {
                        // Create isometric view
                        cameraMode = CameraMode.CAMERA_THIRD_PERSON;
                        // Note: The target distance is related to the render distance in the orthographic projection
                        camera.position = new Vector3(0.0f, 2.0f, -100.0f);
                        camera.target = new Vector3(0.0f, 2.0f, 0.0f);
                        camera.up = new Vector3(0.0f, 1.0f, 0.0f);
                        camera.projection = CameraProjection.CAMERA_ORTHOGRAPHIC;
                        camera.fovy = 20.0f; // near plane width in CAMERA_ORTHOGRAPHIC
                        // CameraYaw(&camera, -135 * DEG2RAD, true);
                        // CameraPitch(&camera, -45 * DEG2RAD, true, true, false);
                    }
                    else if (camera.projection == CameraProjection.CAMERA_ORTHOGRAPHIC)
                    {
                        // Reset to default view
                        cameraMode = CameraMode.CAMERA_THIRD_PERSON;
                        camera.position = new Vector3(0.0f, 2.0f, 10.0f);
                        camera.target = new Vector3(0.0f, 2.0f, 0.0f);
                        camera.up = new Vector3(0.0f, 1.0f, 0.0f);
                        camera.projection = CameraProjection.CAMERA_PERSPECTIVE;
                        camera.fovy = 60.0f;
                    }
                }

                // Update camera computes movement internally depending on the camera mode
                // Some default standard keyboard/mouse inputs are hardcoded to simplify use
                // For advance camera controls, it's reecommended to compute camera movement manually
                UpdateCamera(ref camera, CameraMode.CAMERA_CUSTOM);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                // Draw ground
                DrawPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(32.0f, 32.0f), LIGHTGRAY);

                // Draw a blue wall
                DrawCube(new Vector3(-16.0f, 2.5f, 0.0f), 1.0f, 5.0f, 32.0f, BLUE);

                // Draw a green wall
                DrawCube(new Vector3(16.0f, 2.5f, 0.0f), 1.0f, 5.0f, 32.0f, LIME);

                // Draw a yellow wall
                DrawCube(new Vector3(0.0f, 2.5f, 16.0f), 32.0f, 5.0f, 1.0f, GOLD);

                // Draw some cubes around
                for (int i = 0; i < MAX_COLUMNS; i++)
                {
                    DrawCube(positions[i], 2.0f, heights[i], 2.0f, colors[i]);
                    DrawCubeWires(positions[i], 2.0f, heights[i], 2.0f, MAROON);
                }

                // Draw player cube
                if (cameraMode == CameraMode.CAMERA_THIRD_PERSON)
                {
                    DrawCube(camera.target, 0.5f, 0.5f, 0.5f, PURPLE);
                    DrawCubeWires(camera.target, 0.5f, 0.5f, 0.5f, DARKPURPLE);
                }

                EndMode3D();

                // Draw info boxes
                DrawRectangle(10, 10, 220, 70, ColorAlpha(SKYBLUE, 0.5f));
                DrawRectangleLines(10, 10, 220, 70, BLUE);

                DrawText("Camera controls:", 15, 15, 10, BLACK);
                DrawText("- Move keys: W, A, S, D, Space, Left-Ctrl", 15, 30, 10, BLACK);
                DrawText("- Look around: arrow keys or mouse", 15, 45, 10, BLACK);
                DrawText("- Camera mode keys: 1, 2, 3, 4", 15, 60, 10, BLACK);
                DrawText("- Zoom keys: num-plus, num-minus or mouse scroll", 15, 75, 10, BLACK);
                DrawText("- Camera projection key: P", 15, 90, 10, BLACK);

                DrawRectangle(600, 5, 195, 100, Fade(SKYBLUE, 0.5f));
                DrawRectangleLines(600, 5, 195, 100, BLUE);

                DrawText("Camera status:", 610, 15, 10, BLACK);
                DrawText($"- Mode: {cameraMode}", 610, 30, 10, BLACK);
                DrawText($"- Projection: {camera.projection}", 610, 45, 10, BLACK);
                DrawText($"- Position: {camera.position}", 610, 60, 10, BLACK);
                DrawText($"- Target: {camera.target}", 610, 75, 10, BLACK);
                DrawText($"- Up: {camera.up}", 610, 90, 10, BLACK);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
