﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishORamaEngineLibrary;
using System.Diagnostics.Eventing.Reader;

/* FISHORAMA24 | .NET 6.0 | C.Blythe */

namespace FishORama
{
    /// CLASS: Piranha1 - this class is structured as a FishORama engine Token class
    /// It contains all the elements required to draw a token to screen and update it (for movement etc)
    class Piranha1 : IDraw
    {
        // CLASS VARIABLES
        // Variables hold the information for the class
        // NOTE - these variables must be present for the class to act as a TOKEN for the FishORama engine
        private string textureID;               // Holds a string to identify asset used for this token
        private float xPosition;                // Holds the X coordinate for token position on screen
        private float yPosition;                // Holds the Y coordinate for token position on screen
        private int xDirection;                 // Holds the direction the token is currently moving - X value should be either -1 (left) or 1 (right)
        private int yDirection;                 // Holds the direction the token is currently moving - Y value should be either -1 (down) or 1 (up)
        private Screen screen;                  // Holds a reference to the screen dimansions (width and height)
        private ITokenManager tokenManager;     // Holds a reference to the TokenManager - for access to ChickenLeg variable

        // *** ADD YOUR CLASS VARIABLES HERE *** 
        Random rand;

        float xSpeed;
        float ySpeed;

        float initXSpeed;
        float initYSpeed;

        int assetHeight = 86;
        int assetWidth = 128;

        /// CONSTRUCTOR: Piranha1 Constructor
        /// The elements in the brackets are PARAMETERS, which will be covered later in the course
        public Piranha1(string pTextureID, float pXpos, float pYpos, Screen pScreen, ITokenManager pTokenManager, Random pRand)
        {
            // State initialisation (setup) for the object
            textureID = pTextureID;
            xPosition = pXpos;
            yPosition = pYpos;
            xDirection = 1;
            yDirection = 1;
            screen = pScreen;
            tokenManager = pTokenManager;
            rand = pRand;

            // *** ADD OTHER INITIALISATION (class setup) CODE HERE ***
            xSpeed = rand.Next(2, 6); // randomises horizontal speed between the ranges
            initXSpeed = xSpeed;

            ySpeed = 0;
            initYSpeed = ySpeed;
        }

        /// METHOD: Update - will be called repeatedly by the Update loop in Simulation
        /// Write the movement control code here
        public void Update()
        {
            // *** ADD YOUR MOVEMENT/BEHAVIOUR CODE HERE ***

            // Normal Behaviour
            xPosition += xSpeed * xDirection; // 'xPosition + xSpeed * xDirection' & assigns it
            yPosition += ySpeed * yDirection;

            if (xPosition > ((screen.width / 2) - (assetWidth / 2))) // if it hits the right border
            {
                if (rand.Next(0, 4) == 1) yDirection *= -1;
                xDirection = -1;
            }
            else if (xPosition < ((screen.width / -2) + (assetWidth / 2))) // if it hits the left border
            {
                if (rand.Next(0, 4) == 1) yDirection *= -1;
                xDirection = 1;
            }


            // Hungry Behaviour
            if (tokenManager.ChickenLeg != null)
            {
                xSpeed = 6; // Set speed to 6
                ySpeed = 6;

                /* Below calculations given 5 pixel leeway to stop piranha jittering */

                if (xPosition > (tokenManager.ChickenLeg.Position.X + 5)) // Else if X position > chickenleg X position
                {
                    xDirection = -1; // Go left
                } else if (xPosition < (tokenManager.ChickenLeg.Position.X - 5)) // Else if X position < chickenleg X position
                {
                    xDirection = 1; // Go right
                } else
                {
                    xSpeed = 0; // Else stop going left or right
                }

                if (yPosition > (tokenManager.ChickenLeg.Position.Y + 5))
                {
                    yDirection = -1; // Go down
                } else if (yPosition < (tokenManager.ChickenLeg.Position.Y - 5))
                {
                    yDirection = 1; // Go up
                } else
                {
                    ySpeed = 0; // Else stop going up or down
                }

                if (
                    (xPosition > (tokenManager.ChickenLeg.Position.X - 10)) && (xPosition < (tokenManager.ChickenLeg.Position.X + 10)) /* If within 10 pixel range of chicken leg's x position */
                    && /* AND */
                    (yPosition > (tokenManager.ChickenLeg.Position.Y - 10)) && (yPosition < (tokenManager.ChickenLeg.Position.Y + 10)) /* within 10 pixel range of chicken leg's y position */
                   )
                {
                    tokenManager.RemoveChickenLeg(); // If above is satisfied, remove chicken leg
                }
            }
            else // Else if chicken leg not in scene
            { 
                xSpeed = initXSpeed; // Reset speeds
                ySpeed = initYSpeed;
            }
        }

        /// METHOD: Draw - Called repeatedly by FishORama engine to draw token on screen
        /// DO NOT ALTER, and ensure this Draw method is in each token (fish) class
        /// Comments explain the code - read and try and understand each lines purpose
        public void Draw(IGetAsset pAssetManager, SpriteBatch pSpriteBatch)
        {
            Asset currentAsset = pAssetManager.GetAssetByID(textureID); // Get this token's asset from the AssetManager

            SpriteEffects horizontalDirection; // Stores whether the texture should be flipped horizontally

            if (xDirection < 0)
            {
                // If the token's horizontal direction is negative, draw it reversed
                horizontalDirection = SpriteEffects.FlipHorizontally;
            }
            else
            {
                // If the token's horizontal direction is positive, draw it regularly
                horizontalDirection = SpriteEffects.None;
            }

            // Draw an image centered at the token's position, using the associated texture / position
            pSpriteBatch.Draw(currentAsset.Texture,                                             // Texture
                              new Vector2(xPosition, yPosition * -1),                                // Position
                              null,                                                             // Source rectangle (null)
                              Color.White,                                                      // Background colour
                              0f,                                                               // Rotation (radians)
                              new Vector2(currentAsset.Size.X / 2, currentAsset.Size.Y / 2),    // Origin (places token position at centre of sprite)
                              new Vector2(1, 1),                                                // scale (resizes sprite)
                              horizontalDirection,                                              // Sprite effect (used to reverse image - see above)
                              1);                                                               // Layer depth
        }
    }
}
