using Notella.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notella.ViewModel
{
    public class NotesViewModel
    {
        //public ObservableCollection<Note> Notes;
        static int count = 1;
        public Note Note;
        //public static string dbPath = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "NotesDB.sqlite"));

        public ObservableCollection<Note> Notes
        {
            get;
            private set;
        }

        public NotesViewModel()
        {
            Notes = new ObservableCollection<Note>();
            onCreate();
           
            //using (var db = new SQLiteConnection(App.dbPath))
            //{
            //    db.CreateTable<Note>();
            //}
        }

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

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                StorageFile store = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                Debug.WriteLine(store.GetType());
                return true;
            }
            catch
            {
                
            }
            return false;
        }

        //private async Task<bool> CheckFileExists(string fileName)
        //{
        //    try
        //    {
        //        var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
        //        return true;
        //    }
        //    catch
        //    {

        //    }
        //    return false;
        //}

        public void loadNote()
        {
            DatabaseHelperClass dh = new DatabaseHelperClass();
            //dh.Insert();
            Notes = dh.ReadNotes();
            //for (int i = 0; i < 10; i++)
            //{
            //    Note = new Note { NoteText = "Note " + count++, NoteDate = DateTime.Now.ToString() };
            //    Notes.Add(Note);
            //}
        }

        public Note SelectedNote(int selectedNoteID)
        {
            DatabaseHelperClass dh = new DatabaseHelperClass();
            Note = dh.GetSelectedNote(selectedNoteID);
            return Note;
        }

        public void AddNote(string newNoteText)
        {
            DatabaseHelperClass dh = new DatabaseHelperClass();
            Note note = new Note { NoteText = newNoteText, NoteDate = DateTime.Now.ToString() };
            dh.AddNote(note);
        }

        public void UpdateNote(Note note, string updatedText)
        {
            DatabaseHelperClass dh = new DatabaseHelperClass();
            note.NoteText = updatedText;
            note.NoteDate = DateTime.Now.ToString();
            dh.UpdateNote(note);
        }

        public void DeleteNote(int noteID)
        {
            DatabaseHelperClass dh = new DatabaseHelperClass();
            dh.DeleteNote(noteID);
        }
    }
}
