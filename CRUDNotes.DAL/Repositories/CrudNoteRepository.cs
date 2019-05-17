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
               db.Execute(sqlQuery,new Note() {Content = dto.Content, CreateDate = DateTime.Now});
            }
        }

        public List<NoteDTO> FindAllNotes()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var allNotes = db.Query<Note>("SELECT * FROM Notes").ToList();  
                List<NoteDTO> resultList = new List<NoteDTO>();

                foreach (var n in allNotes)
                {
                    resultList.Add(new NoteDTO() {Content = n.Content, CreateDate = n.CreateDate, NoteId = n.NoteId});
                }

                return resultList;
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
                    return new NoteDTO()
                        {Content = noteEntity.Content, NoteId = noteEntity.NoteId, CreateDate = noteEntity.CreateDate};
                return null;
            }
        }

        public void EditNote(NoteDTO dto)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Notes SET Content = @Content, CreateDate = @CreateDate WHERE NoteId = @NoteId";
                db.Execute(sqlQuery,new Note(){NoteId = dto.NoteId, Content = dto.Content, CreateDate = DateTime.Now});
            }
        }
    }
}
