using System;
using LSPDFR;
using static LSPDFR.API;

namespace ParanormalCallouts
{
    public class BigfootSighting : CalloutBase
    {
        private Ped bigfoot;
        private Ped hiker;
        private Blip sceneBlip;
        private Blip bigfootBlip;
        private bool bigfootSpawned = false;
        private bool calloutCompleted = false;

        public override string CalloutName
        {
            get { return "Bigfoot Sighting"; }
        }

        public override string CalloutDescription
        {
            get { return "A hiker reports seeing a large, hairy creature in the wilderness matching the description of Bigfoot."; }
        }

        public override void Initialize()
        {
            // Set the callout location (Chiliad mountain area)
            Vector3 calloutLocation = new Vector3(425f, 5600f, 785f);
            Game.DisplayNotification("Responding to: " + CalloutName);
            Game.DisplayNotification(CalloutDescription);

            // Spawn the hiker
            hiker = new Ped("a_m_m_business_1", calloutLocation, 0f);
            hiker.Tasks.StartScenario("WORLD_HUMAN_STUPOR", 0);

            // Create a blip for the scene
            sceneBlip = new Blip(calloutLocation);
            sceneBlip.Scale = 0.8f;
            sceneBlip.Color = BlipColor.Red;
            sceneBlip.Route = true;

            bigfootSpawned = false;
        }

        public override void Process()
        {
            // If player gets close to the hiker, spawn Bigfoot nearby
            if (!bigfootSpawned && Game.LocalPlayer.Character.Position.DistanceTo(new Vector3(425f, 5600f, 785f)) < 50f)
            {
                SpawnBigfoot();
                bigfootSpawned = true;
                Game.DisplayNotification("You spot a massive figure moving through the trees!");
            }

            // If Bigfoot spawned, make it flee when player gets close
            if (bigfootSpawned && bigfoot != null && !calloutCompleted)
            {
                if (Game.LocalPlayer.Character.Position.DistanceTo(bigfoot.Position) < 30f)
                {
                    bigfoot.Tasks.StartScenario("WORLD_HUMAN_STUPOR", 0);
                    bigfoot.Tasks.RunTowardsPosition(new Vector3(500f, 5700f, 800f), 30000, true);
                    
                    calloutCompleted = true;
                    Game.DisplayNotification("The creature flees into the wilderness!");
                    Game.DisplayNotification("Investigation complete.");
                }
            }
        }

        private void SpawnBigfoot()
        {
            Vector3 spawnPoint = new Vector3(450f, 5650f, 790f);
            // Using a large ped model to simulate Bigfoot
            bigfoot = new Ped("a_m_m_business_2", spawnPoint, 180f);
            
            // Create a blip for Bigfoot
            if (bigfootBlip != null) bigfootBlip.Delete();
            bigfootBlip = new Blip(spawnPoint);
            bigfootBlip.Scale = 1.0f;
            bigfootBlip.Color = BlipColor.Yellow;
        }

        public override void End()
        {
            if (bigfoot != null) bigfoot.Delete();
            if (hiker != null) hiker.Delete();
            if (sceneBlip != null) sceneBlip.Delete();
            if (bigfootBlip != null) bigfootBlip.Delete();

            Game.DisplayNotification("Callout ended. Bigfoot remains unproven.");
        }
    }
}
