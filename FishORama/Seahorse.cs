using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishORamaEngineLibrary;
using System.Diagnostics.Eventing.Reader;

namespace FishORama
{
    /// CLASS: Seahorse - this class is structured as a FishORama engine Token class
    /// It contains all the elements required to draw a token to screen and update it (for movement etc)
    class Seahorse : IDraw
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

            // Speed Variables
        float xSpeed;
        float ySpeed;
        float initXSpeed;
        float initYSpeed;
            
            // Asset Variables
        int assetHeight = 128;
        int assetWidth = 74;

            // Behaviour Variables
        float zigzagYStart;

        int sinkriseDistance;
        float sinkriseStart;

        int sinkBehaviour = 1; // Zizag = 0, Sink/Rise = 1 - Using switch case allows for future behaviour implementation

        /// CONSTRUCTOR: Seahorse Constructor
        /// The elements in the brackets are PARAMETERS, which will be covered later in the course
        public Seahorse(string pTextureID, float pXpos, float pYpos, Screen pScreen, ITokenManager pTokenManager, Random pRand)
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
            ySpeed = xSpeed;
            initXSpeed = xSpeed;
            initYSpeed = ySpeed;

            zigzagYStart = pYpos;

            sinkriseDistance = 100; // For adjustable sink/rise length

            sinkriseStart = yPosition; // Gets starting position of behaviour
        }

        /// METHOD: Update - will be called repeatedly by the Update loop in Simulation
        /// Write the movement control code here
        public void Update()
        {
            // *** ADD YOUR MOVEMENT/BEHAVIOUR CODE HERE ***

            // Alternate Behvaiours
            if (sinkBehaviour != 1) // If NOT sinking/rising
            {
                // ????????
            }


            // Zig Zag Behaviour
            if (sinkBehaviour == 0) // If zigzag behaviour is active
            {
                xSpeed = initXSpeed;
                ySpeed = initYSpeed;

                xPosition += xSpeed * xDirection; // 'xPosition + xSpeed * xDirection' & assigns it
                yPosition += ySpeed * yDirection; // 'yPosition + xSpeed * Direction' & assigns it

                if (yPosition > (zigzagYStart + 50)) // If Y position more than start of zigzag, including the zigzag length, change direction
                {
                    yDirection = -1;
                }
                else if (yPosition < (zigzagYStart - 50))
                {
                    yDirection = 1;
                }
            }

            // Sink / Rise Behaviour
            if (sinkBehaviour == 1) // If sink/rise behaviour is active
            {
                xSpeed = 0;
                ySpeed = 1;
                if (yDirection == -1) // If going down
                {
                    if (yPosition > (sinkriseStart - 100)) // If current pos > (starting pos - 100), sink 
                    {
                        yPosition += ySpeed * yDirection;
                    }
                    else
                    {
                        sinkBehaviour = 0; // Resets behaviour
                    }
                }
                else if (yDirection == 1) // Else if going up
                {
                    if (yPosition < (sinkriseStart + 100)) // If current pos < (starting pos + 100), rise 
                    {
                        yPosition += ySpeed * yDirection;
                    }
                    else
                    {
                        sinkBehaviour = 0; // Resets behaviour
                    }
                }
            }




            // Boundary Constraints
            if (xPosition > ((screen.width / 2) - (assetWidth / 2))) // if it hits the right border
            {
                if (sinkBehaviour != 1) // If NOT sinking/rising
                {
                    xDirection = -1; // Flip direction
                } 
                else { xPosition -= 1; }    // Else teleport back
                sinkBehaviour = 0;          // And reset behaviour to zigzag
            }
            else if (xPosition < ((screen.width / 2 * -1) + (assetWidth / 2))) // if it hits the left border
            {
                if (sinkBehaviour != 1) // If NOT sinking/rising
                {
                    xDirection = 1; // Flip direction
                } 
                else { xPosition += 1; }    // Else teleport back
                sinkBehaviour = 0;          // And reset behaviour to zigzag
            }

            if (yPosition > ((screen.height / 2) - (assetHeight / 2))) // if it hits the top
            {
                if (sinkBehaviour != 1) // If NOT sinking/rising
                {
                    yDirection = -1; // Flip direction
                } 
                else {  yPosition -= 1; }   // Else teleport back
                sinkBehaviour = 0;          // And reset behaviour to zigzag
            }
            else if (yPosition < ((screen.height / 2 * -1) + (assetHeight / 2))) // if it hits a bottom border, turn it around
            {
                if (sinkBehaviour != 1) // If NOT sinking/rising
                {
                    yDirection = 1; // Flip direction
                } 
                else { yPosition += 1; }    // Else teleport back
                sinkBehaviour = 0;          // And reset behaviour to zigzag
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
