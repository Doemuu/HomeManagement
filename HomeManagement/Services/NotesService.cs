using HomeManagement.Connector;
using HomeManagement.Entities;
using HomeManagement.Models;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public class NotesService
    {
        private readonly IDatabaseConnector _databaseConnector;

        public NotesService(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
        }

        public async Task<ConnectorResult> CreateNote(NoteCreate note)
        {
            Note n = new Note();
            n.NoteTitle = note.NoteTitle;
            n.UploadDate = DateTime.UtcNow;
            n.IsDeleted = false;
            var fetchedNote = await _databaseConnector.FetchNoteByTitle(note.NoteTitle);
            if (fetchedNote != null)
                return new ConnectorResult { Success = false, Exception = "title_already_exists" };

            if (note.NoteFile != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                var pathString = Path.Combine(filePath, "file_" + note.NoteTitle);

                using (Stream stream = new FileStream(pathString, FileMode.Create))
                    note.NoteFile.CopyTo(stream);
                n.Ending = note.NoteFile.ContentType;
            }
            var result = await _databaseConnector.CreateNote(n);

            return new ConnectorResult { Success = result.Success, Exception = result.Exception };
        }

        public async Task<Note> GetNoteById(int id)
        {
            if (id < 0)
                return null;

            var result = await _databaseConnector.GetNoteById(id);

            return result;
        }

        public async Task<FileResult> GetNoteFileById(int id)
        {
            if (id < 0)
                return new FileResult { Success = false, Exception = "invalid_id" };

            var pathString = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(pathString))
                return new FileResult { Success = false, Exception= "internal_error" };

            var note = await _databaseConnector.GetNoteById(id);

            var filePath = Path.Combine(pathString, "file_" + note.NoteTitle);

            if (!System.IO.File.Exists(filePath))
                return new FileResult { Success = false, Exception = "inexistent_file" };


            return new FileResult {Success= true, SubPath = filePath, ContentType = note.Ending };
        }
    }
}
