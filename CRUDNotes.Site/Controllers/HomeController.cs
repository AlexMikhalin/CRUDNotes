using AutoMapper;
using CRUDNotes.BL.Repositories;
using CRUDNotes.Common;
using CRUDNotes.Site.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDNotes.Site.Controllers
{
    public class HomeController(INoteService noteService, ILogger<HomeController> log, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(noteService.GetAllNotes());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NoteModel model)
        {
            noteService.CreateNote(new NoteDTO { Content = model.Content });
            log.LogInformation($"Note created with content: {model.Content}");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            noteService.DeleteNote(id);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var editedNote = noteService.GetNote(id);
            if (editedNote == null)
                return NotFound();

            var model = mapper.Map<NoteModel>(editedNote);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(NoteModel model)
        {
            var dto = mapper.Map<NoteDTO>(model);
            noteService.EditNote(dto);
            return RedirectToAction("Index");
        }
    }
}