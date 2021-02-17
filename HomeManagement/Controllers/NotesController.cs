using HomeManagement.Models;
using HomeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Controllers
{
    [ApiController]
    [Route("/Notes")]
    public class NotesController : Controller
    {
        private readonly NotesService _notesService;
        public NotesController(NotesService notesService)
        {
            _notesService = notesService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromForm] NoteCreate model)
        {
            var result = await _notesService.CreateNote(model);
            if (!result.Success)
                return BadRequest(result.Exception);

            return Json(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var result = await _notesService.GetNoteById(id);
            if (result == null)
                return BadRequest("inexistent_note");

            return Json(result);
        }

        [Authorize]
        [HttpGet("{id}/file")]
        public async Task<IActionResult> GetNoteFileById(int id)
        {
            var result = await _notesService.GetNoteFileById(id);
            if (!result.Success)
                return BadRequest(result.Exception);

            return File(System.IO.File.ReadAllBytes(result.SubPath), result.ContentType);
        }
    }
}
