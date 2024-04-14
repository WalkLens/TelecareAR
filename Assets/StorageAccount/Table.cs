using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using UnityEngine;

namespace AzureStorage
{
    public class Table : TableEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }

        public Table() { }

        public Table(string name, int age, string description)
        {
            Name = name;
            Age = age;
            Description = description;
        }
    }
}