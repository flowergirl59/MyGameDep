using System.Collections;
using System.Collections.Generic;
using System;

namespace MyGame
{
    public class Player
    {
        private Room _currentRoom = null;
        public Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
            set
            {
                _currentRoom = value;
            }
        }

        public Player(Room room)
        {
            _currentRoom = room;
        }

        public void WaltTo(string direction)
        {
            Door nextDoor = this.CurrentRoom.GetExit(direction);
            if (nextDoor != null)
            {
                if (nextDoor.IsOpen) {
                Room nextRoom = nextDoor.RoomOnTheOtherSideOf(CurrentRoom);
                NotificationCenter.Instance.PostNotification(new Notification("PlayerWillEnterRoom", this));
                this.CurrentRoom = nextRoom;
                NotificationCenter.Instance.PostNotification(new Notification("PlayerHasEnteredRoom", this));
                this.OutputMessage("\n" + this.CurrentRoom.Description());
                } else {
                    OutputMessage("\nThe door on " + direction + " is closed.");
                }

                
            }
            else
            {
                this.OutputMessage("\nThere is no door on " + direction);
            }
        }

        public void Say(string word) {
            OutputMessage("Player said " + word);
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            userInfo["word"] = word;
            Notification notification = new Notification("PlayerDidSayWord", this, userInfo);
            NotificationCenter.Instance.PostNotification(notification);
        }

        public void Open(string doorName) {
            Door door = CurrentRoom.GetExit(doorName);
            if (door != null) {
                if (door.IsOpen) {
                OutputMessage("\nThe door on " + doorName + " is already open.");
            } else {
                if (door.Open()) {
                OutputMessage("\nThe door on " + doorName + " is now open.");
                } else {
                    OutputMessage("\nThe door on " + doorName + " cannot be opened.");
                }
                
            }
                    
            } else {
                this.OutputMessage("\nThere is no door on " + doorName);
            }

        }

            public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
    }

}
