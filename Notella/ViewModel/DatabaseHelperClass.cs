using Notella.Model;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Notella.ViewModel
{
    /**
    This class sserves as a helpher class for all database operation the view model might need
    */
    public class DatabaseHelperClass
    {
        public Note GetSelectedNote(int noteID)
        {
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                var existingNote = dbConn.Query<Note>("SELECT * FROM notes WHERE NoteID = " + noteID).FirstOrDefault();
                return existingNote;               
            }
        }

        public ObservableCollection<Note> GetAllNotes()
        {
            using (var dbConn = new SQLiteConnection(App.dbPath))
            {
                ObservableCollection<Model.Note> ListOfNotes = new ObservableCollection<Model.Note>(dbConn.Table<Model.Note>().OrderByDescending(d => d.NoteID));
                return ListOfNotes;
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
