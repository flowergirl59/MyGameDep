using System.Collections;
using System.Collections.Generic;
using System;

namespace MyGame
{

    public class TrapRoom : IRoomDelegate {

        public Room ContainingRoom {
            set; get;
        }

        public string UnlockWord {
            get; set;
        }

        public TrapRoom() : this("password") {

        }


        //Designated constructor
        public TrapRoom(string unlockWord) {
            UnlockWord = unlockWord;
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }

        public Door GetExit(string exitName) {
            return null;
        }
        public string GetExits() {
            return "You cannot escape.";
        }
        public string Description() {
            return "You have been trapped.";
        }

        public void PlayerDidSayWord(Notification notification) {
            Player player = (Player)notification.Object;
            if (player!= null) {
                if (player.CurrentRoom.RoomDelegate == this) {
                Dictionary<string, Object> userInfo = notification.UserInfo;
                string word = (string)userInfo["word"];
                    //player.OutputMessage("Did you say " + word + "?");
                if (word.Equals(UnlockWord)) {
                    player.OutputMessage("You said " + word + "; congratulations!");
                    player.CurrentRoom.RoomDelegate = null;
                    player.OutputMessage("\n" + player.CurrentRoom.Description());
                    NotificationCenter.Instance.RemoveObserver("PlayerDidSayWord", PlayerDidSayWord);
                    } else {
                    player.OutputMessage("you dumb cretin, you bumbling idiot, absolute buffoon\nyou will not live to see the light of day\n" + word + " is not the word");
                }
                
                }
                
            }
        }
    }

    public class EchoRoom : IRoomDelegate {

        public Room ContainingRoom {
            set; get;
        }

        public EchoRoom() {
         NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }

        public Door GetExit(string exitName) {
            ContainingRoom.RoomDelegate = null;
            Door exit = ContainingRoom.GetExit(exitName);
            ContainingRoom.RoomDelegate = this;
            return exit;
        }
        public string GetExits() {
            string exits = "";
            if (ContainingRoom.RoomDelegate != null) {
                ContainingRoom.RoomDelegate = null;
                exits += ContainingRoom.GetExits();
                ContainingRoom.RoomDelegate = this;
            }
            return exits;
        }
        public string Description() {
            string description = "You are in an Echo Room.\n";
            ContainingRoom.RoomDelegate = null;
            description += ContainingRoom.Description();
            ContainingRoom.RoomDelegate = this;
            return description;
        }

        public void PlayerDidSayWord(Notification notification) {
            Player player = (Player)notification.Object;
            if (player != null) {
                if (player.CurrentRoom.RoomDelegate == this) {
                    Dictionary<string, Object> userInfo = notification.UserInfo;
                    string word = (string)userInfo["word"];
                    player.OutputMessage("\n" + word + "... " + word + "... " + word + "... \n");

                }

            }
        }

    }

    public class Room
    {
        private Dictionary<string, Door> _exits;
        private string _tag;
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        private IRoomDelegate _roomDelegate;
        public IRoomDelegate RoomDelegate {

            set {
                _roomDelegate = value;
                if (_roomDelegate != null) {
                    _roomDelegate.ContainingRoom = this;
                }
            }

            get {
                return _roomDelegate;
            }
        }

        public Room() : this("No Tag"){}

        // Designated Constructor
        public Room(string tag)
        {
            _roomDelegate = null;
            _exits = new Dictionary<string, Door>();
            this.Tag = tag;
        }

        public void SetExit(string exitName, Door door)
        {
            _exits[exitName] = door;
        }

        public Door GetExit(string exitName)
        {
            Door door = null;
            if (_roomDelegate != null) {
                door = _roomDelegate.GetExit(exitName);
            } else {
                _exits.TryGetValue(exitName, out door);
            }
            return door;
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            if (_roomDelegate != null) {
                exitNames += _roomDelegate.GetExits();
            } else {
                Dictionary<string, Door>.KeyCollection keys = _exits.Keys;
                foreach (string exitName in keys) {
                    exitNames += " " + exitName;
                }
            }

            return exitNames;
        }

        public string Description()
        {

            return _roomDelegate!=null?_roomDelegate.Description():"You are " + this.Tag + ".\n *** " + this.GetExits();
        }
    }
}
