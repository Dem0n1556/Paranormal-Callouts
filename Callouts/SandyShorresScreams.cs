using System;
using LSPDFR;
using static LSPDFR.API;

namespace ParanormalCallouts
{
    public class SandyShorresScreams : CalloutBase
    {
        private Blip hotelBlip;
        private Ped suspiciousPerson;
        private bool personSpawned = false;
        private bool calloutCompleted = false;

        public override string CalloutName
        {
            get { return "Sandy Shores Screams"; }
        }

        public override string CalloutDescription
        {
            get { return "Anonymous reports of disturbing screams coming from the abandoned Sandy Shores hotel. Investigate the source."; }
        }

        public override void Initialize()
        {
            // Set the callout location (Sandy Shores hotel - abandoned building)
            Vector3 calloutLocation = new Vector3(1158f, 1864f, 75f);
            Game.DisplayNotification("Responding to: " + CalloutName);
            Game.DisplayNotification(CalloutDescription);
            Game.DisplayNotification("WARNING: Approach with caution. Something strange is happening.");

            // Create a blip for the hotel
            hotelBlip = new Blip(calloutLocation);
            hotelBlip.Scale = 0.8f;
            hotelBlip.Color = BlipColor.Red;
            hotelBlip.Route = true;

            personSpawned = false;
        }

        public override void Process()
        {
            // If player gets close to the hotel, something should happen
            if (!personSpawned && Game.LocalPlayer.Character.Position.DistanceTo(new Vector3(1158f, 1864f, 75f)) < 40f)
            {
                SpawnSuspiciousPerson();
                personSpawned = true;
                Game.DisplayNotification("You hear eerie screams echoing from inside the hotel...");
            }

            // If person spawned and player investigates
            if (personSpawned && suspiciousPerson != null && !calloutCompleted)
            {
                if (Game.LocalPlayer.Character.Position.DistanceTo(suspiciousPerson.Position) < 10f)
                {
                    CompleteInvestigation();
                    calloutCompleted = true;
                }
            }
        }

        private void SpawnSuspiciousPerson()
        {
            Vector3 spawnPoint = new Vector3(1165f, 1870f, 75f);
            suspiciousPerson = new Ped("a_m_m_business_1", spawnPoint, 0f);
            suspiciousPerson.Tasks.StartScenario("WORLD_HUMAN_STUPOR", 0);
            
            Game.DisplayNotification("A figure stands in the shadows of the hotel...");
        }

        private void CompleteInvestigation()
        {
            Game.DisplayNotification("The figure vanishes into the darkness of the hotel.");
            Game.DisplayNotification("Investigation complete. The case remains unsolved.");
            Game.DisplayNotification("Perhaps some mysteries are better left unsolved...");
        }

        public override void End()
        {
            if (suspiciousPerson != null) suspiciousPerson.Delete();
            if (hotelBlip != null) hotelBlip.Delete();

            Game.DisplayNotification("Callout ended. The screams have stopped.");
        }
    }
}
