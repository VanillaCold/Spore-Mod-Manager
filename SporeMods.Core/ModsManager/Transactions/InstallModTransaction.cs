using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SporeMods.Core.Transactions;
using SporeMods.Core.Mods;
using System.Linq;

namespace SporeMods.Core.Mods
{
    public class InstallModTransaction : ModTransaction
    {
        ModJobBatchModEntry _entry = null;
        public InstallModTransaction(ModJobBatchModEntry entry)
        {
            _entry = entry;
            //ProgressSignifier = new TaskProgressSignifier(_entry.Mod.DisplayName, TaskCategory.Install);
        }

        public override async Task<bool> CommitAsync()
        {
            await OperationAsync(new AddModToRecordOp(_entry.Mod));
            //ProgressSignifier.Status = TaskStatus.Determinate;
            ZipArchive archive = null;
            Job.TrySetActivityRange(0, JobBase.PROGRESS_OVERALL_MAX / 2);


            ///This block of code is to ensure that updated versions of an already installed mod can be installed.
            ///TODO; ensure that multiple mods with the same unique identifier can't be installed simultaneously.
            Exception exception = null;
            List<ISporeMod> removalMods = new List<ISporeMod>();
            //Remove all mods with the same identifier as the mod being added
            foreach (ISporeMod existingMod in ModsManager.InstalledMods) //Loop through the entire modlist.
            {
                //If the mod's unique identifier is the same as one on the modlist,
                if (existingMod.Unique == _entry.Mod.Unique && existingMod != _entry.Mod)
                {
                    //Remove all files from the old mod
                    exception = await existingMod.PurgeAsync(this);
                    if (exception != null)
                    {
                        Exception = exception;
                        return false;
                    }

                    //And remove all files from the new mod too.
                    exception = await existingMod.RemoveRecordFilesAsync(this, false);
                    if (exception != null)
                    {
                        Exception = exception;
                        return false;
                    }
                    removalMods.Add(existingMod);
                }
            }



            foreach (ISporeMod mod in removalMods)
            {
                ModsManager.InstalledMods.Remove(mod);
            }

            await Task.Run(() =>
            {
                string extension = Path.GetExtension(_entry.ModPath);

                if (extension.Equals(ModUtils.MOD_FILE_EX_SPOREMOD, StringComparison.OrdinalIgnoreCase))
                    archive = ZipFile.OpenRead(_entry.ModPath);
            });

            exception = await _entry.Mod.ExtractRecordFilesAsync(this, _entry.ModPath, archive);
            archive?.Dispose();

            if (exception != null)
            {
                Exception = exception;
                return false;
            }

            Job.TrySetActivityRange(JobBase.PROGRESS_OVERALL_MAX / 2, JobBase.PROGRESS_OVERALL_MAX);

            exception = await _entry.Mod.ApplyAsync(this);
            if (exception != null)
            {
                Exception = exception;
                return false;
            }


            return true;
        }

        public override void Rollback()
        {
            base.Rollback();
            if (_entry == null)
                return;
            else if (_entry.Mod == null)
                return;
            
            /*if (ModsManager.InstalledMods.Contains(_entry.Mod))
            {
                ModsManager.InstalledMods.Remove(_entry.Mod);
            }*/
            //ModsManager.AddMod(_entry.Mod);
            //CompleteProgress(TaskStatus.Failed);
        }

        /*public override void Dispose()
        {
            base.Dispose();
            CompleteProgress(TaskStatus.Succeeded);
        }*/
    }
}