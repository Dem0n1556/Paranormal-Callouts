using System;
using LSPDFR;
using static LSPDFR.API;

namespace ParanormalCallouts
{
    public class VanishingBride : CalloutBase
    {
        private Ped ghostWoman;
        private Vehicle reporterVehicle;
        private Blip sceneBlip;
        private bool womanSpawned = false;
        private bool calloutCompleted = false;

        public override string CalloutName
        {
            get { return "The Vanishing Bride"; }
        }

        public override string CalloutDescription
        {
            get { return "A driver reports almost hitting a woman in a wedding dress. When he stopped, she vanished."; }
        }

        public override void Initialize()
        {
            // Set the callout location (Paleto Bay area)
            Vector3 calloutLocation = new Vector3(-100f, 6400f, 31f);
            Game.DisplayNotification("Responding to: " + CalloutName);
            Game.DisplayNotification(CalloutDescription);

            // Spawn the reporter's vehicle
            reporterVehicle = new Vehicle("blista", calloutLocation);
            reporterVehicle.Heading = 180f;

            // Create a blip for the scene
            sceneBlip = new Blip(calloutLocation);
            sceneBlip.Scale = 0.8f;
            sceneBlip.Color = BlipColor.Red;
            sceneBlip.Route = true;

            womanSpawned = false;
        }

        public override void Process()
        {
            // If player gets close enough to the location, spawn the ghost woman
            if (!womanSpawned && Game.LocalPlayer.Character.Position.DistanceTo(new Vector3(-100f, 6400f, 31f)) < 30f)
            {
                SpawnGhostWoman();
                womanSpawned = true;
            }

            // If the woman was spawned and player gets close, make her vanish
            if (womanSpawned && ghostWoman != null && !calloutCompleted)
            {
                if (Game.LocalPlayer.Character.Position.DistanceTo(ghostWoman.Position) < 5f)
                {
                    MakeWomanVanish();
                    calloutCompleted = true;
                    Game.DisplayNotification("The woman has vanished without a trace...");
                    Game.DisplayNotification("Complete your investigation.");
                }
            }
        }

        private void SpawnGhostWoman()
        {
            Vector3 spawnPoint = new Vector3(-95f, 6400f, 31f);
            ghostWoman = new Ped("a_f_m_business_2", spawnPoint, 0f);
            
            // Give her a wedding dress appearance (using available game models)
            Game.DisplayNotification("A woman in white appears on the roadside...");
        }

        private void MakeWomanVanish()
        {
            if (ghostWoman != null)
            {
                // Fade out effect
                ghostWoman.Alpha = 100;
                Wait(500);
                ghostWoman.Alpha = 50;
                Wait(500);
                ghostWoman.Delete();
                ghostWoman = null;
            }
        }

        public override void End()
        {
            if (ghostWoman != null) ghostWoman.Delete();
            if (reporterVehicle != null) reporterVehicle.Delete();
            if (sceneBlip != null) sceneBlip.Delete();

            Game.DisplayNotification("Callout ended. Investigation complete.");
        }
    }
}
