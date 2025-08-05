using AutoMapper;
using CRUDNotes.Common;
using CRUDNotes.DAL.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUDNotes.DAL.Repositories
{
    public class CrudNoteRepository(string conn, IMapper mapper) : ICrudNoteRepository
    {
        public void CreateNote(NoteDTO? dto)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var sqlQuery = "INSERT INTO Notes (Content,CreateDate) VALUES (@Content, @CreateDate)";
                var note = mapper.Map<Note>(dto);
                note.CreateDate = DateTime.Now;
                db.Execute(sqlQuery, note);
            }
        }

        public List<NoteDTO> FindAllNotes()
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var notes = db.Query<Note>("SELECT * FROM Notes").ToList();
                return mapper.Map<List<NoteDTO>>(notes);
            }
        }

        public void DeleteNote(int id)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var sqlQuery = "DELETE FROM Notes WHERE NoteId = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public NoteDTO? FindNote(int id)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var noteEntity = db.QueryFirstOrDefault<Note>("SELECT * FROM Notes WHERE NoteId = @id", new { id });
                return noteEntity != null ? mapper.Map<NoteDTO>(noteEntity) : null;
            }
        }

        public void EditNote(NoteDTO dto)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                var sqlQuery = "UPDATE Notes SET Content = @Content, CreateDate = @CreateDate WHERE NoteId = @NoteId";
                var note = mapper.Map<Note>(dto);
                note.CreateDate = DateTime.Now;
                db.Execute(sqlQuery, note);
            }
        }
    }
}
