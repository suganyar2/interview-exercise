using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Interview_Exercise
{
    public interface ICountryCodeRepository
    {
        void Add(Country country);
        void Update(Country country);
        void Delete(string countryCode);
        Country Get(string countryCode);
        void Clear();
    }

    public class CountryCodeRepository : ICountryCodeRepository
    {
        IStorageHandler _storageHandler = null;
        Dictionary<string, string> _countriesList;
        string noCountryExist = "Country does not exist.";
        string countryExists = "Country already exists.";
        string countryCodeRegx = "^[a-zA-Z]{3}$";
        string invalidCountyCode = "Invalid Country Code.";

        public CountryCodeRepository(IStorageHandler storageHandler)
        {
            _storageHandler = storageHandler;
            _countriesList = _storageHandler.ReadAll();
        }

        public void Add(Country country)
        {
            Regex rx = new Regex(countryCodeRegx, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if(!rx.IsMatch(country.Code))
            {
                throw new Exception(invalidCountyCode);
            }
            if (_countriesList.ContainsKey(country.Code.ToUpper()))
            {
                throw new Exception(countryExists);
            }
            _storageHandler.Write(country.Code.ToUpper(), country.Name);
            _countriesList.Add(country.Code, country.Name);           
        }

        public void Clear()
        {
            _storageHandler.RemoveAll();
        }

        public void Delete(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            if (!_countriesList.ContainsKey(countryCode))
                throw new Exception(noCountryExist);            
            _countriesList.Remove(countryCode);
            _storageHandler.Remove(countryCode);
        }

        public Country Get(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            if (!_countriesList.ContainsKey(countryCode))
                throw new Exception(noCountryExist);
            return new Country { Code = countryCode, Name = _countriesList[countryCode] };          
        }

        public void Update(Country country)
        {
            country.Code = country.Code.ToUpper();
            if (!_countriesList.ContainsKey(country.Code))
                throw new Exception(noCountryExist);
            _storageHandler.Update(country.Code, country.Name);
            _countriesList[country.Code] = country.Name;
        }
    }
}
