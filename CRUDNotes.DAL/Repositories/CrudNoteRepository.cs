using CRUDNotes.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using CRUDNotes.DAL.EF;
using CRUDNotes.DAL.Entities;
using Dapper;
using Remotion.Linq.Clauses;
using AutoMapper;

namespace CRUDNotes.DAL.Repositories
{
    public class CrudNoteRepository : ICrudNoteRepository
    {

        string connectionString = null;

        public CrudNoteRepository(string conn)
        {
            connectionString = conn;
        }
        public void CreateNote(NoteDTO dto)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
               var sqlQuery = "INSERT INTO Notes (Content,CreateDate) VALUES (@Content, @CreateDate)";
               var note = Mapper.Map(dto, new Note());
               note.CreateDate = DateTime.Now;
               db.Execute(sqlQuery, note);
            }
        }

        public List<NoteDTO> FindAllNotes()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return Mapper.Map<IEnumerable<Note>,List<NoteDTO>>(db.Query<Note>("SELECT * FROM Notes").ToList());  
            }
        }

        public void DeleteNote(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Notes WHERE NoteId = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public NoteDTO FindNote(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Note noteEntity =  db.Query<Note>("SELECT * FROM Notes WHERE NoteId = @id", new { id }).FirstOrDefault();
                if (noteEntity != null)
                    return Mapper.Map(noteEntity,new NoteDTO());
                return null;
            }
        }

        public void EditNote(NoteDTO dto)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Notes SET Content = @Content, CreateDate = @CreateDate WHERE NoteId = @NoteId";
                var note = Mapper.Map(dto, new Note());
                note.CreateDate = DateTime.Now;
                db.Execute(sqlQuery, note);
            }
        }
    }
}
