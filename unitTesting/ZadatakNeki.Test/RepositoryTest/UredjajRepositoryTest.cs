using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZadatakNeki.Models;
using ZadatakNeki.Repository;

namespace ZadatakNeki.Test.RepositoryTest
{
    public class UredjajRepositoryTest
    {
        [Fact]
        public void DajSveEntitete_Test()
        {
            IQueryable<Uredjaj> lista = new List<Uredjaj>()
            {
                new Uredjaj() {Naziv = "tv"},
                new Uredjaj() {Naziv = "telefon"},
                new Uredjaj() {Naziv = "punjac"}
            }.AsQueryable();

            var mockDbSet = KancelarijaRepositoryTest.NapraviDbSet(lista);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Uredjaj>()).Returns(mockDbSet.Object);

            var repository = new UredjajRepository(mockContext.Object);
            var dobijas = repository.DajSveEntitete();

            Assert.Equal(3, dobijas.Count);
            Assert.Equal("tv", dobijas.First().Naziv);
        }

        [Theory]
        [InlineData(1)]
        public void EntitetPoId_Test(long id)
        {
            var uredjaj = new Uredjaj() {Naziv = "tv"};

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Uredjaj>().Find(It.IsAny<long>())).Returns(uredjaj);

            var repository = new UredjajRepository(mockContext.Object);
            var dobijas = repository.EntitetPoId(id);

            Assert.Equal(uredjaj, dobijas);
        }

        [Fact]
        public void DodajEntitet_Test()
        {
            var uredjaj = new Uredjaj() { Naziv = "tv" };

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Uredjaj>().Add(It.IsAny<Uredjaj>()));

            var repository = new UredjajRepository(mockContext.Object);
            repository.DodajEntitet(uredjaj);

            mockContext.Verify(e => e.Set<Uredjaj>().Add(It.IsAny<Uredjaj>()), Times.Exactly(1));
        }

        [Fact]
        public void Izmeni_Test()
        {
            var uredjaj = new Uredjaj() { Naziv = "tv" };

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Uredjaj>().Update(It.IsAny<Uredjaj>()));

            var repository = new UredjajRepository(mockContext.Object);
            repository.Izmeni(uredjaj);

            mockContext.Verify(e => e.Set<Uredjaj>().Update(It.IsAny<Uredjaj>()), Times.Exactly(1));
        }

        [Fact]
        public void ObrisiEntitet_Test()
        {
            var uredjaj = new Uredjaj() { Naziv = "tv" };

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Uredjaj>().Remove(It.IsAny<Uredjaj>()));

            var repository = new UredjajRepository(mockContext.Object);
            repository.ObrisiEntitet(uredjaj);

            mockContext.Verify(e => e.Set<Uredjaj>().Remove(It.IsAny<Uredjaj>()), Times.Exactly(1));
        }

        [Theory]
        [InlineData("digitron")]
        public void PretragaPoImenu_Test(string naziv)
        {
            IQueryable<Uredjaj> lista = new List<Uredjaj>()
            {
                new Uredjaj() {Naziv = "tv"},
                new Uredjaj() {Naziv = "avion"},
                new Uredjaj() {Naziv = "digitron"}
            }.AsQueryable();

            var mockDbSet = KancelarijaRepositoryTest.NapraviDbSet(lista);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Uredjaji).Returns(mockDbSet.Object);

            var repository = new UredjajRepository(mockContext.Object);
            var dobijas = repository.PretragaPoImenu(naziv);

            Assert.Equal("digitron", dobijas.Naziv);
        }
    }
}
