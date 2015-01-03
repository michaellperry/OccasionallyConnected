using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardBoard.Models;

namespace CardBoard.BoardView
{
    public class BoardViewModel
    {
        private readonly Board _board;

        public BoardViewModel(Board board)
        {
            _board = board;
        }
    }
}
