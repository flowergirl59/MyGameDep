using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame {
    public interface IWorldEvent {

        Room Trigger {
            get;
        }

        void Execute();
    }

    public interface IRoomDelegate {

        Room ContainingRoom {
            set; get;
        }

        Door GetExit(string exitName);
        string GetExits();
        string Description();
    }

    public interface ICloseable {
        bool IsClosed {
            get;
        }

        bool IsOpen {
            get;
        }


        bool Open();
        bool Close();
    }

    public interface IVisit
    {
        int NumberOfVisits { get; }
        
        bool visited();
    }

}
