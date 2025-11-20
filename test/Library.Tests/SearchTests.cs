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
}
