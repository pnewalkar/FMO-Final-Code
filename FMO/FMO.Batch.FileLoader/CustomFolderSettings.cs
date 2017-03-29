using System.Xml.Serialization;

namespace FMO.Batch.FileLoader
{
    public class CustomFolderSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFolderSettings"/> class.Default constructor of the class
        /// </summary>
        public CustomFolderSettings()
        {
        }

        /// <summary> Gets or sets unique identifier of the combination File type/folder.
        /// Arbitrary number (for instance 001, 002, and so on)</summary>
        [XmlAttribute]
        public string FolderId { get; set; }

        /// <summary>Gets or sets a value indicating whether if TRUE: the file type and folder will be monitored</summary>
        [XmlElement]
        public bool FolderEnabled { get; set; }

        /// <summary>Gets or sets description of the type of files and folder location –
        /// Just for documentation purpose</summary>
        [XmlElement]
        public string FolderDescription { get; set; }

        /// <summary>Gets or sets filter to select the type of files to be monitored.
        /// (Examples: *.shp, *.*, Project00*.zip)</summary>
        [XmlElement]
        public string FolderFilter { get; set; }

        /// <summary>Gets or sets full path to be monitored
        /// (i.e.: D:\files\projects\shapes\ )</summary>
        [XmlElement]
        public string FolderPath { get; set; }

        /// <summary>Gets or sets a value indicating whether if TRUE: the folder and its subfolders will be monitored</summary>
        [XmlElement]
        public bool FolderIncludeSub { get; set; }

        /// <summary>Gets or sets the command or action to be executed
        /// after an event has raised</summary>
        [XmlElement]
        public string ExecutableFile { get; set; }

        /// <summary>Gets or sets list of arguments to be passed to the executable file</summary>
        [XmlElement]
        public string ExecutableArguments { get; set; }
    }
}