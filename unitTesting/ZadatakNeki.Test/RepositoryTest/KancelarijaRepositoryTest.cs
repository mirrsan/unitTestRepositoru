using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using ZadatakNeki.Models;
using ZadatakNeki.Repository;

namespace ZadatakNeki.Test.RepositoryTest
{
    public class KancelarijaRepositoryTest
    {
        public static Mock<DbSet<T>> NapraviDbSet<T>(IQueryable<T> data) where T : class
        {
            var qData = data.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(e => e.Provider).Returns(qData.Provider);
            mockSet.As<IQueryable<T>>().Setup(e => e.Expression).Returns(qData.Expression);
            mockSet.As<IQueryable<T>>().Setup(e => e.ElementType).Returns(qData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(e => e.GetEnumerator()).Returns(qData.GetEnumerator());

            return mockSet;
        }

        [Fact]
        public void DajSveEntitete_Test()
        {
            IQueryable<Kancelarija> ka = new List<Kancelarija>()
            {
                new Kancelarija() {Opis = "kantina"},
                new Kancelarija() {Opis = "marketing"},
                new Kancelarija() {Opis = "programming"}
            }.AsQueryable();

            var mockSet = NapraviDbSet(ka);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Kancelarija>()).Returns(mockSet.Object);

            var repository = new KancelarijaRepository(mockContext.Object);
            var dobijas = repository.DajSveEntitete();

            Assert.Equal(3, dobijas.Count);
            Assert.Equal("kantina", dobijas.First().Opis);
        }

        [Theory]
        [InlineData(3)]
        public void EntitetPoId_Test(long id)
        {
            var kancelarija = new Kancelarija() {Opis = "programming"};

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Kancelarija>().Find(It.IsAny<long>())).Returns(kancelarija);

            var repository = new KancelarijaRepository(mockContext.Object);
            var dobijas = repository.EntitetPoId(id);

            Assert.Equal(kancelarija, dobijas);
        }

        [Fact]
        public void DodajEntitet_Test()
        {
            var kancelarija = new Kancelarija() {Opis = "programming"};

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Kancelarija>().Add(It.IsAny<Kancelarija>()));

            var repository = new KancelarijaRepository(mockContext.Object);
            repository.DodajEntitet(kancelarija);

            mockContext.Verify(e => e.Set<Kancelarija>().Add(It.IsAny<Kancelarija>()), Times.Exactly(1));
        }

        [Fact]
        public void Izmeni_Test()
        {
            var kancelarija = new Kancelarija() { Opis = "programming" };

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Kancelarija>().Update(It.IsAny<Kancelarija>()));

            var repository = new KancelarijaRepository(mockContext.Object);
            repository.Izmeni(kancelarija);

            mockContext.Verify(e => e.Set<Kancelarija>().Update(It.IsAny<Kancelarija>()), Times.Exactly(1));
        }

        [Fact]
        public void ObrisiEntitet_Test()
        {
            var kancelarija = new Kancelarija() { Opis = "programming" };

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Set<Kancelarija>().Remove(It.IsAny<Kancelarija>()));

            var repository = new KancelarijaRepository(mockContext.Object);
            repository.ObrisiEntitet(kancelarija);

            mockContext.Verify(e => e.Set<Kancelarija>().Remove(It.IsAny<Kancelarija>()), Times.Exactly(1));
        }

        [Theory]
        [InlineData("marketing")]
        public void PretragaPoNazivu_Test(string opis)
        {
            IQueryable<Kancelarija> lista = new List<Kancelarija>()
            {
                new Kancelarija() {Opis = "dizajn"},
                new Kancelarija() {Opis = "marketing"}
            }.AsQueryable();

            var mockDbSet = NapraviDbSet(lista);

            var optionBilder = new DbContextOptionsBuilder<ToDoContext>();

            var mockContext = new Mock<ToDoContext>(optionBilder.Options);
            mockContext.Setup(e => e.Kancelarije).Returns(mockDbSet.Object);

            var repository = new KancelarijaRepository(mockContext.Object);

            var dobijas = repository.PretragaPoNazivu(opis);

            Assert.Equal(opis, dobijas.Opis);
        }
    }
}
