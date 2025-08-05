using AutoMapper;
using CRUDNotes.BL.Repositories;
using CRUDNotes.Common;
using CRUDNotes.Site.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDNotes.Site.Controllers
{
    public class HomeController(INoteService noteService, ILogger<HomeController> log) : Controller
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
            noteService.CreateNote(new NoteDTO() { Content = model.Content });
            log.LogInformation($"File created with content{model.Content}");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                noteService.DeleteNote(id);
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        public IActionResult Edit(int id)
        {
            var editedNote = noteService.GetNote(id);
            return View(Mapper.Map(editedNote,new NoteModel()));
        }

        [HttpPost]
        public IActionResult Edit(NoteModel model)
        {

            noteService.EditNote(Mapper.Map(model,new NoteDTO()));

            return RedirectToAction("Index");
        }
    }
}