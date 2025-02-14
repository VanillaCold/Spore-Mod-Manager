﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace SporeMods.Core.Mods
{
    public static class ModUtils
    {
        public const string MOD_FILE_EX_SPOREMOD = ".sporemod";
        public const string MOD_FILE_EX_DBPF = ".package";
        
        public const string MOD_SUBFILE_EX_DLL = ".dll";

        public const string ID_XML_FILE_NAME = "ModInfo.xml";
        public static readonly LoadOptions ID_XML_LOAD_OPTIONS = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;

        public static readonly Version ID_VER_1_0_0_0 = new Version(1, 0, 0, 0);
        public static readonly Version ID_VER_1_0_1_0 = new Version(1, 0, 1, 0);
        public static readonly Version ID_VER_1_0_1_1 = new Version(1, 0, 1, 1);
        public static readonly Version ID_VER_1_0_1_2 = new Version(1, 0, 1, 2);
        public static bool IsID_1_0_1_X(Version version)
                => (version == ID_VER_1_0_1_0)
                || (version == ID_VER_1_0_1_1)
                || (version == ID_VER_1_0_1_2)
                ;
        public static bool IsID_1_0_X_X(Version version)
            => (version == ID_VER_1_0_0_0)
            || IsID_1_0_1_X(version)
            ;

        public static readonly Version ID_VER_GRANDFATHER = new Version(1, 1, 0, 0);


        public const string AT_UNIQUE = "unique";
        public const string AT_DISP_NAME = "displayName";

        static readonly IReadOnlyList<char> _INVALID_PATH_CHARS = new Func<IReadOnlyList<char>>(() =>
        {
            List<char> chars = Path.GetInvalidFileNameChars().ToList();
            var invalidPathChars = Path.GetInvalidPathChars();
            foreach (char ch in invalidPathChars)
            {
                if (chars.Contains(ch))
                    continue;

                chars.Add(ch);
            }
            return chars.AsReadOnly();
        })();


        public static string GetModsRecordDirNameFromFilePath(string filePath)
            => GetModsRecordDirNameFromString(Path.GetFileNameWithoutExtension(filePath));

        public static string GetModsRecordDirNameFromString(string str)
        {
            string modsRecordDirName = str;
            foreach (char c in _INVALID_PATH_CHARS)
            {
                modsRecordDirName = modsRecordDirName.Replace(c, '-');
            }
            return modsRecordDirName;
        }

        public static bool IsIncomingModFile(string filePath, out bool isSporeMod)
        {
            string ext = Path.GetExtension(filePath);
            if (MOD_FILE_EX_SPOREMOD.Equals(ext, StringComparison.OrdinalIgnoreCase))
            {
                isSporeMod = true;
                return true;
            }
            if (MOD_FILE_EX_DBPF.Equals(ext, StringComparison.OrdinalIgnoreCase))
            {
                isSporeMod = false;
                return true;
            }

            isSporeMod = false;
            return false;
        }

        public static bool HasSettings(this ISporeMod mod, out IConfigurableMod cfgMod)
        {
            cfgMod = null;

            if (mod is IConfigurableMod cMod)
            {
                cfgMod = cMod;
                return cMod.HasSettings;
            }

            return false;
        }

        public static void WhenWarningPropertyChanged(this ISporeMod mod, bool value, ref ObservableCollection<ModWarningLabel> warningLabels, [CallerMemberName] string propertyName = "")
        {
            Cmd.WriteLine($"WhenWarningPropertyChanged: {propertyName}={value}");
            if (!Enum.TryParse<ModWarningLabel.WarningLabelType>(propertyName, out ModWarningLabel.WarningLabelType labelType))
                return;
            Cmd.WriteLine("Parsed!");

            ModWarningLabel label = ModWarningLabel.Labels[labelType];
            bool containsLabel = warningLabels.Contains(label);
            if (value != containsLabel)
            {
                if (value)
                    warningLabels.Add(label);
                else
                    warningLabels.Remove(label);
            }
            /*else
            {
                /*label = WarningLabels.FirstOrDefault(x => x.LabelType == labelType);
                if (label != null)* /
                if (WarningLabels.Contains(label))
                    WarningLabels.Remove(label);
            }*/
        }

        public static bool AreAnyFilesCustomCode(IEnumerable<string> fileNames)
        {
            foreach (string fileName in fileNames)
            {
                if (Path.GetExtension(fileName).Equals(MOD_SUBFILE_EX_DLL, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
