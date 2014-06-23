namespace ReSharperToolKit.Editors
{
    public interface iSourceEditor
    {
        /// <summary>
        /// Adds a new class to the source file and provides an editor for it.
        /// </summary>
        iClassEditor AddClass(string pIdentifier, bool pPublic = true);

        /// <summary>
        /// Adds a using declaration to the using list.
        /// </summary>
        void AddUsing(string pNameSpace);
    }
}