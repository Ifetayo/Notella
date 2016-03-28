using Notella.Model;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notella.ViewModel
{
    public class NotesViewModel
    {
        public Note Note;

        public ObservableCollection<Note> Notes
        {
            get;
            private set;
        }

        public NotesViewModel()
        {
            Notes = new ObservableCollection<Note>();
            onCreate();
        }

        /**
        This method checks to see if the database has been created
        if not, it creates it.
        */
        private async void onCreate()
        {
            bool h = await CheckFileExists("NotesDB.sqlite");
            if (!h)
            {
                using (var db = new SQLiteConnection(App.dbPath))
                {
                    db.CreateTable<Note>();
                }
            }
        }

        /**
        Checks if the database file exists
        */
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                StorageFile store = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                Debug.WriteLine(store.GetType());
                return true;
            }
            catch(Exception e)
            {
                //log error here
                //exception caught  
            }
            return false;
        }

        /**Get all the notes stored*/
        public void LoadNotes()
        {
            DatabaseHelperClass databaseHelper = new DatabaseHelperClass();
            Notes = databaseHelper.GetAllNotes();
        }

        /**
        This method gets the selected note object that corresponds
        with the provided note ID from the DB
        */
        public Note SelectedNote(int selectedNoteID)
        {
            DatabaseHelperClass databaseHelper = new DatabaseHelperClass();
            Note = databaseHelper.GetSelectedNote(selectedNoteID);
            return Note;
        }

        /**
        This method adds a note to the database
        */
        public void AddNote(string newNoteText)
        {
            DatabaseHelperClass databaseHelper = new DatabaseHelperClass();
            Note note = new Note { NoteText = newNoteText, NoteDate = DateTime.Now.ToString() };
            databaseHelper.AddNote(note);
        }

        /**
        This method updates an edited note
        */
        public void UpdateNote(Note note, string updatedText)
        {
            DatabaseHelperClass databaseHelper = new DatabaseHelperClass();
            note.NoteText = updatedText;
            note.NoteDate = DateTime.Now.ToString();
            databaseHelper.UpdateNote(note);
        }

        /**
        This method deletes a note with the note ID
        */
        public void DeleteNote(int noteID)
        {
            DatabaseHelperClass databaseHelper = new DatabaseHelperClass();
            databaseHelper.DeleteNote(noteID);
        }
    }
}
