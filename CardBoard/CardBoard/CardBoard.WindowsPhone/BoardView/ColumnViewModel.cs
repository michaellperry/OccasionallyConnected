using CardBoard.Models;

namespace CardBoard.BoardView
{
    public class ColumnViewModel
    {
        private readonly Column _column;

        public ColumnViewModel(Column column)
        {
            _column = column;
        }

        public Column Column
        {
            get { return _column; }
        }

        public string Name
        {
            get
            {
                if (_column == Column.ToDo)
                    return "To Do";
                else if (_column == Column.Doing)
                    return "Doing";
                else
                    return "Done";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (ColumnViewModel)obj;
            return this._column == that._column;
        }

        public override int GetHashCode()
        {
            return _column.GetHashCode();
        }
    }
}
