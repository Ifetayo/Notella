using Notella.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notella.ViewModel
{
    public class DatabaseHelperClass
    {
        static int count = 1;

        public Note GetSelectedNote(int noteID)
        {
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                var existingNote = dbConn.Query<Note>("SELECT * FROM notes WHERE NoteID = " + noteID).FirstOrDefault();
                return existingNote;               
            }
        }

        public ObservableCollection<Model.Note> ReadNotes()
        {
            Insert();
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                //List<Notes> myCollection = dbConn.Table<Notes>().ToList();
                ObservableCollection<Model.Note> NotesList = new ObservableCollection<Model.Note>(dbConn.Table<Model.Note>().OrderByDescending(d => d.NoteDate));
                return NotesList;
            }
        }

        public void UpdateNote(Note note)
        {
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                var existingNote = dbConn.Query<Note>("SELECT * FROM notes WHERE noteid = " + note.NoteID).FirstOrDefault();
                if (existingNote != null)
                {
                    existingNote.NoteText = note.NoteText;
                    existingNote.NoteDate = DateTime.Now.ToString();
                    dbConn.RunInTransaction(() => 
                    {
                        dbConn.Update(existingNote);
                    });
                }
            }
        }

        public void AddNote(Note newNote)
        {
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newNote);
                });
            }
        }

        public void Insert()
        {
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                dbConn.RunInTransaction(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        dbConn.Insert(new Note { NoteText = "notes from db " + count++, NoteDate = DateTime.Now.ToString() });
                    }
                });
            }
        }

        public void DeleteNote(int noteID)
        {
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                var existingNote = dbConn.Query<Note>("SELECT * FROM notes where NoteID = " + noteID).FirstOrDefault();
                if (existingNote != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingNote);
                    });
                }
            }
        }
    }
}
