using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BubblesEngine.Helpers;

namespace BubblesEngine.Engines.Implementations
{
    public class DomainFs : IDomainFs
    {
        public async Task<bool> WriteFile(string path, string content)
        {
            try{
                if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(content)) return false;
                await File.WriteAllTextAsync(path, content);
                return true;
            }
            catch (IOException e){
                Logger.LogError(nameof(DomainFs), e.ToString());
            }

            return false;
        }

        public async Task<string> ReadFile(string path)
        {
            try{
                if (string.IsNullOrEmpty(path)) return null;
                var content = await File.ReadAllTextAsync(path);
                return content;
            }
            catch (IOException e){
                Logger.LogError(nameof(DomainFs), e.ToString());
            }

            return null;
        }

        public bool IsExists(string path)
        {
            return File.Exists(path);
        }

        public bool CreateDirectory(string path)
        {
            try{
                if (string.IsNullOrEmpty(path)) return false;
                Directory.CreateDirectory(path);
                return true;
            }
            catch (IOException e){
                Logger.LogError(nameof(DomainFs), e.ToString());
            }

            return false;
        }

        public List<string>? ListDirectories(string path)
        {
            try{
                if (string.IsNullOrEmpty(path)) return null;
                return new List<string>(Directory.GetDirectories(path));
            }
            catch (Exception e){
                Logger.LogError(nameof(DomainFs), e.ToString());
            }

            return null;
        }

        public List<string>? ListFiles(string path)
        {
            try{
                return string.IsNullOrEmpty(path) ? new List<string>() : new List<string>(Directory.GetFiles(path));
            }
            catch (IOException e){
                Logger.LogError(nameof(DomainFs), e.ToString());
            }

            return null;
        }
    }
}