using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperToolKit.Daemons
{
    /// <summary>
    /// A reusable base for creating daemon processes for a specific language and file type.
    /// </summary>
    public abstract class GenericDaemonStage<TLanguageType, TFileType> : IDaemonStage
        where TLanguageType : PsiLanguageType
        where TFileType : class, IFile
    {
        /// <summary>
        /// Called to create processors only when a C sharp file is being processed.
        /// </summary>
        protected abstract IEnumerable<IDaemonStageProcess> CreateProcessByFile(IDaemonProcess pProcess,
                                                                                IContextBoundSettingsStore pSettings,
                                                                                DaemonProcessKind pProcessKind,
                                                                                TFileType pFile);

        /// <summary>
        /// Creates processors for C sharp files.
        /// </summary>
        public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess pProcess,
                                                              IContextBoundSettingsStore pSettings,
                                                              DaemonProcessKind pProcessKind)
        {
            IPsiSourceFile sourceFile = pProcess.SourceFile;
            IPsiServices psiServices = sourceFile.GetPsiServices();
            TFileType file = psiServices.Files.GetDominantPsiFile<TLanguageType>(sourceFile) as TFileType;

            return file == null
                ? Enumerable.Empty<IDaemonStageProcess>()
                : CreateProcessByFile(pProcess, pSettings, pProcessKind, file);
        }

        /// <summary>
        /// Check the error stripe indicator necessity for this stage after processing given <paramref name="pSourceFile"/>
        /// </summary>
        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile pSourceFile, IContextBoundSettingsStore pSettingsStore)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }
    }
}