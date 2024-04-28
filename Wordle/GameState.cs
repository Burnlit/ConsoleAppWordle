﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    public enum GameState
    {
        RoundStarted,
        WaitingForUserInput,
        CheckingUserInput,
        RoundWin,
        RoundLoss,
        GameOver
    }
}
