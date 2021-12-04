﻿// -----------------------------------------------------------------------------------------------
//  Country.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace FamilyTreeWF.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";

        [InverseProperty("BirthCountry")]
        public List<Person>? PeopleBorn { get; set; }
        [InverseProperty("DeathCountry")]
        public List<Person>? PeopleDead { get; set; }

    }
}
