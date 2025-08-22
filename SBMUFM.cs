using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;

namespace shittyFileManager/*
    i keep forgeting that mod managers are just kinda like file managers for mods 
*/
{
    public class SBMUFM
    {
        private DirectoryInfo ryuRoot;
        private DirectoryInfo ryuSDCardUltimate;
        private DirectoryInfo smashAtmos;
        private DirectoryInfo ryuSDCard;

        private DirectoryInfo skylinePlugins;
        private DirectoryInfo ultimateMods;
        private DirectoryInfo rootDisabled;
        public SBMUFM() {
            RyuSlightSetup();
        }
        private void RyuSlightSetup()// a long function of creating folders and making sure it exists ig
        {
            //auto add like the sdcard stuff
            string RyuPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ryujinx";
            ryuRoot = new DirectoryInfo(RyuPath);
            if (!ryuRoot.Exists)
            {
                ryuRoot.Create();
            }
            ryuSDCard = new DirectoryInfo(ryuRoot.FullName + @"\sdcard");
            if (!ryuSDCard.Exists)
            {
                ryuSDCard.Create();
            }
            ryuSDCardUltimate = new DirectoryInfo(ryuSDCard.FullName + @"\ultimate");
            if (!ryuSDCardUltimate.Exists)
            {
                ryuSDCardUltimate.Create();
                ryuSDCardUltimate.CreateSubdirectory("mods");
                ultimateMods = new DirectoryInfo(ryuSDCardUltimate.FullName + @"\mods");
            }
            else
            {
                ultimateMods = new DirectoryInfo(ryuSDCardUltimate.FullName + @"\mods");
                if (!ultimateMods.Exists)
                {
                    ultimateMods.Create();
                }
            }
            rootDisabled = new DirectoryInfo(ryuSDCardUltimate.FullName + @"\_fisabled");
            if (!rootDisabled.Exists)
            {
                rootDisabled.Create();
                rootDisabled.CreateSubdirectory("nro");
                rootDisabled.CreateSubdirectory("mods");
            }
            else
            {
                DirectoryInfo nro = new DirectoryInfo(rootDisabled.FullName + @"\nro");
                DirectoryInfo mods = new DirectoryInfo(rootDisabled.FullName + @"\mods");
                if (!nro.Exists)
                {
                    nro.Create();
                }
                if (!mods.Exists)
                {
                    mods.Create();
                }
            }
            //auto add like the atmosphere folder stuff
            DirectoryInfo _atmosphere = new DirectoryInfo(ryuSDCard.FullName + @"\atmosphere");
            if (!_atmosphere.Exists)
            {
                _atmosphere.Create();
            }
            DirectoryInfo _contents = new DirectoryInfo(_atmosphere.FullName + @"\contents");
            if (!_contents.Exists)
            {
                _contents.Create();
            }
            smashAtmos = new DirectoryInfo(_contents.FullName + @"\01006a800016e000");
            if (!smashAtmos.Exists)
            {
                smashAtmos.Create();
                smashAtmos.CreateSubdirectory("romfs");
                smashAtmos.CreateSubdirectory("exefs");
            }
            else
            {
                DirectoryInfo _romfs = new DirectoryInfo(smashAtmos.FullName + @"\romfs");
                DirectoryInfo _exefs = new DirectoryInfo(smashAtmos.FullName + @"\exefs");
                if (!_romfs.Exists)
                {
                    _romfs.Create();
                }
                if (!_exefs.Exists)
                {
                    _exefs.Create();
                }
            }
            DirectoryInfo _skyline = new DirectoryInfo(smashAtmos.FullName + @"\romfs\skyline");
            if (!_skyline.Exists)
            {
                _skyline.Create();
                _skyline.CreateSubdirectory("plugins");
                skylinePlugins = new DirectoryInfo(_skyline.FullName + @"\plugins");//do i think theres a better way than to paste this line twice? mayhaps but am i gonna try and figure it out?? nah.
            }
            else
            {
                skylinePlugins = new DirectoryInfo(_skyline.FullName + @"\plugins");
                if (!skylinePlugins.Exists)
                {
                    skylinePlugins.Create();
                }
            }
        }
        public Dictionary<// disabled{}, enalbled{}, ..etc
            string, Dictionary<string, string>/*
                mod name and mod full filepath -- or maybe from the root of the sdcard idk what i decided yet
            */
            > getAllMods(string want = ""){// all of ts looks so ugly
            Dictionary<string, Dictionary<string, string>> returnValue = new();/*
                just realized after chaing it so i can pick whichever i want 
                that i can structure this better and use a single function to do all the repeating code but im not gonna!
            */
            if (want == "" || want.Contains('a') )
            {
                DirectoryInfo[] activeMods = ultimateMods.GetDirectories();
                Dictionary<string, string> emods = new();
                foreach (DirectoryInfo dir in activeMods) { emods.Add(dir.Name, dir.FullName); }
                returnValue.Add("emods", emods);
            }
            if (want == "" || want.Contains('b') )
            {
                FileInfo[] activePlugins = skylinePlugins.GetFiles();
                Dictionary<string, string> eplugins = new();
                foreach (FileInfo dir in activePlugins) { eplugins.Add(dir.Name, dir.FullName); }
                returnValue.Add("eplugins", eplugins);
            }
            if (want == "" || want.Contains('c') )
            {
                DirectoryInfo[] inactiveMods = new DirectoryInfo(rootDisabled.FullName + @"\mods").GetDirectories();
                Dictionary<string, string> imods = new();
                foreach (DirectoryInfo dir in inactiveMods) { imods.Add(dir.Name, dir.FullName); }
                returnValue.Add("imods", imods);
            }
            if (want == "" || want.Contains('d') ) 
            {
                FileInfo[] inactivePlugins = new DirectoryInfo(rootDisabled.FullName + @"\nro").GetFiles();
                Dictionary<string, string> iplugins = new();
                foreach (FileInfo dir in inactivePlugins) { iplugins.Add(dir.Name, dir.FullName); }
                returnValue.Add("iplugins", iplugins);
            }
            return returnValue;
        }
        public bool modState(string modpath)// just checks if file/dir path is in the disabled folder basicly a isDisabled variable
        {
            if (modpath == null)
            { 
                return false; 
            }
            return modpath.Contains(rootDisabled.FullName);
        }
        public void Toggle(string _thingToMove)
        {
            // i couldnt use var cause of implicity of something why couldnt it be like js </3
            bool isDisabled = modState(_thingToMove);
            bool isFile = _thingToMove.EndsWith(".nro");// terrible but honestly easier
            if (isFile)
            {
                FileInfo itemFile = new FileInfo(_thingToMove);
                try{
                    if (isDisabled)
                    {
                        File.Move(_thingToMove, skylinePlugins.FullName + "\\" + itemFile.Name, true);
                    }
                    else
                    {
                        File.Move(_thingToMove, rootDisabled.FullName + @"\nro\" + itemFile.Name, true);
                    }
                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            } else
            {
                try{
                    DirectoryInfo itemDir = new DirectoryInfo(_thingToMove);
                    if (isDisabled)
                    {
                        Directory.Move(_thingToMove, ultimateMods.FullName + "\\" + itemDir.Name);
                    }
                    else
                    {
                        Directory.Move(_thingToMove, rootDisabled.FullName + @"\mods\" + itemDir.Name);
                    }
                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
        }
        private string getZip(string name)// took ts from the microsoft docs about ZipFile
        {
            string path = @".\" + name + ".zip";

            using (OpenFileDialog gay = new OpenFileDialog())
            {
                gay.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
                gay.Filter = name+"latest zip Archive |*.zip";

                if (gay.ShowDialog() == DialogResult.OK)
                {
                    path = gay.FileName;
                }
            }
            return path;
        }
        private bool checkFileInZip(string zipPath,string[] names)//practically to check if the zipfile is worth it kindof a hardcoded way to check if its the reall ARCropolis or skyline but what works works
        {
            bool returnValue = false;
            using (ZipArchive zip = ZipFile.OpenRead(zipPath))
            {
                int found = 0;
                foreach (ZipArchiveEntry probablyafile in zip.Entries)
                {
                    foreach (string fileName in names)
                    {
                        if (probablyafile.Name.Contains(fileName))
                        {
                            found++;
                            break;
                        }
                    }
                }
                if (found == names.Length)
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }
        public void addArc()
        {
            string thepath = getZip("ARCropolis");
            FileInfo israel = new(thepath);
            if (!israel.Exists)
            {
                return;
            }
            bool theOne = checkFileInZip(thepath, ["libarcropolis.nro"]);
            if (!theOne)
            {
                return;
            }
            ZipFile.ExtractToDirectory(thepath, ryuSDCard.FullName, true);
        }
        public void addSkyline()
        {
            string thepath = getZip("Skyline");
            FileInfo israel = new(thepath);
            if (!israel.Exists)
            {
                return;
            }
            bool theOne = checkFileInZip(thepath, ["main.npdm", "subsdk9"]);// for all i care they could rebuild it for another game and i wouldnt know!
            if (!theOne)
            {
                return;
            }
            ZipFile.ExtractToDirectory(thepath, smashAtmos.FullName, true);
        }
        public string infoToml(string modPath, int mode = 0,
                string displayName = "Mod name goes here", 
                string auth = "author(s) go here", 
                string ver = "1.0", 
                string desc = "Description for mod goes here", 
                string cat = "Misc")/*
                category = "Fighter, Stage, Effects, UI, Param, Audio, Misc"
            */
        {
            FileInfo toml = new FileInfo(modPath + @"\info.toml");
            if (!toml.Exists && mode == 0) {
                return "Doesnt exist";
            }
            if (mode == 0) //just get from the file
            {
                ///toml.
            }
            else if (mode == 1)//overwrite to the file or create it
            {

            }
            return "";
        }

    }
}
