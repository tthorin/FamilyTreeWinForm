// -----------------------------------------------------------------------------------------------
//  AddRemoveCity.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace FamilyTreeWF.Forms
{
    using FamilyTreeWF.Helpers;
    using FamilyTreeWF.Models;
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class AddRemoveCity : Form
    {
        private List<City> cities = new();
        public AddRemoveCity(List<City> cityList)
        {
            cities = cityList;
            InitializeComponent();
            SetupDataForForm();
        }
        public AddRemoveCity()
        {
            InitializeComponent();
            SetupDataForForm();
        }

        private void B_addCity_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cb_cities.Text)) MessageBox.Show("Can not add a city without a name.", "Input error");
            else
            {
                var city = cities.Where(c => c.Name == cb_cities.Text).FirstOrDefault(new City() { CityId = 0, Name = cb_cities.Text });
                if (city.CityId == 0) AddCity(city);
                else RemoveCity(city);
            }
        }

        private void RemoveCity(City city)
        {
            const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            const MessageBoxIcon icon = MessageBoxIcon.Warning;
            var answer = MessageBox.Show($"Really delete {city.Name}?\n This action cannot be undone.", "Confirm delete", buttons, icon);
            if (answer == DialogResult.Yes)
            {
                var result = CityHelper.RemoveCity(city);
                if (result > 0) MessageBox.Show($"City {city.Name} removed from database.", "Success");
                this.Close();
            }
        }

        private void AddCity(City city)
        {
            city = TidyUpCityName(city);
            const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            var answer = MessageBox.Show($"Create city {city.Name}?", "Confirm add", buttons);
            if (answer == DialogResult.Yes)
            {
                city = CityHelper.AddCity(city);
                MessageBox.Show($"City {city.Name} added to the database with Id {city.CityId}", "Success");
                this.Close();
            }
        }

        private static City TidyUpCityName(City city)
        {
            string name = city.Name.Trim();
            if (name.Length >= 2) name = name[..1].ToUpper() + name[1..].ToLower();
            city.Name = name;
            return city;
        }

        private void SetupDataForForm()
        {
            if (cities.Count == 0) cities = CityHelper.GetAllCities();
            cb_cities.DataSource = null;
            cb_cities.DataSource = cities;
            cb_cities.DisplayMember = "Name";
            cb_cities.ValueMember = "CityId";
            cb_cities.SelectedIndex = -1;
        }
    }
}
