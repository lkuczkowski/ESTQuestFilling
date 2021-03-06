﻿using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ESTQuestFilling.Model
{
    class Company
    {
        public string Name { get; }
        public List<Checkpoint> CheckpointsList { get; }

        public Company(string name)
        {
            Name = name;
            CheckpointsList = new List<Checkpoint>();
        }

        public void AddCheckpoint(Checkpoint checkpoint)
        {
            CheckpointsList.Add(checkpoint);
        }

        // TODO - sprawdzać czy istnieją już pliki i pytać czy nadpisać
        // TODO - do rozwiązania problem z nazwą pliki ze "/" i "\"
        public void WriteInstitutionCodeFiles()
        {
            System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();
            if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string companyDirectory = $"{f.SelectedPath}\\{Name}";
            if (!Directory.Exists(companyDirectory))
            {
                System.IO.Directory.CreateDirectory(companyDirectory);
            }
            string path = "error";
            foreach (var checkpoint in CheckpointsList)
            {
                path = $"{companyDirectory}\\{Name} - {checkpoint.Name}.xml";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(checkpoint.GetCheckpointCode());
                    }
                }
            }

            MessageBox.Show($"Zapisano punkty kontrolna dla {Name}\n\n Ścieżka: {companyDirectory}");
        }
    }
}
