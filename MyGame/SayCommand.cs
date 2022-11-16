using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame {
    public class SayCommand : Command {
        //SAY IT AIN'T SOoOoO
        public SayCommand() : base() {
            this.Name = "say";
        }

        override
        public bool Execute(Player player) {
            if (this.HasSecondWord()) {
                player.Say(this.SecondWord);
            } else {
                player.OutputMessage("\nWhat are you trying to say?");
            }
            return false;
        }
    }
}
