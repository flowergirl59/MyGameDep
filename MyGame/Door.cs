using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame {
    public class Door : ICloseable {
        private Room _roomA;
        private Room _roomB;
        private bool _closed;

        public Door(Room roomA, Room roomB) {
            _roomA = roomA;
            _roomB = roomB;
            _closed = false;
        }

        public Room RoomOnTheOtherSideOf(Room room) {
            Room theOtherRoom = null;
            if (room == _roomA) {
                theOtherRoom = _roomB;
            }

            if (room == _roomB) {
                theOtherRoom = _roomA;
            }

            return theOtherRoom;
        }

        public static Door CreateDoor(Room roomA, Room roomB, string toRoomA, string toRoomB) {
            Door door = new Door(roomA, roomB);
            roomA.SetExit(toRoomA, door);
            roomB.SetExit(toRoomB, door);
            return door;
        }

        public bool IsClosed {
            get {
                return _closed;
            }
        }

        public bool IsOpen {
            get {
                return !_closed;
            }
        }

        public bool Open() {
            bool result = true;
            _closed = false;
            return result;
        }
        public bool Close() {
            bool result = true;
            _closed = true;
            return result;
        }


    }
}
