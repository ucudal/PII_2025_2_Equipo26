using NUnit.Framework;
using Library;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    public class SearchTests
    {
        private RepoClientes _repo;
        private Cliente _cliente;

        [SetUp]
        public void Setup()
        {
            _repo = new RepoClientes();
            _repo.Agregar("Juan", "Perez", "123456789", "juan@example.com", "Masculino", new DateTime(1990, 1, 1));
            _cliente = _repo.BuscarPorTermino("Juan")[0];
        }

        [Test]
        public void SearchByName_ShouldFindClient()
        {
            // Arrange
            string term = "Juan";

            // Act
            var results = _repo.BuscarPorTermino(term);

            // Assert
            Assert.That(results, Does.Contain(_cliente));
        }

        [Test]
        public void SearchBySurname_ShouldFindClient()
        {
            // Arrange
            string term = "Perez";

            // Act
            var results = _repo.BuscarPorTermino(term);

            // Assert
            Assert.That(results, Does.Contain(_cliente));
        }

        [Test]
        public void SearchByPhone_ShouldFindClient()
        {
            // Arrange
            string term = "123456789";

            // Act
            var results = _repo.BuscarPorTermino(term);

            // Assert
            Assert.That(results, Does.Contain(_cliente));
        }

        [Test]
        public void SearchByEmail_ShouldFindClient()
        {
            // Arrange
            string term = "juan@example.com";

            // Act
            var results = _repo.BuscarPorTermino(term);

            // Assert
            Assert.That(results, Does.Contain(_cliente));
        }

        [Test]
        public void SearchByGender_ShouldNOTFindClient()
        {
            // Arrange
            string term = "Masculino";

            // Act
            var results = _repo.BuscarPorTermino(term);

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Search_CaseInsensitive()
        {
            // Arrange
            string term = "JUAN";

            // Act
            var results = _repo.BuscarPorTermino(term);

            // Assert
            Assert.That(results, Does.Contain(_cliente));
        }

        [Test]
        public void Search_PartialMatch()
        {
            // Arrange
            string term = "uan";

            // Act
            var results = _repo.BuscarPorTermino(term);

            // Assert
            Assert.That(results, Does.Contain(_cliente));
        }
    }

    public class SearchEtiquetasTests
    {
        private RepoEtiquetas _repo;

        [SetUp]
        public void Setup()
        {
            _repo = new RepoEtiquetas();
            _repo.Crear("VIP");
        }

        [Test]
        public void SearchByName_ShouldFindEtiqueta()
        {
            var results = _repo.BuscarPorTermino("VIP");
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Nombre, Is.EqualTo("VIP"));
        }

        [Test]
        public void Search_CaseInsensitive()
        {
            var results = _repo.BuscarPorTermino("vip");
            Assert.That(results.Count, Is.EqualTo(1));
        }
    }

    public class SearchVentasTests
    {
        private RepoVentas _repo;

        [SetUp]
        public void Setup()
        {
            _repo = new RepoVentas();
            _repo.Agregar("Laptop", 1000, DateTime.Now);
        }

        [Test]
        public void SearchByProduct_ShouldFindVenta()
        {
            var results = _repo.BuscarPorTermino("Laptop");
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Producto, Is.EqualTo("Laptop"));
        }

        [Test]
        public void Search_PartialMatch()
        {
            var results = _repo.BuscarPorTermino("Lap");
            Assert.That(results.Count, Is.EqualTo(1));
        }
    }
}
