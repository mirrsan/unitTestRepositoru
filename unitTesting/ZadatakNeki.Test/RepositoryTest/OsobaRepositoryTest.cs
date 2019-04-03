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
    public class OsobaRepositoryTest
    {
        [Fact]
        public void DajSveEntitete_Test()
        {
            IQueryable<Osoba> lista = new List<Osoba>()
            {
                new Osoba() {Ime = "mirsan", Prezime = "kajovic", Kancelarija = new Kancelarija() {Opis = "kuca"}},
                new Osoba() {Ime = "neko", Prezime = "nekic", Kancelarija = new Kancelarija() {Opis = "market"}},
                new Osoba() {Ime = "samra", Prezime = "kajevic", Kancelarija = new Kancelarija() {Opis = "sala"}}
            }.AsQueryable();

            var mockDbSet = KancelarijaRepositoryTest.NapraviDbSet(lista);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Osobe).Returns(mockDbSet.Object);

            var repository = new OsobaRepository(mockContext.Object);
            var dobijas = repository.DajSveEntitete();

            Assert.Equal(3 , dobijas.Count);
            Assert.Equal("sala", dobijas.Last().Kancelarija.Opis);
        }

        [Theory]
        [InlineData(4)]
        public void EntitetPoId_Test(long id)
        {
            IQueryable<Osoba> lista = new List<Osoba>()
            {
                new Osoba() {Ime = "mirsan", Prezime = "kajovic", Kancelarija = new Kancelarija() {Opis = "kuca"}},
                new Osoba() {Ime = "neko", Prezime = "nekic", Kancelarija = new Kancelarija() {Opis = "market"}},
                new Osoba() {Id = 4, Ime = "sw", Prezime = "s", Kancelarija = new Kancelarija() {Opis = "sala"}}
            }.AsQueryable();

            var mockDbSet = KancelarijaRepositoryTest.NapraviDbSet(lista);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Osobe).Returns(mockDbSet.Object);

            var repository = new OsobaRepository(mockContext.Object);
            var dobijas = repository.EntitetPoId(id);

            Assert.Equal("sw", dobijas.Ime);
            Assert.Equal("sala", dobijas.Kancelarija.Opis);
        }

        [Fact]
        public void DodajEntitet_Test()
        {
            var osoba = new Osoba() {Ime = "mirsan"};

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Osoba>().Add(It.IsAny<Osoba>()));

            var repository = new OsobaRepository(mockContext.Object);
            repository.DodajEntitet(osoba);

            mockContext.Verify(e => e.Set<Osoba>().Add(It.IsAny<Osoba>()), Times.Exactly(1));
        }

        [Fact]
        public void Izmeni_Test()
        {
            var osoba = new Osoba() { Ime = "mirsan" };

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Osoba>().Update(It.IsAny<Osoba>()));

            var repository = new OsobaRepository(mockContext.Object);
            repository.Izmeni(osoba);

            mockContext.Verify(e => e.Set<Osoba>().Update(It.IsAny<Osoba>()), Times.Exactly(1));
        }

        [Fact]
        public void ObrisiEntitet_Test()
        {
            var osoba = new Osoba() { Ime = "mirsan" };

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Osoba>().Remove(It.IsAny<Osoba>()));

            var repository = new OsobaRepository(mockContext.Object);
            repository.ObrisiEntitet(osoba);

            mockContext.Verify(e => e.Set<Osoba>().Remove(It.IsAny<Osoba>()), Times.Exactly(1));
        }

        [Theory]
        [InlineData("mirsan")]
        public void PretragaPoImenu_Test(string ime)
        {
            IQueryable<Osoba> lista = new List<Osoba>()
            {
                new Osoba() {Ime = "mirsan", Prezime = "kaj"},
                new Osoba() {Ime = "neko"},
                new Osoba() {Ime = "samra"}
            }.AsQueryable();

            var mockDbSet = KancelarijaRepositoryTest.NapraviDbSet(lista);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Osobe).Returns(mockDbSet.Object);

            var repository = new OsobaRepository(mockContext.Object);
            var dobijas = repository.PretragaPoImenu(ime);

            Assert.Equal("kaj", dobijas.Prezime);
        }
    }
}
