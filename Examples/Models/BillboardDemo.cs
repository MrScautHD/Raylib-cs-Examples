/*******************************************************************************************
*
*   raylib [models] example - Drawing billboards
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

namespace Examples.Models
{
    public class BillboardDemo
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - drawing billboards");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(5.0f, 4.0f, 5.0f);
            camera.target = new Vector3(0.0f, 2.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            // Our texture billboard
            Texture2D bill = LoadTexture("resources/billboard.png");

            // Position of billboard billboard
            Vector3 billPositionStatic = new Vector3(0.0f, 2.0f, 0.0f);
            Vector3 billPositionRotating = new Vector3(1.0f, 2.0f, 1.0f);

            // Entire billboard texture, source is used to take a segment from a larger texture.
            Rectangle source = new Rectangle(0.0f, 0.0f, (float)bill.width, (float)bill.height);

            // NOTE: Billboard locked on axis-Y
            Vector3 billUp = new Vector3(0.0f, 1.0f, 0.0f);

            // Rotate around origin
            // Here we choose to rotate around the image center
            // NOTE: (-1, 1) is the range where origin.x, origin.y is inside the texture
            Vector2 rotateOrigin = Vector2.Zero;

            // Distance is needed for the correct billboard draw order
            // Larger distance (further away from the camera) should be drawn prior to smaller distance.
            float distanceStatic = 0.0f;
            float distanceRotating = 0.0f;

            float rotation = 0.0f;

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_ORBITAL);
                rotation += 0.4f;
                distanceStatic = Vector3.Distance(camera.position, billPositionStatic);
                distanceRotating = Vector3.Distance(camera.position, billPositionRotating);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode3D(camera);

                DrawGrid(10, 1.0f);

                // Draw order matters!
                if (distanceStatic > distanceRotating)
                {
                    DrawBillboard(camera, bill, billPositionStatic, 2.0f, Color.WHITE);
                    DrawBillboardPro(
                        camera,
                        bill,
                        source,
                        billPositionRotating,
                        billUp,
                        new Vector2(1.0f, 1.0f),
                        rotateOrigin,
                        rotation,
                        Color.WHITE
                    );
                }
                else
                {
                    DrawBillboardPro(
                        camera,
                        bill,
                        source,
                        billPositionRotating,
                        billUp,
                        new Vector2(1.0f, 1.0f),
                        rotateOrigin,
                        rotation,
                        Color.WHITE
                    );
                    DrawBillboard(camera, bill, billPositionStatic, 2.0f, Color.WHITE);
                }

                EndMode3D();

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(bill);        // Unload texture

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
