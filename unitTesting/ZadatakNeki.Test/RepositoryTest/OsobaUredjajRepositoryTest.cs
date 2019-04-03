using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ZadatakNeki.Models;
using ZadatakNeki.Repository;

namespace ZadatakNeki.Test.RepositoryTest
{
    public class OsobaUredjajRepositoryTest
    {
        [Fact]
        public void DajSveEntitete_Test()
        {
            IQueryable<OsobaUredjaj> lista = new List<OsobaUredjaj>()
            {
                new OsobaUredjaj(){Id = 1},
                new OsobaUredjaj(){Id = 2},
                new OsobaUredjaj(){Id = 1777}
            }.AsQueryable();

            var mockDbSet = KancelarijaRepositoryTest.NapraviDbSet(lista);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.OsobaUredjaj).Returns(mockDbSet.Object);

            var repository = new OsobaUredjajRepository(mockContext.Object);
            var dobijas = repository.DajSveEntitete();

            Assert.Equal(3, dobijas.Count);
            Assert.Equal(1777, dobijas.Last().Id);
        }

        [Theory]
        [InlineData(1)]
        public void EntitetPoId_Test(long id)
        {
            IQueryable<OsobaUredjaj> lista = new List<OsobaUredjaj>()
            {
                new OsobaUredjaj() {Id = 1, Osoba = new Osoba() {Ime = "mirsan"}},
                new OsobaUredjaj() {Id = 4}
            }.AsQueryable();

            var mockDbSet = KancelarijaRepositoryTest.NapraviDbSet(lista);

            var optiionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optiionBilder.Options);
            mockContext.Setup(e => e.OsobaUredjaj).Returns(mockDbSet.Object);

            var repository = new OsobaUredjajRepository(mockContext.Object);
            var dobijas = repository.EntitetPoId(id);

            Assert.Equal("mirsan", dobijas.Osoba.Ime);
        }

        [Fact]
        public void DodajEntitet_Test()
        {
            var osobaUredjaj = new OsobaUredjaj() {Id = 16};

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<OsobaUredjaj>().Add(It.IsAny<OsobaUredjaj>()));

            var repository = new OsobaUredjajRepository(mockContext.Object);
            repository.DodajEntitet(osobaUredjaj);

            mockContext.Verify(e => e.Set<OsobaUredjaj>().Add(It.IsAny<OsobaUredjaj>()), Times.Exactly(1));
        }

        [Fact]
        public void Izmeni_Test()
        {
            var osobaUredjaj = new OsobaUredjaj() {Id = 1};

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<OsobaUredjaj>().Update(It.IsAny<OsobaUredjaj>()));

            var repository = new OsobaUredjajRepository(mockContext.Object);
            repository.Izmeni(osobaUredjaj);

            mockContext.Verify(e => e.Set<OsobaUredjaj>().Update(It.IsAny<OsobaUredjaj>()), Times.Exactly(1));
        }

        [Fact]
        public void ObrisiEntitet_Test()
        {
            var osobaUredjaj = new OsobaUredjaj() { Id = 3};

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<OsobaUredjaj>().Remove(It.IsAny<OsobaUredjaj>()));

            var repository = new OsobaUredjajRepository(mockContext.Object);
            repository.ObrisiEntitet(osobaUredjaj);

            mockContext.Verify(e => e.Set<OsobaUredjaj>().Remove(It.IsAny<OsobaUredjaj>()), Times.Exactly(1));
        }
    }
}
