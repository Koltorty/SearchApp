using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using SearchApp.Extensions;
using SearchApp.Models;

namespace SearchApp.Services
{
    public class SearchService
    {
        private static SearchService instance;

        private SearchService() {}

        public static SearchService GetSearchService()
        {
            return instance ?? (instance = new SearchService());
        }
        
        public string GetCatalogPath()
        {
            var openFileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            var directoryName = string.Empty;

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                directoryName = openFileDialog.FileName;
            }

            return directoryName;
        }
        
        public IEnumerable<SearchResultModel> Search(
            string directoryPath,
            string query,
            string fileSize,
            DateTime? date,
            bool? searchInSubDirectories)
        {
            var subDirectories = new List<string>();
            var files = new List<FileInfo>();

            if (searchInSubDirectories.HasValue && searchInSubDirectories.Value)
            {
                subDirectories = Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories).ToList();
            }
            else
            {
                subDirectories.Add(directoryPath);
            }

            foreach (var subDirectory in subDirectories)
            {
                files.AddRange(GetFiles(subDirectory, fileSize, date));
            }

            return files.Select(file => GetSearchResult(file, query));
        }

        private static IEnumerable<FileInfo> GetFiles(string directoryPath, string fileSize, DateTime? date)
        {
            long size = 0;
            var files = new DirectoryInfo(directoryPath).GetFiles();
            
            if (!string.IsNullOrEmpty(fileSize))
            {
                double.TryParse(fileSize, NumberStyles.Float, new NumberFormatInfo(), out var doulbeSize);
                size = (long)doulbeSize * 1024 * 1024;
            }

            return files
                .FilterOnTextFiles()
                .Where(file => date.HasValue && file.CreationTime <= date)
                .Where(file => size != 0 && file.Length <= size);
        }

        private static SearchResultModel GetSearchResult(FileInfo file, string query)
        {
            var stringBuilder = new StringBuilder();

            using (var streamReader = file.OpenText())
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Contains(query))
                    {
                        stringBuilder.AppendLine(line);
                    }
                }
            }

            if (stringBuilder.Length != 0)
            {
                return new SearchResultModel
                {
                    FileName = file.FullName,
                    Lines = stringBuilder.ToString()
                };
            }

            return null;
        }
    }
}
