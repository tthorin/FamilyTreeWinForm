// -----------------------------------------------------------------------------------------------
//  AddRemoveCountry.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace FamilyTreeWF.Forms
{
    using FamilyTreeWF.Helpers;
    using FamilyTreeWF.Models;
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class AddRemoveCountry : Form
    {
        private List<Country> countries = new();
        public AddRemoveCountry(List<Country> countryList)
        {
            countries = countryList;
            InitializeComponent();
            SetupDataForForm();
        }
        public AddRemoveCountry()
        {
            InitializeComponent();
            SetupDataForForm();
        }

        private void B_addCountry_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cb_countries.Text)) MessageBox.Show("Can not add a country without a name.", "Input error");
            else
            {
                var country = countries.Where(c => c.Name == cb_countries.Text).FirstOrDefault(new Country() { CountryId = 0, Name = cb_countries.Text });
                if (country.CountryId == 0) AddCountry(country);
                else RemoveCountry(country);
            }
        }

        private void RemoveCountry(Country country)
        {
            const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            const MessageBoxIcon icon = MessageBoxIcon.Warning;
            var answer = MessageBox.Show($"Really delete {country.Name}?\n This action cannot be undone.", "Confirm delete", buttons, icon);
            if (answer == DialogResult.Yes)
            {
                var result = CountryHelper.RemoveCountry(country);
                if (result > 0) MessageBox.Show($"Country {country.Name} removed from database.", "Success");
            }
            this.Close();
        }

        private void AddCountry(Country country)
        {
            country = TidyUpCountryName(country);
            const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            var answer = MessageBox.Show($"Create country {country.Name}?", "Confirm add", buttons);
            if (answer == DialogResult.Yes)
            {
                country = CountryHelper.AddCountry(country);
                MessageBox.Show($"City {country.Name} added to the database with Id {country.CountryId}", "Success");
                this.Close();
            }
        }

        private static Country TidyUpCountryName(Country country)
        {
            string name = country.Name.Trim();
            if (name.Length >= 2) name = name[..1].ToUpper() + name[1..].ToLower();
            country.Name = name;
            return country;
        }

        private void SetupDataForForm()
        {
            if (countries.Count == 0) countries = CountryHelper.GetAllCountries();
            cb_countries.DataSource = null;
            cb_countries.DataSource = countries;
            cb_countries.DisplayMember = "Name";
            cb_countries.ValueMember = "CountryId";
            cb_countries.SelectedIndex = -1;
        }
    }
}
