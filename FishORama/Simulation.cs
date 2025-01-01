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
            
        Random rand; // Declares rand variable

            // Orange Fish
        int orangeFishHeight = 86;
        int orangeFishWidth = 128;
        
        OrangeFish orangeFish1;

            // Urchin
        int urchinHeight = 112;
        int urchinWidth = 180;
        
        Urchin[] urchinArray = new Urchin[3]; // Creates array and defines size

        // Seahorse
        int seahorseHeight = 128;
        int seahorseWidth = 74;

        Seahorse[] seahorseArray = new Seahorse[5]; // Creates array and defines size

            // Piranha
        int piranha1Height = 128;
        int piranha1Width = 132; 

        Piranha1 piranha1;

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

            // Orange Fish
            int initXpos = rand.Next(((screen.width / -2 + 1) + (orangeFishWidth / 2)), 
                (screen.width / 2 - 1) - (orangeFishWidth / 2)); 
                /* Minimum - Divides screen width by 2, then makes it a negative int. 
                Also account for asset size and adjusts by a pixel to avoid it getting stuck | Maximum - Divides screen width by 2 */

            int initYpos = rand.Next(((screen.height / -2 + 1 ) + (orangeFishHeight / 2)), 
                (screen.height / 2 - 1) - (orangeFishHeight / 2)); 
                // Same as above, for screen height and asset height

            orangeFish1 = new OrangeFish("OrangeFish", initXpos, initYpos, screen, tokenManager, rand); // Single Orange Fish token
            kernel.InsertToken(orangeFish1);

            // Piranha1
            int P1initXpos = rand.Next(((screen.width / -2 + 1) + (piranha1Width / 2)),
                (screen.width / 2 - 1) - (piranha1Width / 2));
            
            int P1initYpos = rand.Next((((screen.height / 2) - (screen.height * 2 / 3)) + (piranha1Height / 2)),
                (screen.height / 2 - 1) - (piranha1Height / 2));

            piranha1 = new Piranha1("Piranha1", P1initXpos, P1initYpos, screen, tokenManager, rand); // Single Piranha1 token
            kernel.InsertToken(piranha1);


            // Urchin
            for (int i = 0; i < urchinArray.Length; i++)
            {
                int tempInitXpos = rand.Next(((screen.width / -2 + 1) + (urchinWidth / 2)), // Random starting position within constraints of screen (and accounting for asset size)
                    (screen.width / 2 - 1) - (urchinWidth / 2));
                int tempInitYpos = rand.Next(((screen.height / -2 + 1) + (urchinHeight / 2)), 
                    (screen.height / -4 - 1) - (urchinHeight / 2));
                Urchin tempUrchin = new Urchin("Urchin", tempInitXpos, tempInitYpos, screen, tokenManager, rand);
                urchinArray[i] = tempUrchin;
                kernel.InsertToken(tempUrchin);
            }

            // Seahorse
            for (int i = 0; i < seahorseArray.Length; i++)
            {
                int SinitXpos = rand.Next(((screen.width / -2 + 1) + (urchinWidth / 2)), // Random starting position within constraints of screen (and accounting for asset size)
                    (screen.width / 2 - 1) - (urchinWidth / 2));
                int SinitYpos = rand.Next(((screen.height / -2 + 1) + (seahorseHeight / 2)),
                (screen.height / 2 - 1) - (seahorseHeight / 2));
                Seahorse tempSeahorse = new Seahorse("Seahorse", SinitXpos, SinitYpos, screen, tokenManager, rand);
                seahorseArray[i] = tempSeahorse;
                kernel.InsertToken(tempSeahorse);
            }


        }

        /// METHOD: Update - called 60 times a second by the FishORama engine when the program is running
        /// Add all tokens so Update is called on them regularly
        public void Update(GameTime gameTime)
        {

            // *** ADD YOUR UPDATE CODE HERE ***
            // Each fish object (sitting in a variable) must have Update() called on it here

            orangeFish1.Update(); // Update orange fish

            piranha1.Update(); // Update piranha

            foreach (Urchin fish in urchinArray) // For each fish in urchin array
            {
            fish.Update(); // Update it
            }

            foreach (Seahorse fish in seahorseArray) // For each fish in seahorse array
            { 
            fish.Update(); // Update it
            }
        }
    }
}
