/*******************************************************************************************
*
*   raylib [models] example - Skybox loading and drawing
*
*   This example has been created using raylib 1.8 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraMode;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.ShaderUniformDataType;

namespace Examples.Models
{
    public class SkyboxDemo
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - skybox loading and drawing");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(4.0f, 1.0f, 4.0f), new Vector3(0.0f, 1.0f, 0.0f), 45.0f, CAMERA_PERSPECTIVE);

            // Load skybox model
            Mesh cube = GenMeshCube(1.0f, 1.0f, 1.0f);
            Model skybox = LoadModelFromMesh(cube);

            // Load skybox shader and set required locations
            // NOTE: Some locations are automatically set at shader loading
            Shader shader = LoadShader("resources/shaders/glsl330/skybox.vs", "resources/shaders/glsl330/skybox.fs");
            Raylib.SetMaterialShader(ref skybox, 0, ref shader);
            Raylib.SetShaderValue(shader, GetShaderLocation(shader, "environmentMap"), (int)MATERIAL_MAP_CUBEMAP, SHADER_UNIFORM_INT);
            Raylib.SetShaderValue(shader, GetShaderLocation(shader, "vflipped"), 1, SHADER_UNIFORM_INT);

            // Load cubemap shader and setup required shader locations
            Shader shdrCubemap = LoadShader("resources/shaders/glsl330/cubemap.vs", "resources/shaders/glsl330/cubemap.fs");
            Raylib.SetShaderValue(shdrCubemap, GetShaderLocation(shdrCubemap, "equirectangularMap"), 0, SHADER_UNIFORM_INT);

            // Load HDR panorama (sphere) texture
            string panoFileName = "resources/dresden_square_2k.hdr";
            Texture2D panorama = LoadTexture(panoFileName);

            // Generate cubemap (texture with 6 quads-cube-mapping) from panorama HDR texture
            // NOTE: New texture is generated rendering to texture, shader computes the sphre->cube coordinates mapping
            //Texture2D cubemap = PBR.GenTextureCubemap(shdrCubemap, panorama, 1024, PIXELFORMAT_UNCOMPRESSED_R8G8B8A8);
            //Raylib.SetMaterialTexture(ref skybox, 0, MATERIAL_MAP_CUBEMAP, ref cubemap);
            UnloadTexture(panorama);      // Texture not required anymore, cubemap already generated

            SetCameraMode(camera, CAMERA_FIRST_PERSON);  // Set a first person camera mode

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);              // Update camera

                // Load new cubemap texture on drag & drop
                if (IsFileDropped())
                {
                    string[] files = Raylib.GetDroppedFiles();

                    if (files.Length == 1)
                    {
                        if (IsFileExtension(files[0], ".png;.jpg;.hdr;.bmp;.tga"))
                        {
                            // Unload current cubemap texture and load new one
                            UnloadTexture(Raylib.GetMaterialTexture(ref skybox, 0, MATERIAL_MAP_CUBEMAP));
                            panorama = LoadTexture(files[0]);
                            panoFileName = files[0];

                            // Generate cubemap from panorama texture
                            //cubemap = PBR.GenTextureCubemap(shdrCubemap, panorama, 1024, PIXELFORMAT_UNCOMPRESSED_R8G8B8A8);
                            // Raylib.SetMaterialTexture(ref skybox, 0, MATERIAL_MAP_CUBEMAP, ref cubemap);
                            UnloadTexture(panorama);
                        }
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);
                DrawModel(skybox, new Vector3(0, 0, 0), 1.0f, WHITE);
                DrawGrid(10, 1.0f);
                EndMode3D();

                DrawFPS(10, 10);
                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(skybox);        // Unload skybox model (and textures)

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
