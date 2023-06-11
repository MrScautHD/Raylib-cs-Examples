/*******************************************************************************************
*
*   raylib [models] example - rlgl module usage with push/pop matrix transformations
*
*   This example uses [rlgl] module funtionality (pseudo-OpenGL 1.1 style coding)
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2018 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace Examples.Models
{
    public class SolarSystem
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            const float sunRadius = 4.0f;
            const float earthRadius = 0.6f;
            const float earthOrbitRadius = 8.0f;
            const float moonRadius = 0.16f;
            const float moonOrbitRadius = 1.5f;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - rlgl module usage with push/pop matrix transformations");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(16.0f, 16.0f, 16.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            // General system rotation speed
            float rotationSpeed = 0.2f;
            // Rotation of earth around itself (days) in degrees
            float earthRotation = 0.0f;
            // Rotation of earth around the Sun (years) in degrees
            float earthOrbitRotation = 0.0f;
            // Rotation of moon around itself
            float moonRotation = 0.0f;
            // Rotation of moon around earth in degrees
            float moonOrbitRotation = 0.0f;

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_FREE);

                earthRotation += (5.0f * rotationSpeed);
                earthOrbitRotation += (365 / 360.0f * (5.0f * rotationSpeed) * rotationSpeed);
                moonRotation += (2.0f * rotationSpeed);
                moonOrbitRotation += (8.0f * rotationSpeed);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode3D(camera);

                rlPushMatrix();
                // Scale Sun
                rlScalef(sunRadius, sunRadius, sunRadius);
                // Draw the Sun
                DrawSphereBasic(Color.GOLD);
                rlPopMatrix();

                rlPushMatrix();
                // Rotation for Earth orbit around Sun
                rlRotatef(earthOrbitRotation, 0.0f, 1.0f, 0.0f);
                // Translation for Earth orbit
                rlTranslatef(earthOrbitRadius, 0.0f, 0.0f);
                // Rotation for Earth orbit around Sun inverted
                rlRotatef(-earthOrbitRotation, 0.0f, 1.0f, 0.0f);

                rlPushMatrix();
                // Rotation for Earth itself
                rlRotatef(earthRotation, 0.25f, 1.0f, 0.0f);
                // Scale Earth
                rlScalef(earthRadius, earthRadius, earthRadius);

                // Draw the Earth
                DrawSphereBasic(Color.BLUE);
                rlPopMatrix();

                // Rotation for Moon orbit around Earth
                rlRotatef(moonOrbitRotation, 0.0f, 1.0f, 0.0f);
                // Translation for Moon orbit
                rlTranslatef(moonOrbitRadius, 0.0f, 0.0f);
                // Rotation for Moon orbit around Earth inverted
                rlRotatef(-moonOrbitRotation, 0.0f, 1.0f, 0.0f);
                // Rotation for Moon itself
                rlRotatef(moonRotation, 0.0f, 1.0f, 0.0f);
                // Scale Moon
                rlScalef(moonRadius, moonRadius, moonRadius);

                // Draw the Moon
                DrawSphereBasic(Color.LIGHTGRAY);
                rlPopMatrix();

                // Some reference elements (not affected by previous matrix transformations)
                DrawCircle3D(
                    new Vector3(0.0f, 0.0f, 0.0f),
                    earthOrbitRadius,
                    new Vector3(1, 0, 0),
                    90.0f,
                    ColorAlpha(Color.RED, 0.5f)
                );
                DrawGrid(20, 1.0f);

                EndMode3D();

                DrawText("EARTH ORBITING AROUND THE SUN!", 400, 10, 20, Color.MAROON);
                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // Draw sphere without any matrix transformation
        // NOTE: Sphere is drawn in world position ( 0, 0, 0 ) with radius 1.0f
        static void DrawSphereBasic(Color color)
        {
            int rings = 16;
            int slices = 16;

            rlBegin(DrawMode.TRIANGLES);
            rlColor4ub(color.r, color.g, color.b, color.a);

            for (int i = 0; i < (rings + 2); i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    rlVertex3f(
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                        MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * i)),
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Cos(DEG2RAD * (j * 360 / slices))
                    );
                    rlVertex3f(
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                        MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices))
                    );
                    rlVertex3f(
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                        MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * (j * 360 / slices))
                    );

                    rlVertex3f(
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                        MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * i)),
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Cos(DEG2RAD * (j * 360 / slices))
                    );
                    rlVertex3f(
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                        MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i))),
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices))
                    );
                    rlVertex3f(
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                        MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                        MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices))
                    );
                }
            }
            rlEnd();
        }
    }
}
