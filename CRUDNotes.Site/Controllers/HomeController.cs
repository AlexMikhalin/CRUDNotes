using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRUDNotes.BL.Repositories;
using CRUDNotes.Common;
using CRUDNotes.Site.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRUDNotes.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly INoteService _noteService;
        private readonly ILogger<HomeController> _log;

        public HomeController(INoteService noteService, ILogger<HomeController> log)
        {
            _noteService = noteService;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(_noteService.GetAllNotes());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NoteModel model)
        { 
            _noteService.CreateNote(new NoteDTO() { Content = model.Content });
            _log.LogInformation($"File created with content{model.Content}");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                _noteService.DeleteNote(id);
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        public IActionResult Edit(int id)
        {
            var editedNote = _noteService.GetNote(id);
            return View(Mapper.Map(editedNote,new NoteModel()));
        }

        [HttpPost]
        public IActionResult Edit(NoteModel model)
        {

            _noteService.EditNote(Mapper.Map(model,new NoteDTO()));

            return RedirectToAction("Index");
        }
    }
}