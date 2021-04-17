using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region On Loaded
        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Loops through the logical drives on the computer
                foreach (var drive in Directory.GetLogicalDrives())
                {
                    // Create a new item for it
                    var item = new TreeViewItem()
                    {
                        // Set the header
                        Header = drive,
                        // Set the full path
                        Tag = drive
                    };    

                    // Add a shitty item
                    item.Items.Add(null);

                    // Listen out for item being expanded
                    item.Expanded += Folder_Expanded;

                    // Add it to the main tree-view
                    FolderView.Items.Add(item);
                }
            }
            catch (System.IO.IOException)
            {
                System.Console.WriteLine("An I/O error occurs.");
            }
            catch (System.Security.SecurityException)
            {
                System.Console.WriteLine("The caller does not have the " +
                    "required permission.");
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            // If the item only contains the shitty data
            if (item.Items.Count != 1 || item.Items[0] == null)
                return;

            // Clear shitty data
            item.Items.Clear();

            // Get full path
            var fullPath = (String)item.Tag;

            // Create a new list for all direcroties
            var directories = new List<String>();

            // Try and get directories , Ignoring any issues. 
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if(dirs.Length > 0)
                {
                    directories.AddRange(dirs);
                }
            }
            catch{}

            // For each directory
            directories.ForEach(directoryPath =>
            {
                // Create directory item
                var subItem = new TreeViewItem()
                {
                    // Set header as folder name
                    Header = Path.GetDirectoryName(directoryPath),
                    // And tag as full path
                    Tag = directoryPath
                };

                // Add a shitty item so we can expand folder
                subItem.Items.Add(null);

                // Handle expanding (recursive)
                subItem.Expanded += Folder_Expanded;

                // Add this item to the parent
                item.Items.Add(subItem);

            });

        }
        #endregion
    }
}
