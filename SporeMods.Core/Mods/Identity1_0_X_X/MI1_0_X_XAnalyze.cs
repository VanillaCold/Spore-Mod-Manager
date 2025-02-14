﻿using SporeMods.Core.Mods.ModIdentity.V1_0_X_XComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SporeMods.Core.Mods
{
    public partial class MI1_0_X_XMod
    {
        Dictionary<string, MemoryStream> _imageStreams = new Dictionary<string, MemoryStream>();

        public virtual MemoryStream GetImageStream(string fileName)
            => _imageStreams.TryGetValue(fileName, out MemoryStream stream) ? stream : null;



        public override bool TryGetFromRecordDir(string subdirName, XDocument doc, out Exception error)
        {
            error = null;
            try
            {
                RecordDirName = subdirName;
                string subdirPath = Path.Combine(Settings.ModConfigsPath, subdirName);
                Version identityVersion = EnsureIdentityVersion(doc);
                string identityPath = Path.Combine(subdirPath, SporeMods.Core.Mods.ModUtils.ID_XML_FILE_NAME);
                var xmlRoot = doc.Root;
                if (!xmlRoot.TryGetAttributeValue("unique", out string unique))
                    throw new ModException(true, "_UUUUUUUU_ (PLACEHOLDER)");

                Unique = unique;

                var filePaths = Directory.EnumerateFiles(subdirPath);
                foreach (string f in filePaths)
                {
                    _fileNames.Add(Path.GetFileName(f));
                }
                WarningLabels.UsesCodeInjection = ModUtils.AreAnyFilesCustomCode(filePaths);

                ReadIdentity(doc); //TODO: Don't read in features right away?

                return true;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return false;
        }

        public bool TryAnalyzeIncomingSporemodFile(string inFilePath, ZipArchive archive, out Exception error)
        {
            error = null;
            try
            {
                if (!archive.TryGetEntry(ModUtils.ID_XML_FILE_NAME, out ZipArchiveEntry identityEntry))
                    throw new ModException(true, "_BBBBBBBB_ (PLACEHOLDER)");

                XDocument doc = null;
                using (StreamReader reader = new StreamReader(identityEntry.Open()))
                {
                    string text = reader.ReadToEnd();
                    doc = XDocument.Parse(text, ModUtils.ID_XML_LOAD_OPTIONS);
                }
                if (doc == null)
                    throw new ModException(false, "_CCCCCCC_ (PLACEHOLDER)");

                var xmlRoot = doc.Root;

                Version identityVersion = EnsureIdentityVersion(doc);


                if (!xmlRoot.TryGetAttributeValue("unique", out string unique))
                    throw new ModException(false, "_UUUUUUUU_ (PLACEHOLDER)");

                Unique = unique;

                string recordDirName = ModUtils.GetModsRecordDirNameFromString(unique);
                RecordDirName = recordDirName;


                var imageStreams = new Dictionary<string, MemoryStream>();

                foreach (var h in archive.Entries)
                {
                    string hRawName = h.Name;
                    string hName = Path.GetFileName(hRawName);
                    _fileNames.Add(hName);

                    if (Path.GetExtension(hName).Equals(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        MemoryStream copyStream = new MemoryStream();
                        using (var stream = h.Open())
                            stream.CopyTo(copyStream);

                        copyStream.Seek(0, SeekOrigin.Begin);
                        imageStreams.Add(hName, copyStream);
                    }

                }
                WarningLabels.UsesCodeInjection = ModUtils.AreAnyFilesCustomCode(_fileNames);


                IsIncoming = true;

                _imageStreams = imageStreams;

                ReadIdentity(doc);
                IsIncoming = false;

                return true;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return false;
        }


        List<string> _fileNames = new List<string>();
        /*public MI1_0_X_XMod(string recordDirName, string unique, List<string> fileNames)
        {
            RecordDirName = recordDirName;
            Unique = unique;
            _fileNames = fileNames;
            UsesCodeInjection = _fileNames.Any(x => Path.GetExtension(x).Equals(".dll", StringComparison.OrdinalIgnoreCase));
        }*/


        protected virtual void ReadIdentity(XDocument xml)
        {
            var xmlRoot = xml.Root;
            ReadIdentityRoot(xmlRoot);
            ReadIdentityComponents(xmlRoot);
        }


        protected virtual void ReadIdentityRoot(XElement xmlRoot)
        {
            if (xmlRoot.TryGetAttributeValue("displayName", out string displayName))
                DisplayName = new FixedModText(displayName);
            else
                DisplayName = new FixedModText(Unique);


            if (xmlRoot.TryGetAttributeValue("description", out string description))
                InlineDescription = new FixedModText(description);
        }

        protected void ReadIdentityComponents(XElement xmlRoot)
        {
            foreach (XElement el in xmlRoot.Elements())
            {
                if (TryReadIdentityComponent(el, out ComponentBase component))
                {
                    AllComponents.Add(component);
                    if ((component is Feature) || (component is RadioGroup))
                        FeatureComponents.Add(component);
                }
            }
        }

        protected virtual bool TryReadIdentityComponent(XElement xmlEl, out ComponentBase component)
        {
            string elName = xmlEl.Name.LocalName.ToLowerInvariant();

            component = null;
            
            if (elName == "prerequisite")
                component = Prerequisite.FromXml(this, xmlEl, _fileNames);
            else if (elName == "compatfile")
                component = CompatFile.FromXml(this, xmlEl, _fileNames);
            else if (elName == "componentgroup")
                component = RadioGroup.FromXml(this, xmlEl, _fileNames);
            else if (elName == "component")
                component = Feature.FromXml(this, xmlEl, _fileNames);

            return component != null;
        }
    }
}
