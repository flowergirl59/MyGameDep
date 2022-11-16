using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame
{
    public class Visit : IVisit
    {
        private bool _visit;
        private int _numberOfVisits;

        public int NumberOfVisits{ get { return _numberOfVisits; } }


        public bool visited()
        {
            return _visit;
        }
    }
}
