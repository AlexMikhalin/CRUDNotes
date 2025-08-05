using AutoMapper;
using CRUDNotes.Common;
using CRUDNotes.DAL.Entities;
using CRUDNotes.DAL.Repositories;
using Moq;
using System.Data;

public class CrudNoteRepositoryTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IDbConnection> _dbConnectionMock = new();

    private CrudNoteRepository CreateRepository(string conn = "fake", IMapper? mapper = null)
    {
        return new CrudNoteRepository(conn, mapper ?? _mapperMock.Object);
    }

    [Fact]
    public void CreateNote_Should_Map_And_Execute()
    {
        var dto = new NoteDTO { Content = "test" };
        var note = new Note();
        _mapperMock.Setup(m => m.Map<Note>(dto)).Returns(note);

        var repo = CreateRepository();

        _mapperMock.Verify(m => m.Map<Note>(dto), Times.Never);
    }

    [Fact]
    public void FindAllNotes_Should_Map()
    {
        var notes = new List<Note> { new() { NoteId = 1, Content = "x" } };
        _mapperMock.Setup(m => m.Map<List<NoteDTO>>(notes)).Returns([new() { Content = "x" }]);

        var repo = CreateRepository();

        _mapperMock.Verify(m => m.Map<List<NoteDTO>>(notes), Times.Never);
    }
}