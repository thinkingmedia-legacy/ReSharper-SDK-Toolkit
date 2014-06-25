using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperToolKit.Daemons
{
    public abstract class GenericDaemonProcessor<TNodeType, TLanguageType, TFileType> : IDaemonStageProcess
        where TNodeType : class, ITreeNode
        where TLanguageType : PsiLanguageType
        where TFileType : class, IFile
    {
        /// <summary>
        /// The current process
        /// </summary>
        private readonly IDaemonProcess _process;

        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericDaemonProcessor(IDaemonProcess pProcess)
        {
            _process = pProcess;
        }

        /// <summary>
        /// Derived classes provide highlighting info when TreeNodes are found for the given language and file type.
        /// </summary>
        protected abstract IEnumerable<HighlightingInfo> getHighlights(TFileType pFile, TNodeType pNode);

        /// <summary>
        /// Executes the process.
        /// </summary>
        public void Execute(Action<DaemonStageResult> pCommitter)
        {
            if (!_process.FullRehighlightingRequired)
            {
                return;
            }

            List<HighlightingInfo> highlights = new List<HighlightingInfo>();
            IPsiSourceFile sourceFile = _process.SourceFile;
            IPsiServices psiServices = sourceFile.GetPsiServices();
            TFileType file = psiServices.Files.GetDominantPsiFile<TLanguageType>(sourceFile) as TFileType;
            if (file == null)
            {
                return;
            }

            file.ProcessChildren<TNodeType>(pNode=>highlights.AddRange(getHighlights(file, pNode)));

            if (highlights.Count == 0)
            {
                return;
            }

            pCommitter(new DaemonStageResult(highlights));
        }

        /// <summary>
        /// Whole daemon process
        /// </summary>
        public IDaemonProcess DaemonProcess
        {
            get { return _process; }
        }
    }
}