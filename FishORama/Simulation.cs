using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FishORamaEngineLibrary;
using System.IO.IsolatedStorage;

/* FISHORAMA24 | .NET 6.0 | C.Blythe */

namespace FishORama
{
    /// CLASS: Simulation class - the main class users code in to set up a FishORama simulation
    /// All tokens to be displayed in the scene are added here
    public class Simulation : IUpdate, ILoadContent
    {
        // CLASS VARIABLES
        // Variables store the information for the class
        private IKernel kernel;                 // Holds a reference to the game engine kernel which calls the draw method for every token you add to it
        private Screen screen;                  // Holds a reference to the screeen dimensions (width, height)
        private ITokenManager tokenManager;     // Holds a reference to the TokenManager - for access to ChickenLeg variable

        /// PROPERTIES
        public ITokenManager TokenManager      // Property to access chickenLeg variable
        {
            set { tokenManager = value; }
        }

        // *** ADD YOUR CLASS VARIABLES HERE ***
        // Variables to hold fish will be declared here
            
        Random rand;

            // Orange Fish
        int orangeFishWidth = 128;
        int orangeFishHeight = 64;

        OrangeFish orangeFish1;

        int urchinWidth = 180;
        int urchinHeight = 112;

        Urchin[] urchinArray = new Urchin[3];


        /// CONSTRUCTOR - for the Simulation class - run once only when an object of the Simulation class is INSTANTIATED (created)
        /// Use constructors to set up the state of a class
        public Simulation(IKernel pKernel)
        {
            kernel = pKernel;                   // Stores the game engine kernel which is passed to the constructor when this class is created
            screen = kernel.Screen;             // Sets the screen variable in Simulation so the screen dimensions are accessible

            // *** ADD OTHER INITIALISATION (class setup) CODE HERE ***
            rand = new Random();



        }

        /// METHOD: LoadContent - called once at start of program
        /// Create all token objects and 'insert' them into the FishORama engine
        public void LoadContent(IGetAsset pAssetManager)
        {
            // *** ADD YOUR NEW TOKEN CREATION CODE HERE ***
            // Code to create fish tokens and assign to their variables goes here
            // Remember to insert each token into the kernel

            int initXpos = rand.Next(((screen.width / -2 + 1) + (orangeFishWidth / 2)), 
                (screen.width / 2 - 1) - (orangeFishWidth / 2)); 
            // Minimum - Divides screen width by 2, then makes it a negative int. Also account for asset size and adjusts by a pixel to avoid it getting stuck | Maximum - Divides screen width by 2
            int initYpos = rand.Next(((screen.height / -2 + 1 ) + (orangeFishHeight / 2)), 
                (screen.height / 2 - 1) - (orangeFishHeight / 2)); 
            // Same as above, for screen height and asset height

            orangeFish1 = new OrangeFish("OrangeFish", initXpos, initYpos, screen, tokenManager, rand);
            kernel.InsertToken(orangeFish1);

            for (int i = 0; i < urchinArray.Length; i++)
            {
                int UinitXpos = rand.Next(((screen.width / -2 + 1) + (urchinWidth / 2)), 
                    (screen.width / 2 - 1) - (urchinWidth / 2));
                int UinitYpos = rand.Next(((screen.height / -2 + 1) + (urchinHeight / 2)), 
                    (screen.height / -4 - 1) - (urchinHeight / 2));
                Urchin tempUrchin = new Urchin("Urchin", UinitXpos, UinitYpos, screen, tokenManager, rand);
                urchinArray[i] = tempUrchin;
                kernel.InsertToken(tempUrchin);
            }

            

        }

        /// METHOD: Update - called 60 times a second by the FishORama engine when the program is running
        /// Add all tokens so Update is called on them regularly
        public void Update(GameTime gameTime)
        {

            // *** ADD YOUR UPDATE CODE HERE ***
            // Each fish object (sitting in a variable) must have Update() called on it here

            orangeFish1.Update();

            foreach (Urchin fish in urchinArray)
            {
            fish.Update(); 
            }

        }
    }
}
