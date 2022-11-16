using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame {
    public class GameWorld {
        private static GameWorld _instance = null;
        public static GameWorld Instance {
            get {
                if (_instance == null) {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }

        private Room _entrance;
        public Room Entrance { get { return _entrance; } }
        private Room _exit;
        public Room Exit { get { return _exit; } }
        private int counter;


        /*
        private Room _trigger;
        private Room _sideA;
        private Room _sideB;
        private string _toSideA;
        private string _toSideB;
        */
        private Dictionary<Room,IWorldEvent> worldEvents;
        //private Dictionary<Room, List<IWorldEvent>> worldEvents;
        //private WorldMod worldMod;

        private GameWorld() {
            worldEvents = new Dictionary<Room, IWorldEvent>();
            CreateWorld();
            NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);
            NotificationCenter.Instance.AddObserver("PlayerWillEnterRoom", PlayerWillEnterRoom);
            counter = 0;
        }

        public void PlayerDidEnterRoom(Notification notification) {
            Player player = (Player)notification.Object;
            if (player != null) {
                if (player.CurrentRoom == Exit) {
                    player.OutputMessage("\n*** The player reached the exit ");
                    counter++;
                    if (counter == 5) {
                        //Exit.SetExit("shortcut", Entrance);
                        //Entrance.SetExit("shortcut", Exit);
                    }

                }
                if (player.CurrentRoom == Entrance) {
                    player.OutputMessage("\n *** The player returned to the entrance.");
                }

                IWorldEvent worldEvent = null;
                worldEvents.TryGetValue(player.CurrentRoom, out worldEvent);

                if (worldEvent != null) {
                    worldEvent.Execute();
                    player.OutputMessage("There is a change in the world.");
                    RemoveWorldEvent(worldEvent);

                }
            }
        }


        public void PlayerWillEnterRoom(Notification notification) {
            Player player = (Player)notification.Object;
            if (player != null) {
                if (player.CurrentRoom == Entrance) {
                    player.OutputMessage("\n>>> the player is leaving the entrance");
                }
                if (player.CurrentRoom == Exit) {
                    player.OutputMessage("\n >>> The player is going away from the exit. ");
                }
            }
        }

        public void AddWorldEvent(IWorldEvent worldEvent) {
            worldEvents[worldEvent.Trigger] = worldEvent;
        }

        private void RemoveWorldEvent(IWorldEvent worldEvent) {
            worldEvents.Remove(worldEvent.Trigger);
        }

        private void CreateWorld() {
            Room outside = new Room("outside");
            Room bank = new Room("on the front sidewalk of the bank");
            Room townHall = new Room("on the front sidewalk of the town hall");
            Room jail = new Room("on the front sidewalk of the jail");
            Room fire = new Room("on the front sidewalk of the fire department");
            Room library = new Room("on the front sidewalk of the library");
            Room saloon = new Room("on the front sidewalk of the saloon");
            Room market = new Room("in market");
            Room mamaPrissy = new Room("at mama Prissy's house");
            Room loco = new Room("at loco's house");
            Room ebbie = new Room("at Ebbie's house");
            Room postOffice = new Room("at the post office");
            Room kate = new Room("at Kate's house");

            //
            /*
            outside.SetExit("west", boulevard);
            boulevard.SetExit("east", outside);
            */

            /*
            Door door = new Door(boulevard, outside);
            boulevard.SetExit("east", door);
            outside.SetExit("west", door);
            
            boulevard.SetExit("south", scctparking);
            scctparking.SetExit("north", boulevard);
            
            boulevard.SetExit("west", theGreen);
            theGreen.SetExit("east", boulevard);
            
            boulevard.SetExit("north", universityParking);
            universityParking.SetExit("south", boulevard);
            
            scctparking.SetExit("west", scct);
            scct.SetExit("east", scctparking);
            
            scct.SetExit("north", schuster);
            schuster.SetExit("south", scct);
            
            schuster.SetExit("north", universityHall);
            universityHall.SetExit("south", schuster);
            
            schuster.SetExit("east", theGreen);
            theGreen.SetExit("west", schuster);
            
            universityHall.SetExit("east", universityParking);
            universityParking.SetExit("west", universityHall);
            */

            Door door = Door.CreateDoor(townHall, outside, "east", "west");

            
            door = Door.CreateDoor(bank, townHall, "north", "south");

            
            door = Door.CreateDoor(saloon, townHall, "east", "west");


            door = Door.CreateDoor(jail, townHall, "south", "north");

            
            door = Door.CreateDoor(library, bank, "east", "west");

            
            door = Door.CreateDoor(mamaPrissy, library, "north", "south");
            door.Close();

            
            door = Door.CreateDoor(mamaPrissy, market, "north", "south");

            
            door = Door.CreateDoor(mamaPrissy, saloon, "east", "west");

            
            door = Door.CreateDoor(jail, market, "east", "west");

            //universityParking.SetExit("north", parkingDeck);
            //parkingDeck.SetExit("south", universityParking);
            door = Door.CreateDoor(fire, jail, "south", "north");

            //Extra rooms
            Room davidson = new Room("In the Davidson Center");
            Room clockTower = new Room("at the Clock Tower");
            Room greekCenter = new Room("at the Greek Center.");
            Room woodall = new Room("at Woodall Hall");

            //Connect the special rooms
            //davidson.SetExit("west", clockTower);
            //clockTower.SetExit("east", davidson);
            door = Door.CreateDoor(clockTower, davidson, "east", "west");


            //clockTower.SetExit("north", greekCenter);
            //greekCenter.SetExit("south", clockTower);
            door = Door.CreateDoor(greekCenter, clockTower, "south", "north");

            //clockTower.SetExit("south", woodall);
            //woodall.SetExit("north", clockTower);
            door = Door.CreateDoor(woodall, clockTower, "north", "south");


            // Setup Connection

            /*
            _trigger = parkingDeck;
            _sideA = schuster;
            _sideB = davidson;
            _toSideA = "east";
            _toSideB = "west";
            */
            IWorldEvent worldMod = new WorldMod(fire, mamaPrissy, davidson, "west", "east");
            AddWorldEvent(worldMod);

            //Create and connect Lumpkin Center to Recreation Center
            Room lumpkin = new Room("in the Lumpkin Center");
            Room recreationCenter = new Room("in the Recreation Center");

            //lumpkin.SetExit("west", recreationCenter);
            //recreationCenter.SetExit("east", lumpkin);
            door = Door.CreateDoor(lumpkin, recreationCenter, "west", "east");

            worldMod = new WorldMod(library, fire, lumpkin, "north", "south");
            AddWorldEvent(worldMod);

            worldMod = new WorldMod(woodall, recreationCenter, greekCenter, "west", "east");
            AddWorldEvent(worldMod);

            //trap room time :] (delegates)
            IRoomDelegate trapRoom = new TrapRoom();
            library.RoomDelegate = trapRoom;
            //trapRoom.ContainingRoom = scct;

            IRoomDelegate echoRoom = new EchoRoom();
            fire.RoomDelegate = echoRoom;
            //echoRoom.ContainingRoom = parkingDeck;

            // Assign special rooms
            _entrance = outside;
            _exit = mamaPrissy;

            // return outside;
        }

    }
}
