/*******************************************************************************************
*
*   raylib [models] example - Draw textured cube
*
*   Example originally created with raylib 4.5, last time updated with raylib 4.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2022-2023 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace Examples.Models
{
    public class ModelCubeTexture
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - draw cube texture");

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(0.0f, 10.0f, 10.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            // Load texture to be applied to the cubes sides
            Texture2D texture = LoadTexture("resources/cubicmap_atlas.png");

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode3D(camera);

                // Draw cube with an applied texture
                DrawCubeTexture(texture, new Vector3(-2.0f, 2.0f, 0.0f), 2.0f, 4.0f, 2.0f, Color.WHITE);

                // Draw cube with an applied texture, but only a defined rectangle piece of the texture
                DrawCubeTextureRec(
                    texture,
                    new Rectangle(0, texture.height / 2, texture.width / 2, texture.height / 2),
                    new Vector3(2.0f, 1.0f, 0.0f),
                    2.0f,
                    2.0f,
                    2.0f,
                    Color.WHITE
                );

                DrawGrid(10, 1.0f);

                EndMode3D();

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texture);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // Draw cube textured
        // NOTE: Cube position is the center position
        static void DrawCubeTexture(
            Texture2D texture,
            Vector3 position,
            float width,
            float height,
            float length,
            Color color
        )
        {
            float x = position.X;
            float y = position.Y;
            float z = position.Z;

            // Set desired texture to be enabled while drawing following vertex data
            rlSetTexture(texture.id);

            // Vertex data transformation can be defined with the commented lines,
            // but in this example we calculate the transformed vertex data directly when calling rlVertex3f()
            // rlPushMatrix();
            // NOTE: Transformation is applied in inverse order (scale -> rotate -> translate)
            // rlTranslatef(2.0f, 0.0f, 0.0f);
            // rlRotatef(45, 0, 1, 0);
            // rlScalef(2.0f, 2.0f, 2.0f);

            rlBegin(DrawMode.QUADS);
            rlColor4ub(color.r, color.g, color.b, color.a);

            // Front Face
            // Normal Pointing Towards Viewer
            rlNormal3f(0.0f, 0.0f, 1.0f);
            rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            rlVertex3f(x - width / 2, y + height / 2, z + length / 2);

            // Back Face
            // Normal Pointing Away From Viewer
            rlNormal3f(0.0f, 0.0f, -1.0f);
            rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            rlVertex3f(x + width / 2, y - height / 2, z - length / 2);

            // Top Face
            // Normal Pointing Up
            rlNormal3f(0.0f, 1.0f, 0.0f);
            rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            rlVertex3f(x + width / 2, y + height / 2, z - length / 2);

            // Bottom Face
            // Normal Pointing Down
            rlNormal3f(0.0f, -1.0f, 0.0f);
            rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            rlVertex3f(x - width / 2, y - height / 2, z + length / 2);

            // Right face
            // Normal Pointing Right
            rlNormal3f(1.0f, 0.0f, 0.0f);
            rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            rlVertex3f(x + width / 2, y - height / 2, z + length / 2);

            // Left Face
            // Normal Pointing Left
            rlNormal3f(-1.0f, 0.0f, 0.0f);
            rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            rlEnd();
            //rlPopMatrix();

            rlSetTexture(0);
        }

        // Draw cube with texture piece applied to all faces
        static void DrawCubeTextureRec(
            Texture2D texture,
            Rectangle source,
            Vector3 position,
            float width,
            float height,
            float length,
            Color color
        )
        {
            float x = position.X;
            float y = position.Y;
            float z = position.Z;
            float texWidth = (float)texture.width;
            float texHeight = (float)texture.height;

            // Set desired texture to be enabled while drawing following vertex data
            rlSetTexture(texture.id);

            // We calculate the normalized texture coordinates for the desired texture-source-rectangle
            // It means converting from (tex.width, tex.height) coordinates to [0.0f, 1.0f] equivalent
            rlBegin(DrawMode.QUADS);
            rlColor4ub(color.r, color.g, color.b, color.a);

            // Front face
            rlNormal3f(0.0f, 0.0f, 1.0f);
            rlTexCoord2f(source.x / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, source.y / texHeight);
            rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(source.x / texWidth, source.y / texHeight);
            rlVertex3f(x - width / 2, y + height / 2, z + length / 2);

            // Back face
            rlNormal3f(0.0f, 0.0f, -1.0f);
            rlTexCoord2f((source.x + source.width) / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, source.y / texHeight);
            rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(source.x / texWidth, source.y / texHeight);
            rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(source.x / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x + width / 2, y - height / 2, z - length / 2);

            // Top face
            rlNormal3f(0.0f, 1.0f, 0.0f);
            rlTexCoord2f(source.x / texWidth, source.y / texHeight);
            rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(source.x / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, source.y / texHeight);
            rlVertex3f(x + width / 2, y + height / 2, z - length / 2);

            // Bottom face
            rlNormal3f(0.0f, -1.0f, 0.0f);
            rlTexCoord2f((source.x + source.width) / texWidth, source.y / texHeight);
            rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f(source.x / texWidth, source.y / texHeight);
            rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f(source.x / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x - width / 2, y - height / 2, z + length / 2);

            // Right face
            rlNormal3f(1.0f, 0.0f, 0.0f);
            rlTexCoord2f((source.x + source.width) / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, source.y / texHeight);
            rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
            rlTexCoord2f(source.x / texWidth, source.y / texHeight);
            rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(source.x / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x + width / 2, y - height / 2, z + length / 2);

            // Left face
            rlNormal3f(-1.0f, 0.0f, 0.0f);
            rlTexCoord2f(source.x / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, (source.y + source.height) / texHeight);
            rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
            rlTexCoord2f((source.x + source.width) / texWidth, source.y / texHeight);
            rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
            rlTexCoord2f(source.x / texWidth, source.y / texHeight);
            rlVertex3f(x - width / 2, y + height / 2, z - length / 2);

            rlEnd();

            rlSetTexture(0);
        }
    }
}
