using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notella.Model
{
    [Table("notes")]
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int NoteID { get; private set; }

        private string noteText;

        [Column("noteText")]
        public string NoteText
        {
            get
            {
                return noteText;
            }
            set
            {
                if (noteText !=value)
                {
                    noteText = value;
                }
            }
        }

        private string noteDate;

        [Column("noteDate")]
        public string NoteDate
        {
            get
            {
                return noteDate;
            }
            set
            {
                if (noteDate != value)
                {
                    noteDate = value;
                }
            }
        }
    }
}
