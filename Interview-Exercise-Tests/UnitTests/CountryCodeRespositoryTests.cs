using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Interview_Exercise;

namespace Interview_Exercise_Tests.UnitTests
{
    public class CountryCodeRespositoryTests: TestFixtureBase
    {
        private IStorageHandler _storageHandler;
        private CountryCodeRepository _countryCodeRepo;

        protected override void Arrange()
        {
            _storageHandler = Mock<IStorageHandler>();
            _storageHandler.Expect(x => x.ReadAll()).Return(GetCountriesList());
            _countryCodeRepo = new CountryCodeRepository(_storageHandler);
        }

        protected override void Act()
        {
            _countryCodeRepo.Add(new Country { Code = "SMC", Name = "SomeCountry" });
            _countryCodeRepo.Add(new Country { Code = "USA", Name = "UnitedStates" });
        }

        private Dictionary<string, string> GetCountriesList()
        {
            var countriesList = new Dictionary<string, string>();
            countriesList.Add("USA", "UnitedStates");
            countriesList.Add("ARM", "Armania");
            return countriesList;
        }

        [Test]
        public void Should_have_used_the_dependency()
        {
            _storageHandler.AssertWasCalled(x => x.ReadAll(), y => y.Repeat.Once());
        }

        [Test]
        public void Should_Have_Called_WriteMethod_WhilePassingValidCountryCode_ToAddMethod()
        {
            //Assert
            _storageHandler.AssertWasCalled(x => x.Write(Arg<string>.Is.Anything, Arg<string>.Is.Anything), y => y.Repeat.Once());
        }       

        [Test]
        public void Should_ThrowException_WhilePassingExistingCountryCode_ToAddMethod()
        {          
            //Assert
            AssertAll(
                 () => Assert.That(ActualException.Message, Is.Not.Null),
                 () => Assert.That(ActualException.Message, Is.EqualTo("Country already exists.")));
        }

        [Test]
        public void Should_Have_Called_UpdateMethod_WhilePassingExistingCountryCode_ToUpdateMethod()
        {
            //Act
            _countryCodeRepo.Update(new Country { Code = "USA", Name = "UnitedStatesOfAmerica" });

            //Assert
            _storageHandler.AssertWasCalled(x => x.Update(Arg<string>.Is.Anything, Arg<string>.Is.Anything), y => y.Repeat.Once());
        }       

        [Test]
        public void Should_Return_Country_WhilePassingExistingCountryCode_ToGetMethod()
        {
            //Act
            var result = _countryCodeRepo.Get("USA");

            //Assert
            Assert.IsNotNull(result);
        }
       
    }
}
